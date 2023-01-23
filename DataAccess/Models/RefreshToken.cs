using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("refreshToken")]
    public class RefreshToken
    {
        [Key] public string Token { get; set; }

        public string JwtId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool Used { get; set; }
        
        public bool Invalidated { get; set; }
        
        public string AccountEmail { get; set; }
        
        [ForeignKey(nameof(AccountEmail))]
        public virtual Account Account { get; set; }
    }
}