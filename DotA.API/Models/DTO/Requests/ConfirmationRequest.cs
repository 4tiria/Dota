using Newtonsoft.Json;

namespace DotA.API.Models.DTO.Requests
{
    public class ConfirmationRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}