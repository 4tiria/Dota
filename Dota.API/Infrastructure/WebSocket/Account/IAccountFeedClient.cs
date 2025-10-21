namespace Dota.API.WebSocket.Account;

public interface IAccountFeedClient
{
    public Task UpdateFeed(UpdateAccountFeedDto message);
}