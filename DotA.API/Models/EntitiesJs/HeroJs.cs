using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models.EntitiesJs
{
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
    
    [JsonProperty("tags")]
    public List<TagJs> Tags { get; set; }  = new List<TagJs>();
    
    [JsonProperty("image")]
    public HeroImageJs ImageJs {get;set;}
  }
}
