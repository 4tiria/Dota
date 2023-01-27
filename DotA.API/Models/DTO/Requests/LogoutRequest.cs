using Newtonsoft.Json;

namespace DotA.API.Models.DTO.Requests
{
    public class LogoutRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}