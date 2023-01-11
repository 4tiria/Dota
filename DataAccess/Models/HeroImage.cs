using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class HeroImage
    {
        [ForeignKey("Hero"), Key]
        public int HeroId { get; set; }
        
        public virtual Hero Hero { get; set; }
        
        public byte[] Bytes { get; set; }
    }
}