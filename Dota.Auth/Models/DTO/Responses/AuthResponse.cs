using Newtonsoft.Json;

namespace Dota.Auth.Models.DTO.Responses
{
    public class AuthResponse
    {
        [JsonProperty("account")]
        public AccountResponse AccountResponse { get; set; }
        
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}