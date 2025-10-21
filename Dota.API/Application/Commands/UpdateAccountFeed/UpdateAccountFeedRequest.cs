using System.Reflection.Metadata;
using MediatR;

namespace Dota.API.Commands.UpdateAccountFeed;

public class UpdateAccountFeedRequest : IRequest
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string NickName { get; set; }
}