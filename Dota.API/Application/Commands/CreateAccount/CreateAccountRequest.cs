using System.Reflection.Metadata;
using MediatR;

namespace Dota.API.Commands.CreateAccount;

public class CreateAccountRequest : IRequest
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string NickName { get; set; }
    
    public string? Email { get; set; }
    
    public Blob? Avatar { get; set; }
}