using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataAccess.Models
{
    [Table("heroInMatch")]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class HeroInMatch
    {
        [Key, ForeignKey("Hero")] 
        public int HeroId { get; set; }

        public virtual Hero Hero { get; set; }

        [Key, ForeignKey("Match")] 
        public Guid MatchId { get; set; }

        public virtual Match Match { get; set; }

        public string Side { get; set; }
        
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }

        public int Gold { get; set; }

        // ReSharper disable once InconsistentNaming
        public int XP { get; set; }

        //todo: Items, Talents, Time...
    }
}