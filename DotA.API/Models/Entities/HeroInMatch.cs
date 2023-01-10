using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models.Entities
{
    [Table("heroInMatch")]
    public class HeroInMatch
    {
        [Key, ForeignKey("Hero")]
        [JsonProperty("heroId")]
        public int HeroId { get; set; }
        
        [JsonProperty("hero")]
        public virtual Hero Hero { get; set; }
        
        [Key, ForeignKey("Match")]
        [JsonProperty("matchId")]
        public Guid MatchId { get; set; }
        
        [JsonProperty("match")]
        public virtual Match Match { get; set; }
        
        [JsonProperty("side")]
        public string Side { get; set; }
        
        [JsonProperty("kda")]
        // ReSharper disable once InconsistentNaming
        public string KDA { get; set; }
        
        [JsonProperty("gold")]
        public int Gold { get; set; }
        
        [JsonProperty("xp")]
        // ReSharper disable once InconsistentNaming
        public int XP { get; set; }
        
        //todo: Items, Talents, Time...
    }
}