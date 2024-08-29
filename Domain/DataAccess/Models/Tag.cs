using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("tag")]
    public class Tag
    {
        [Key]
        public string Name { get; set; }
        
        public virtual List<Hero> Heroes { get; set; }
    }
}