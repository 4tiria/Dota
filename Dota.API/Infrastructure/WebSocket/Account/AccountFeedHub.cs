using Microsoft.AspNetCore.SignalR;

namespace Dota.API.WebSocket.Account;

public class AccountFeedHub : Hub<IAccountFeedClient>
{
    public async Task UpdateFeed(UpdateAccountFeedDto message)
    {
        await Clients.All.UpdateFeed(message);
    }
}