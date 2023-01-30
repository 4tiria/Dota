using Newtonsoft.Json;

namespace Dota.Auth.Models.DTO.Requests
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}