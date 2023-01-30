using Dota.Auth.Models.Enums;
using Newtonsoft.Json;

namespace Dota.Auth.Models.DTO.Requests
{
    public class RegistrationRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }   
        
        [JsonProperty("password")]
        public string Password { get; set; }  
        
        [JsonProperty("userName")]
        public string UserName { get; set; }   
        
        [JsonProperty("accessLevel")]
        public AccessLevel AccessLevel { get; set; }       
    }
}