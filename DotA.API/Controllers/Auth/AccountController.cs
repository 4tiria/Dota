using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using DotA.API.Helpers;
using DotA.API.Models;
using DotA.API.Models.DTO.Requests;
using DotA.API.Models.DTO.Responses;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Cookie = DotA.API.Helpers.Cookie;

namespace DotA.API.Controllers.Auth
{
    [ApiController, Route("api/account")]
    public class AccountController : Controller
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IConfiguration _configuration;

        public AccountController(ApiContext context,
            IMapper mapper,
            IOptions<AuthOptions> authOptions,
            TokenValidationParameters tokenValidationParameters, 
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _authOptions = authOptions;
            _tokenValidationParameters = tokenValidationParameters;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login([FromBody] AuthRequest authRequest)
        {
            if (authRequest is null)
                return BadRequest();

            var account = AuthenticateUser(authRequest.Email, authRequest.Password);
            if (account is null) return Unauthorized();

            var jwt = GenerateJwt(account);
            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(x => x.AccountEmail == authRequest.Email));
            var refreshTokenString = CreateAndSaveRefreshToken(jwt, authRequest.Email);

            var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);
            Response.Cookies.Append(Cookie.RefreshToken, refreshTokenString, new CookieOptions() { HttpOnly = true });
            return Ok(new AuthResponse()
            {
                AccessToken = jwtString,
                UserEmail = account.Email
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
        public async Task<IActionResult> RegisterAsync([FromBody] AuthRequest authRequest)
        {
            var hashCodePassword = authRequest.Password.PasswordToStringHashCode();
            if (_context.Accounts.Any(x => x.Email == authRequest.Email))
            {
                return BadRequest("user with this email already registered");
            }

            var confirmationJwtToken = GenerateEmailConfirmationJwtToken();
            var confirmationJwtTokenString = new JwtSecurityTokenHandler().WriteToken(confirmationJwtToken);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { email = authRequest.Email, code = confirmationJwtTokenString },
                protocol: HttpContext.Request.Scheme);
            var newAccount = new Account()
            {
                Email = authRequest.Email,
                Password = hashCodePassword,
                AccessLevel = authRequest.AccessLevel,
                ConfirmationLink = callbackUrl
            };

            SendEmail(authRequest.Email,
                $"Для подтверждения почты перейдите по ссылке: <a href='{callbackUrl}'>link</a>");

            newAccount.ConfirmationLink = callbackUrl;
            _context.Accounts.Add(newAccount);

            await _context.SaveChangesAsync();
            return Login(authRequest);
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest logoutRequest)
        {
            var refreshToken = _context.RefreshTokens.Find(logoutRequest.Email);
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
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            if (email is null || code is null)
                return BadRequest();

            var account = await _context.Accounts.FindAsync(email);
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

            var account = _context.Accounts.Find(storedRefreshToken.AccountEmail);
            var newAccessToken = GenerateJwt(account);
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

        private string CreateAndSaveRefreshToken(JwtSecurityToken jwt, string accountEmail)
        {
            var authParams = _authOptions.Value;
            var generatedKeyForRefreshToken = GenerateTokenId();
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtId = jwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, generatedKeyForRefreshToken),
                new Claim(JwtRegisteredClaimNames.Email, accountEmail),
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
                AccountEmail = accountEmail,
                CreationDate = DateTime.Now,
                ExpireDate = expirationDate,
            };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();


            return refreshJwtString;
        }

        private JwtSecurityToken GenerateJwt(Account account)
        {
            var authParams = _authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var id = GenerateTokenId();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, id),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim("role", Enum.GetName(typeof(AccessLevel), account.AccessLevel) ?? string.Empty)
            };

            var jwt = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(authParams.AccessTokenLifeTimeInMinutes),
                signingCredentials: credentials);

            return jwt;
        }

        private JwtSecurityToken GenerateEmailConfirmationJwtToken()
        {
            var id = GenerateTokenId();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, id),
            };

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMonths(1));

            return jwt;
        }

        private static string GenerateTokenId()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public void SendEmail(string email, string message)
        {
            var mail = new MailMessage()
            {
                From = new MailAddress("phowar@yandex.ru"),
                To = { new MailAddress(email) },
                Subject = "Email confirmation",
                Body = message,
                IsBodyHtml = true,
            };

            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Credentials = new NetworkCredential("phowar@yandex.ru", "Ph0warthef1rst"),
                Port = 587,
                EnableSsl = true,
            };

            smtpClient.Send(mail);
        }
    }
}