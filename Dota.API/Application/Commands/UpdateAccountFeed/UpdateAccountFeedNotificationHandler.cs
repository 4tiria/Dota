using AutoMapper;
using Dota.API.Infrastructure.WebSocket.Account;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Dota.API.Commands.UpdateAccountFeed;

public class UpdateAccountFeedNotificationHandler(IHubContext<AccountFeedHub, IAccountFeedClient> hubContext, IMapper mapper) : INotificationHandler<UpdateAccountFeedNotification>
{
    public async Task Handle(UpdateAccountFeedNotification notification, CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.UpdateFeed(mapper.Map<UpdateAccountFeedDto>(notification));
    }
}