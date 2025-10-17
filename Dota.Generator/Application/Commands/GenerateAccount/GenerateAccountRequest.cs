using System.Reflection.Metadata;

namespace Dota.Generator.Application.Commands.GenerateAccount;

public class GenerateAccountRequest
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string NickName { get; set; }
    
    public string? Email { get; set; }
    
    public Blob? Avatar { get; set; }
}