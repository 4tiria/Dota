using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        
        public AccountController(ApiContext context, IMapper mapper, IOptions<AuthOptions> authOptions)
        {
            _context = context;
            _mapper = mapper;
            _authOptions = authOptions;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthDataJs authDataJs)
        {
            if (authDataJs is null)
                return BadRequest();
            
            var account = AuthenticateUser(authDataJs.Email, authDataJs.Password);
            if (account is null) return Unauthorized();

            var token = GenerateJwt(account);

            return Ok(new { access_token = token });
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
            return Ok(newAccount);
        }
        
        private Account AuthenticateUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return null;
            var passwordHashCode = password.PasswordToStringHashCode();
            return _context.Accounts.FirstOrDefault(x => x.Email == email && x.Password == passwordHashCode);
        }
        
        private string GenerateJwt(Account account)
        {
            var authParams = _authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim("access", Enum.GetName(typeof(AccessLevel),  account.AccessLevel) ?? string.Empty)
            };
            
            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}