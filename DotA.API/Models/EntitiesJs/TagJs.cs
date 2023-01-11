using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models.EntitiesJs
{
  public class TagJs
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonIgnore]
    public List<HeroJs> Heroes { get; set; }
  }
}
