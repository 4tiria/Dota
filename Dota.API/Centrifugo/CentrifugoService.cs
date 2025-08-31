using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Centrifugo.AspNetCore.Abstractions;
using Centrifugo.AspNetCore.Models.Request;
using Microsoft.IdentityModel.Tokens;

namespace Dota.API.Centrifugo;

public class CentrifugoService(ICentrifugoClient centrifugoClient, IConfiguration configuration) : ICentrifugoService
{
    public string GenerateCentrifugoToken(string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{configuration["Centrifugo:Token:Secret"]}"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: [new Claim("sub", userId)],
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public async Task PublishToCentrifugoAsync(string channel, object data)
    {
        await centrifugoClient.Publish(new PublishParams
        {
            Channel = "channel1",
            Data = new { text = "hello" }
        });
    }
}