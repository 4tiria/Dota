using System.Reflection.Metadata;

namespace Dota.API.WebSocket.Account;

public class UpdateAccountFeedDto
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string NickName { get; set; }
}