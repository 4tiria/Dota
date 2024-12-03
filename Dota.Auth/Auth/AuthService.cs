using Domain.Mongo.Auth.Models.Entities;
using Dota.Auth.Helpers;
using Dota.Auth.Models.DTO.Requests;
using Dota.Auth.Models.DTO.Responses;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Collections.Generic;
using Domain.Mongo.Auth;
using Domain.Mongo.Auth.Models;
using Microsoft.Extensions.Options;
using Dota.Auth.Email;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Dota.Auth.Auth;

public class AuthService(
    MongoDbContext context, 
    IEmailService emailService, 
    IOptions<AuthOptions> authOptions, 
    IConfiguration configuration,
    TokenValidationParameters tokenValidationParameters) : IAuthService
{
    private readonly MongoDbContext _context = context;
    private readonly IEmailService _emailService = emailService;
    private readonly IOptions<AuthOptions> _authOptions = authOptions;
    private readonly IConfiguration _configuration = configuration;
    private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;

    public AuthResponse Login(Account account)
    {
        var jwt = Generate.Jwt(account, _authOptions.Value);

        _context.RefreshTokens.DeleteMany(x => x.AccountId == account.Id);

        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);

        //TODO: DTO заменить на обычные классы из репозитория, хуй знает маппер мб нужен
        return new AuthResponse()
        {
            AccessToken = jwtString,
            AccountResponse = new AccountResponse()
            {
                Avatar = account.Avatar,
                Email = account.Email,
                Id = account.Id,
                AccessLevel = account.AccessLevel,
                IsConfirmed = account.IsConfirmed,
                UserName = account.UserName
            }
        };
    }

    public void Register(RegistrationRequest registrationRequest, Guid userGuid, string callbackUrl)
    {
        var hashCodePassword = registrationRequest.Password.PasswordToStringHashCode();

        var newAccount = new Account()
        {
            Id = userGuid,
            Avatar = null,
            UserName = registrationRequest.UserName,
            Email = registrationRequest.Email,
            Password = hashCodePassword,
            AccessLevel = registrationRequest.AccessLevel,
            ConfirmationLink = callbackUrl,
            ConfirmationExpiration =
                DateTime.UtcNow.AddDays(int.Parse(_configuration["Confirmation:ExpiresInDays"])),
        };

        // Перенести в EmailService
        _emailService.SendEmail(registrationRequest.Email,
            $"Для подтверждения почты перейдите по ссылке: <a href='{callbackUrl}'>{callbackUrl}</a>");

        newAccount.ConfirmationLink = callbackUrl;
        _context.Accounts.InsertOne(newAccount);
    }

    public string GetRefreshedAccessToken(AuthResponse authResponse, string refreshToken)
    {
        var validatedJwt = GetPrincipalFromToken(authResponse.AccessToken);
        if (validatedJwt == null)
        {
            //token is invalid or smth idk
            return null;
        }

        var expiryDateUnix = long.Parse(validatedJwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        var storedRefreshToken = _context.RefreshTokens.Find(x => x.Token == refreshToken).SingleOrDefault()
            ?? throw new NotImplementedException("this refreshToken does not exist");

        if (DateTime.UtcNow > storedRefreshToken.ExpireDate)
        {
            throw new NotImplementedException("the refreshToken has expired");
        }

        var jti = validatedJwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        var account = _context.Accounts.Find(x => x.Id == storedRefreshToken.AccountId).SingleOrDefault();

        var newAccessToken = Generate.Jwt(account, _authOptions.Value);
        var newAccessTokenString = new JwtSecurityTokenHandler().WriteToken(newAccessToken);

        //storedRefreshToken.AccessTokenId =
        //    newAccessToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        _context.RefreshTokens.FindOneAndUpdate(
            refreshToken => refreshToken.Token == storedRefreshToken.Token,
            Builders<RefreshToken>.Update
                .Set(refreshToken => refreshToken.Used, true));

        return newAccessTokenString;
    }

    private ClaimsPrincipal GetPrincipalFromToken(string jwt)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var principal = jwtHandler.ValidateToken(jwt, _tokenValidationParameters, out var validatedJwt);
        return IsJwtWithValidSecurityAlgorithm(validatedJwt) ? principal : null;
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }


    public Account AuthenticateUser(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return null;

        var passwordHashCode = password.PasswordToStringHashCode();
        return _context.Accounts.Find(x => x.Email == email && x.Password == passwordHashCode).FirstOrDefault();
    }

    public string CreateRefreshToken(Guid accountId)
    {
        var authParams = _authOptions.Value;

        var jwtId = Guid.NewGuid().ToString();
        var securityKey = authParams.GetSymmetricSecurityKey();
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expirationDate = DateTime.UtcNow.AddMonths(authParams.RefreshTokenLifeTimeInMonths);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, jwtId),
            new(JwtRegisteredClaimNames.Sub, accountId.ToString()), // Sub обычно используется для хранения идентификатора пользователя
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var refreshJwt = new JwtSecurityToken(
            issuer: authParams.Issuer,
            audience: authParams.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expirationDate,
            signingCredentials: credentials);

        var refreshJwtString = new JwtSecurityTokenHandler().WriteToken(refreshJwt);

        var refreshToken = new RefreshToken
        {
            Token = refreshJwtString,
            AccountId = accountId,
            CreationDate = DateTime.UtcNow,
            ExpireDate = expirationDate,
        };

        _context.RefreshTokens.InsertOne(refreshToken);

        return refreshJwtString;
    }


}
