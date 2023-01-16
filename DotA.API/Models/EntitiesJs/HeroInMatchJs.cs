using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models.EntitiesJs
{
    public class HeroInMatchJs
    {
        [JsonProperty("heroId")]
        public int HeroId { get; set; }
        
        [JsonProperty("hero")]
        public HeroJs Hero { get; set; }
     
        [JsonProperty("matchId")]
        public Guid MatchId { get; set; }
        
        [JsonProperty("match")]
        public MatchJs Match { get; set; }
        
        [JsonProperty("side")]
        public string Side { get; set; }
        
        [JsonProperty("kills")]
        public int Kills { get; set; }
        
        [JsonProperty("deaths")]
        public int Deaths { get; set; }
        
        [JsonProperty("assists")]
        public int Assists { get; set; }
        
        [JsonProperty("gold")]
        public int Gold { get; set; }
        
        [JsonProperty("xp")]
        // ReSharper disable once InconsistentNaming
        public int XP { get; set; }
        
        //todo: Items, Talents, Time...
    }
}