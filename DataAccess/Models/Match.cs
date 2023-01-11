using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("match")]
    public class Match
    {
        [Key] 
        public Guid Id { get; set; }

        public virtual List<HeroInMatch> Heroes { get; set; }

        public DateTime Start { get; set; }
        
        public DateTime End { get; set; }
        
        public string Score { get; set; }
    }
}