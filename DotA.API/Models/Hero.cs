using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models
{
  [Table("hero")]
  public class Hero
  {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("attackType")]
    public string AttackType { get; set; }

    [JsonProperty("mainAttribute")]
    public string MainAttribute { get; set; }
    
    public virtual List<Tag> Tags { get; set; }  = new List<Tag>();
  }
}
