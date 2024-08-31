using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.NoSql.Auth;
using Domain.NoSql.Auth.Models;
using Domain.NoSql.Auth.Models.Entities;
using Dota.Auth.Helpers;
using Dota.Auth.Models;
using Dota.Auth.Models.DTO.Requests;
using Dota.Auth.Models.DTO.Responses;
using Dota.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Dota.Auth.Controllers
{
    [ApiController, Route("auth")]
    public class AccountController(MongoDbContext context,
        IOptions<AuthOptions> authOptions,
        TokenValidationParameters tokenValidationParameters,
        IConfiguration configuration,
        IEmailService emailService) : Controller
    {
        private readonly MongoDbContext _context = context;
        private readonly IOptions<AuthOptions> _authOptions = authOptions;
        private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;
        private readonly IConfiguration _configuration = configuration;
        private readonly IEmailService _emailService = emailService;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest is null)
                return BadRequest();

            var account = AuthenticateUser(loginRequest.Email, loginRequest.Password);
            if (account is null) 
                return Unauthorized();

            var contextAccount = _context.Accounts.Find(x => x.Email == loginRequest.Email).FirstOrDefault();
            if (contextAccount is null)
                return NotFound();

            var jwt = Generate.Jwt(account, _authOptions.Value);
            _context.RefreshTokens.DeleteMany(x => x.AccountId == contextAccount.Id);
            var refreshTokenString = CreateRefreshToken(contextAccount.Id);

            var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);
            Response.Cookies.Append(Cookie.REFRESH_TOKEN, refreshTokenString, new CookieOptions() { HttpOnly = true });
            return Ok(new AuthResponse()
            {
                AccessToken = jwtString,
                AccountResponse = new AccountResponse()
                {
                    Avatar = contextAccount.Avatar,
                    Email = contextAccount.Email,
                    Id = contextAccount.Id,
                    AccessLevel = contextAccount.AccessLevel,
                    IsConfirmed = contextAccount.IsConfirmed,
                    UserName = contextAccount.UserName
                }
            });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] AuthResponse request)
        {
            if (request is null)
                return BadRequest();

            if (!Request.Cookies.TryGetValue(Cookie.REFRESH_TOKEN, out var refreshToken) || refreshToken == null)
                return Unauthorized();

            var tokens = GetRefreshedAccessToken(request, refreshToken);
            Response.Cookies.Append(Cookie.REFRESH_TOKEN, refreshToken, new CookieOptions() { HttpOnly = true });
            return Ok(tokens);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationRequest registrationRequest)
        {
            var hashCodePassword = registrationRequest.Password.PasswordToStringHashCode();
            if (_context.Accounts.Find(x => x.Email == registrationRequest.Email).Any())
                return BadRequest("user with this email already registered");

            var confirmationJwtToken = GenerateEmailConfirmationJwtToken();
            var confirmationJwtTokenString = new JwtSecurityTokenHandler().WriteToken(confirmationJwtToken);
            var guid = new Guid();
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = guid, code = confirmationJwtTokenString },
                protocol: HttpContext.Request.Scheme);

            var newAccount = new Account()
            {
                Id = guid,
                Avatar = null,
                UserName = registrationRequest.UserName,
                Email = registrationRequest.Email,
                Password = hashCodePassword,
                AccessLevel = registrationRequest.AccessLevel,
                ConfirmationLink = callbackUrl,
                ConfirmationExpiration =
                    DateTime.UtcNow.AddDays(int.Parse(_configuration["Confirmation:ExpiresInDays"])),
            };

            _emailService.SendEmail(registrationRequest.Email,
                $"Для подтверждения почты перейдите по ссылке: <a href='{callbackUrl}'>{callbackUrl}</a>");

            newAccount.ConfirmationLink = callbackUrl;
            _context.Accounts.InsertOne(newAccount);

            return Login(new LoginRequest()
            {
                Email = registrationRequest.Email,
                Password = registrationRequest.Password,
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest logoutRequest)
        {
            var refreshToken = _context.RefreshTokens.FindOneAndDelete(x => x.AccountId.ToString() == logoutRequest.AccountId);

            if (refreshToken is null)
                return NotFound();

            if (Request.Cookies.TryGetValue(Cookie.REFRESH_TOKEN, out _))
                Response.Cookies.Delete(Cookie.REFRESH_TOKEN);

            return Ok();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string code)
        {
            if (code is null)
                return BadRequest();

            var updatedAccount = await _context.Accounts.FindOneAndUpdateAsync(
                account => account.Id == userId,
                Builders<Account>.Update
                    .Set(account => account.IsConfirmed, true)
                    .Set(account => account.ConfirmationLink, null)
            );

            if (updatedAccount == null)
                return NotFound();

            return Redirect($"{_configuration["Client:Url"]}");
        }

        private string GetRefreshedAccessToken(AuthResponse authResponse, string refreshToken)
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

        private Account AuthenticateUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) 
                return null;

            var passwordHashCode = password.PasswordToStringHashCode();
            return _context.Accounts.Find(x => x.Email == email && x.Password == passwordHashCode).FirstOrDefault();
        }

        private string CreateAndSaveRefreshToken_Obsolete(JwtSecurityToken jwt, Guid accountId)
        {
            var authParams = _authOptions.Value;
            var generatedKeyForRefreshToken = Generate.TokenId();
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtId = jwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, generatedKeyForRefreshToken),
                new(JwtRegisteredClaimNames.Email, accountId.ToString()),
            };

            var expirationDate = DateTime.Now.AddMonths(_authOptions.Value.RefreshTokenLifeTimeInMonths);
            var refreshJwt = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims: claims,
                expires: expirationDate,
                signingCredentials: credentials);

            var refreshJwtString = new JwtSecurityTokenHandler().WriteToken(refreshJwt);
            var refreshToken = new RefreshToken()
            {
                Token = refreshJwtString,
                AccessTokenId = jwtId,
                AccountId = accountId,
                CreationDate = DateTime.Now,
                ExpireDate = expirationDate,
            };

            _context.RefreshTokens.InsertOne(refreshToken);

            return refreshJwtString;
        }

        private string CreateRefreshToken(Guid accountId)
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

        private JwtSecurityToken GenerateEmailConfirmationJwtToken()
        {
            var id = Generate.TokenId();
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, id),
            };

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMonths(1));

            return jwt;
        }
    }
}