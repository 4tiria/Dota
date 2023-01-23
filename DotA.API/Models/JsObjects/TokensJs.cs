using Newtonsoft.Json;

namespace DotA.API.Models.JsObjects
{
    public class TokensJs
    {
        [JsonProperty("accessToken")]
        public string Jwt { get; set; }
        
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}