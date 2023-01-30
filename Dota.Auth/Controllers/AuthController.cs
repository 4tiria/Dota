using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dota.Auth.Helpers;
using Dota.Auth.Models;
using Dota.Auth.Models.DTO.Requests;
using Dota.Auth.Models.DTO.Responses;
using Dota.Auth.Models.Entities;
using Dota.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dota.Auth.Controllers
{
    [ApiController, Route("auth")]
    public class AccountController : Controller
    {
        private readonly AuthContext _context;
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AccountController(AuthContext context,
            IOptions<AuthOptions> authOptions,
            TokenValidationParameters tokenValidationParameters,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _context = context;
            _authOptions = authOptions;
            _tokenValidationParameters = tokenValidationParameters;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest is null)
                return BadRequest();

            var account = AuthenticateUser(loginRequest.Email, loginRequest.Password);
            if (account is null) return Unauthorized();
            var contextAccount = _context.Accounts.FirstOrDefault(x => x.Email == loginRequest.Email);
            if (contextAccount is null)
                return NotFound();

            var jwt = Generate.Jwt(account, _authOptions.Value);
            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(x => x.AccountId == contextAccount.Id));
            var refreshTokenString = CreateAndSaveRefreshToken(jwt, contextAccount.Id);

            var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);
            Response.Cookies.Append(Cookie.RefreshToken, refreshTokenString, new CookieOptions() { HttpOnly = true });
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

            if (!Request.Cookies.TryGetValue(Cookie.RefreshToken, out var refreshToken) || refreshToken == null)
                return Unauthorized();

            var tokens = GetRefreshedAccessToken(request, refreshToken);
            Response.Cookies.Append(Cookie.RefreshToken, refreshToken, new CookieOptions() { HttpOnly = true });
            return Ok(tokens);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationRequest registrationRequest)
        {
            var hashCodePassword = registrationRequest.Password.PasswordToStringHashCode();
            if (_context.Accounts.Any(x => x.Email == registrationRequest.Email))
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
            _context.Accounts.Add(newAccount);

            _context.SaveChanges();
            return Login(new LoginRequest()
            {
                Email = registrationRequest.Email,
                Password = registrationRequest.Password,
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest logoutRequest)
        {
            var refreshToken =
                _context.RefreshTokens.FirstOrDefault(x => x.AccountId.ToString() == logoutRequest.AccountId);
            if (Request.Cookies.TryGetValue(Cookie.RefreshToken, out _))
                Response.Cookies.Delete(Cookie.RefreshToken);

            if (refreshToken is null)
                return NotFound();

            _context.RefreshTokens.Remove(refreshToken);
            _context.SaveChanges();
            return Ok();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId is null || code is null)
                return BadRequest();

            var account = await _context.Accounts.FindAsync(userId);
            if (account == null)
                return NotFound();

            account.IsConfirmed = true;
            account.ConfirmationLink = null;
            await _context.SaveChangesAsync();
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

            var expiryDateUnix =
                long.Parse(validatedJwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTime = DateTime.UnixEpoch.AddSeconds(expiryDateUnix);

            if (expiryDateTime > DateTime.UtcNow)
            {
                throw new NotImplementedException("this token has not expired yet");
            }

            var storedRefreshToken = _context.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                throw new NotImplementedException("this refreshToken does not exist");
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpireDate)
            {
                throw new NotImplementedException("the refreshToken has expired");
            }

            //todo: also add Invalidated and Used conditions
            var jti = validatedJwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedRefreshToken.AccessTokenId != jti)
            {
                throw new NotImplementedException("the refreshToken does not match this JWT");
            }

            var account = _context.Accounts.Find(storedRefreshToken.AccountId);
            var newAccessToken = Generate.Jwt(account, _authOptions.Value);
            var newAccessTokenString = new JwtSecurityTokenHandler().WriteToken(newAccessToken);

            storedRefreshToken.AccessTokenId =
                newAccessToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            _context.SaveChanges();

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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return null;
            var passwordHashCode = password.PasswordToStringHashCode();
            return _context.Accounts.FirstOrDefault(x => x.Email == email && x.Password == passwordHashCode);
        }

        private string CreateAndSaveRefreshToken(JwtSecurityToken jwt, Guid accountId)
        {
            var authParams = _authOptions.Value;
            var generatedKeyForRefreshToken = Generate.TokenId();
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtId = jwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, generatedKeyForRefreshToken),
                new Claim(JwtRegisteredClaimNames.Email, accountId.ToString()),
            };

            var expirationDate = DateTime.Now.AddDays(_authOptions.Value.RefreshTokenLifeTimeInDays);
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
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();


            return refreshJwtString;
        }

        private JwtSecurityToken GenerateEmailConfirmationJwtToken()
        {
            var id = Generate.TokenId();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, id),
            };

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMonths(1));

            return jwt;
        }
    }
}