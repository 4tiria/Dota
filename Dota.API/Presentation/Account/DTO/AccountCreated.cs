using System.Reflection.Metadata;
using MediatR;

namespace Dota.API.Account.DTO;

public class AccountCreated : IRequest
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string NickName { get; set; }
    
    public string? Email { get; set; }
    
    public Blob? Avatar { get; set; }
}