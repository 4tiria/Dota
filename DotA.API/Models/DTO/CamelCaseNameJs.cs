using Newtonsoft.Json;

namespace DotA.API.Models.DTO
{
    public class CamelCaseNameJs
    {
        [JsonProperty("name")] 
        public string Name { get; set; }
    }
}