using Newtonsoft.Json;

namespace Dota.API.Models.DTO
{
    public class CamelCaseNameJs
    {
        [JsonProperty("name")] 
        public string Name { get; set; }
    }
}