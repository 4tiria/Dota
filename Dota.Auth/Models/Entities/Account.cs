using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dota.Auth.Models.Enums;

namespace Dota.Auth.Models.Entities
{
    [Table("account")]
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public string ConfirmationLink { get; set; }
        public DateTime? ConfirmationExpiration { get; set; }
        public byte[] Avatar { get; set; }
    }
}