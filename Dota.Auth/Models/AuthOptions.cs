using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Dota.Auth.Models
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        
        public int AccessTokenLifeTimeInMinutes { get; set; }
        
        public int RefreshTokenLifeTimeInDays { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        }
    }
}