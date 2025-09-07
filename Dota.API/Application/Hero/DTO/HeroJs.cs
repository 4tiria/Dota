using Newtonsoft.Json;

namespace Dota.API.Models.EntitiesJs;

public class HeroJs
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("attackType")]
    public string AttackType { get; set; }

    [JsonProperty("mainAttribute")]
    public string MainAttribute { get; set; }

    [JsonProperty("roles")]
    public List<string> Roles { get; set; } = [];

    public string ImageLink { get; set; }
}