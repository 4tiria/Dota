using System.Collections.Generic;
using DataAccess.Models;
using Newtonsoft.Json;

namespace DotA.API.Models.FilterModels
{
    public class MatchFilterModel
    {
        [JsonProperty("minDurationInMinutes")]
        public int? MinDurationInMinutes { get; set; }
        
        [JsonProperty("maxDurationInMinutes")]
        public int? MaxDurationInMinutes { get; set; }
        
        [JsonProperty("minStartedMillisecondsBefore")]
        public long? MinStartedMillisecondsBefore { get; set; }
        
        [JsonProperty("maxStartedMillisecondsBefore")]
        public long? MaxStartedMillisecondsBefore { get; set; }

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