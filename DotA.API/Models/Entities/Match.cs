using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models.Entities
{
    [Table("match")]
    public class Match
    {
        [Key]
        [JsonProperty("id")] 
        public Guid Id { get; set; }

        [JsonProperty("heroes")] 
        public virtual List<HeroInMatch> Heroes { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }
        
        [JsonProperty("end")]
        public DateTime End { get; set; }
        
        [JsonProperty("score")]
        public string Score { get; set; }
    }
}