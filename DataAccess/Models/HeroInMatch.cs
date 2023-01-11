using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DataAccess.Models
{
    [Table("heroInMatch")]
    public class HeroInMatch
    {
        [Key, ForeignKey("Hero")]
        public int HeroId { get; set; }
        
        public virtual Hero Hero { get; set; }
        
        [Key, ForeignKey("Match")]
        public Guid MatchId { get; set; }
        
        public virtual Match Match { get; set; }
        
        public string Side { get; set; }
        
        // ReSharper disable once InconsistentNaming
        public string KDA { get; set; }
        
        public int Gold { get; set; }
        
        // ReSharper disable once InconsistentNaming
        public int XP { get; set; }
        
        //todo: Items, Talents, Time...
    }
}