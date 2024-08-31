using System;
using Domain.NoSql.Auth.Models.Enums;

namespace Dota.Auth.Models.DTO.Responses
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public byte[] Avatar { get; set; }
    }
}