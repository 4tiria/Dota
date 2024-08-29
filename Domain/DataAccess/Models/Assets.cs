using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace DataAccess.Models
{
    [Table("asset")]
    public class Asset
    {
        [Key]
        public string Name { get; set; }
        public byte[] Blob { get; set; }
    }
}