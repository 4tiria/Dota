using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Domain.NoSql.Auth.Models;
using Domain.NoSql.Auth.Models.Entities;
using Domain.NoSql.Auth.Models.Enums;
using Microsoft.IdentityModel.Tokens;

namespace Dota.Auth.Helpers
{
    public static class Generate
    {
        public static string TokenId()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        
        public static JwtSecurityToken Jwt(Account account, AuthOptions authParams)
        {
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var id = TokenId();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, id),
                new(JwtRegisteredClaimNames.Email, account.Email),
                new("role", Enum.GetName(typeof(AccessLevel), account.AccessLevel) ?? string.Empty)
            };

            var jwt = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(authParams.AccessTokenLifeTimeInMinutes),
                signingCredentials: credentials);

            return jwt;
        }
    }
}