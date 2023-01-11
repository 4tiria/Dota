using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DataAccess.Models
{
    public class Hero
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string AttackType { get; set; }
        
        public string MainAttribute { get; set; }
        
        public virtual List<Tag> Tags { get; set; }  = new List<Tag>();
        
        public virtual HeroImage Image {get;set;}
    }
}