using System.Reflection.Metadata;

namespace Dota.API.Commands.CreateAccount;

public class CreateAccountRequest
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string NickName { get; set; }
    
    public string? Email { get; set; }
    
    public Blob? Avatar { get; set; }
}