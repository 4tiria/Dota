using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using DotA.API.Helpers;
using DotA.API.Models;
using DotA.API.Models.JsObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/account")]
    public class AccountController : Controller
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AccountController(ApiContext context, IMapper mapper,
            IOptions<AuthOptions> authOptions,
            TokenValidationParameters tokenValidationParameters)
        {
            _context = context;
            _mapper = mapper;
            _authOptions = authOptions;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthDataJs authDataJs)
        {
            if (authDataJs is null)
                return BadRequest();

            var account = AuthenticateUser(authDataJs.Email, authDataJs.Password);
            if (account is null) return Unauthorized();

            var jwt = GenerateJwt(account);
            var refreshTokenString = UploadRefreshToken(jwt, authDataJs.Email);

            var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(new TokensJs() { Jwt = jwtString, RefreshToken = refreshTokenString });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] TokensJs request)
        {
            if (request is null)
                return BadRequest();

            var tokens = RefreshToken(request);
            return Ok(tokens);
        }
        
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthDataJs authDataJs)
        {
            var hashCodePassword = authDataJs.Password.PasswordToStringHashCode();
            if (_context.Accounts.Any(x => x.Email == authDataJs.Email))
            {
                return BadRequest("user with this email already registered");
            }

            var newAccount = new Account()
            {
                Email = authDataJs.Email,
                Password = hashCodePassword,
                AccessLevel = authDataJs.AccessLevel
            };

            _context.Accounts.Add(newAccount);

            _context.SaveChanges();
            return Login(authDataJs);
        }
        private TokensJs RefreshToken(TokensJs tokensJs)
        {
            var validatedJwt = GetPrincipalFromToken(tokensJs.Jwt);
            if (validatedJwt == null)
            {
                //token is invalid or smth idk
                return null;
            }

            //у меня скорее всего по-другому
            var expiryDateUnix =
                long.Parse(validatedJwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTime = DateTime.UnixEpoch.AddSeconds(expiryDateUnix);

            if (expiryDateTime > DateTime.UtcNow)
            {
                throw new NotImplementedException("this token has not expired yet");
            }
            
            var storedRefreshToken = _context.RefreshTokens.SingleOrDefault(x => x.Token == tokensJs.RefreshToken);

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
            if (storedRefreshToken.JwtId != jti)
            {
                throw new NotImplementedException("the refreshToken does not match this JWT");
            }
            
            var account = _context.Accounts.Find(storedRefreshToken.AccountEmail);
            var newJwt = GenerateJwt(account);
            var newJwtString = new JwtSecurityTokenHandler().WriteToken(newJwt);
            
            storedRefreshToken.JwtId = newJwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;;
            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            _context.SaveChanges();
            
            return new TokensJs() { Jwt = newJwtString, RefreshToken = storedRefreshToken.Token };
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

        private string UploadRefreshToken(JwtSecurityToken jwt, string accountEmail)
        {
            var generatedKeyForRefreshToken = GenerateTokenId();
            var jwtId = jwt.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var refreshToken = new RefreshToken()
            {
                Token = generatedKeyForRefreshToken,
                JwtId = jwtId,
                AccountEmail = accountEmail,
                CreationDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddDays(_authOptions.Value.RefreshTokenLifeTimeInDays),
            };

            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            return refreshToken.Token;
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

            var accessToken = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(authParams.AccessTokenLifeTimeInSeconds),
                signingCredentials: credentials);

            return accessToken;
        }

        private static string GenerateTokenId()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}