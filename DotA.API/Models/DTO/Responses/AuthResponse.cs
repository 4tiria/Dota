using Newtonsoft.Json;

namespace DotA.API.Models.DTO.Responses
{
    public class AuthResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }
    }
}