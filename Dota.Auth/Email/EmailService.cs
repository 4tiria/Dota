using Dota.Auth.Helpers;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using MongoDB.Driver;
using Domain.Mongo.Auth.Models.Entities;
using Domain.Mongo.Auth;

namespace Dota.Auth.Email;

public class EmailService(MongoDbContext context) : IEmailService
{
    private readonly MongoDbContext _context = context;
    private const string MailAddress = "phowar@yandex.ru";
    private readonly SmtpClient _smtpClient = new("smtp.yandex.ru")
    {
        Credentials = new NetworkCredential(MailAddress, "Ph0warthef1rst"),
        Port = 587,
        EnableSsl = true,
    };

    private readonly MailAddress _mailAddressFrom = new(MailAddress);

    public async Task<Account> ConfirmEmail(Guid userId, string code)
    {
        return await _context.Accounts.FindOneAndUpdateAsync(
             account => account.Id == userId,
             Builders<Account>.Update
                 .Set(account => account.IsConfirmed, true)
                 .Set(account => account.ConfirmationLink, null)
        );    
    }

    public async Task SendEmail(string email, string message)
    {
        var mail = new MailMessage()
        {
            From = _mailAddressFrom,
            To = { new MailAddress(email) },
            Subject = "Email confirmation",
            Body = message,
            IsBodyHtml = true,
        };

        await _smtpClient.SendMailAsync(mail);
    }

    public JwtSecurityToken GenerateEmailConfirmationJwtToken()
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

