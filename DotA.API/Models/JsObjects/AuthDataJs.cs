using DataAccess.Enums;
using Newtonsoft.Json;

namespace DotA.API.Models.JsObjects
{
    public class AuthDataJs
    {
        [JsonProperty("email")]
        public string Email { get; set; }   
        
        [JsonProperty("password")]
        public string Password { get; set; }  
        
        [JsonProperty("accessLevel")]
        public AccessLevel AccessLevel { get; set; }       
    }
}