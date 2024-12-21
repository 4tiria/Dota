using RabbitMQ.Client;

namespace Dota.API.Hero.RabbitMq.DLX;

public class HeroDlxService : IHeroDlxService
{
    private readonly IModel _channel;

    public HeroDlxService(IModel channel)
    {
        _channel = channel;
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
    }

    public void Configure(string exchange, string routingKey, string dlxQueue)
    {
        _channel.ExchangeDeclare(exchange, "topic", true, false, null);
        _channel.QueueDeclare(dlxQueue, true, false, false);
        _channel.QueueBind(dlxQueue, exchange, routingKey);
    }
}