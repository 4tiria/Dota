namespace Dota.API.Centrifugo;

public interface ICentrifugoService
{
    string GenerateCentrifugoToken(string userId);
    Task PublishToCentrifugoAsync(string channel, object data);
}