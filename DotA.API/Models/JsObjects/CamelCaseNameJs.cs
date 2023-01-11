using Newtonsoft.Json;

namespace DotA.API.Models.JsObjects
{
    public class CamelCaseNameJs
    {
        [JsonProperty("name")] 
        public string Name { get; set; }
    }
}