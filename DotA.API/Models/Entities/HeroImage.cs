using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DotA.API.Models.Entities
{
    [Table("heroImage")]
    public class HeroImage
    {
        [ForeignKey("Hero"), Key]
        [JsonProperty("heroId")]
        public int HeroId { get; set; }
        
        public virtual Hero Hero { get; set; }
        
        [JsonProperty("bytes")]
        public byte[] Bytes { get; set; }
    }
}