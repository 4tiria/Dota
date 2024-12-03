using System.Threading.Tasks;
using System;
using Domain.Mongo.Auth.Models.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Dota.Auth.Email;

public interface IEmailService
{
    Task SendEmail(string email, string message);

    Task<Account> ConfirmEmail(Guid userId, string code);

    JwtSecurityToken GenerateEmailConfirmationJwtToken();
}