using System.Collections.Generic;
using DataAccess.Models;
using Newtonsoft.Json;

namespace DotA.API.Models.FilterModels
{
    public class MatchFilterModel
    {
        [JsonProperty("minDuration")]
        public int? MinDuration { get; set; }
        
        [JsonProperty("maxDuration")]
        public int? MaxDuration { get; set; }
        
        [JsonProperty("minStart")]
        public int? MinStart { get; set; }
        
        [JsonProperty("maxStart")]
        public int? MaxStart { get; set; }
        
        [JsonProperty("minEnd")]
        public int? MinEnd { get; set; }
        
        [JsonProperty("maxEnd")]
        public int? MaxEnd { get; set; }

        [JsonProperty("selfTeam")]
        public List<Hero> SelfTeam { get; set; } = new List<Hero>();
        
        [JsonProperty("otherTeam")]
        public List<Hero> OtherTeam { get; set; } = new List<Hero>();
        
        [JsonProperty("skip")]
        public int? Skip { get; set; }
        
        [JsonProperty("take")]
        public int? Take { get; set; }
    }
}