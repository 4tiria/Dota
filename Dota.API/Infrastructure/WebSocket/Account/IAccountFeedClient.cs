namespace Dota.API.Infrastructure.WebSocket.Account;

public interface IAccountFeedClient
{
    public Task UpdateFeed(UpdateAccountFeedDto message);
}