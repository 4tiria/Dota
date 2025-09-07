using Newtonsoft.Json;

namespace Dota.API.Models.DTO;

//TODO: что это??? Надо убрать
public class CamelCaseNameJs
{
    [JsonProperty("name")] 
    public string Name { get; set; }
}
