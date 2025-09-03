namespace Dota.API.WebSocket;

public interface INotificationClient
{
    public Task SendMessage(string message);
}