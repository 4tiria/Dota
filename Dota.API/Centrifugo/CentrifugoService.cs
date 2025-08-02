using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Dota.API.Centrifugo;

public class CentrifugoService(IConfiguration configuration) : ICentrifugoService
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
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"apikey {configuration["Centrifugo:Token:Secret"]}");

        var payload = new
        {
            method = "publish",
            @params = new
            {
                channel = channel,
                data = data
            }
        };

        var response = await client.PostAsJsonAsync(configuration["Centrifugo:Server"], payload);
        response.EnsureSuccessStatusCode();
    }
}