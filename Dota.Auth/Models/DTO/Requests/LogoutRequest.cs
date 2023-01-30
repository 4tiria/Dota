using Newtonsoft.Json;

namespace Dota.Auth.Models.DTO.Requests
{
    public class LogoutRequest
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }
    }
}