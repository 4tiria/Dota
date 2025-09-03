using Microsoft.AspNetCore.SignalR;

namespace Dota.API.WebSocket;

public class NotificationHub : Hub<INotificationClient>
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendMessage(message);
    }
}