using RabbitMQ.Client;

namespace Dota.API.Statistics.RabbitMq.DLX;

public class StatisticsDlxService : IStatisticsDlxService
{
    private readonly IModel _channel;

    public StatisticsDlxService(IModel channel)
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