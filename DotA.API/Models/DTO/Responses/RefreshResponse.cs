using Newtonsoft.Json;

namespace DotA.API.Models.DTO.Responses
{
    public class RefreshResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}