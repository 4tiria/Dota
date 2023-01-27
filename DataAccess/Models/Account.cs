using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;

namespace DataAccess.Models
{
    [Table("account")]
    public class Account
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public bool IsConfirmed { get; set; } = false;
        
        public string ConfirmationLink { get; set; }
    }
}