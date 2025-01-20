using Dota.API.Models.EntitiesJs;
using Newtonsoft.Json;

namespace Dota.API.Models.FilterModels;

public class HeroFilterModel
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("mainAttribute")]
    public string MainAttribute { get; set; }

    [JsonProperty("attackType")]
    public string AttackType { get; set; }

    [JsonProperty("roles")]
    public List<string> Roles { get; set; }
}