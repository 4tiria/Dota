using Newtonsoft.Json;

namespace Dota.API.Models.EntitiesJs;

public class HeroJs
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("attackType")]
    public string AttackType { get; set; }

    [JsonProperty("mainAttribute")]
    public string MainAttribute { get; set; }

    [JsonProperty("image")]
    public byte[] Image { get; set; }

    [JsonProperty("tags")]
    public List<TagJs> Tags { get; set; } = [];
    
    [JsonProperty("winrate")]
    public float Winrate { get; set; }
}
