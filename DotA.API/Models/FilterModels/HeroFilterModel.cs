using System.Collections.Generic;
using DotA.API.Models.EntitiesJs;
using Newtonsoft.Json;

namespace DotA.API.Models.FilterModels
{
    public class HeroFilterModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("mainAttribute")]
        public string MainAttribute { get; set; }
        
        [JsonProperty("attackType")]
        public string AttackType { get; set; }
        
        [JsonProperty("tags")]
        public List<TagJs> Tags { get; set; }
    }
}