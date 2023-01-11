using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models.EntitiesJs
{

    public class MatchJs
    {
        [JsonProperty("id")] 
        public Guid Id { get; set; }

        [JsonProperty("heroes")] 
        public List<HeroInMatchJs> Heroes { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }
        
        [JsonProperty("end")]
        public long End { get; set; }
        
        [JsonProperty("score")]
        public string Score { get; set; }
    }
}