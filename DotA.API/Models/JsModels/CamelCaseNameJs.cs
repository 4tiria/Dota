using Newtonsoft.Json;

namespace DotA.API.Models.JsModels
{
    public class CamelCaseNameJs
    {
        [JsonProperty("name")] 
        public string Name { get; set; }
    }
}