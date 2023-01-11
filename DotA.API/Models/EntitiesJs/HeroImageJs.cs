using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models.EntitiesJs
{
    public class HeroImageJs
    {
        [JsonProperty("heroId")]
        public int HeroId { get; set; }
        
        public HeroJs HeroJs { get; set; }
        
        [JsonProperty("bytes")]
        public byte[] Bytes { get; set; }
    }
}