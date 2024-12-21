namespace Dota.API.Hero.RabbitMq.DLX;

public interface IHeroDlxService
{
    void Configure(string exchange, string routingKey, string dlxQueue);
}