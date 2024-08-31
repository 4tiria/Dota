using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.NoSql.Auth.Models.Enums;

namespace Domain.NoSql.Auth.Models.Entities;

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
    public byte[]? Avatar { get; set; }
}