using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models
{
  [Table("tag")]
  public class Tag
  {
    [Key]
    [JsonProperty("Name")]
    public string Name { get; set; }

    public virtual List<Hero> Heroes { get; set; }
  }
}
