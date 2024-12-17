using RabbitMQ.Client;

namespace Dota.API.Statistics.RabbitMq.Producers;

public class StatisticsProducerService : IStatisticsProducerService
{
    private const string ExchangeName = "update-statistics-topic";
    private const string RoutingKey = "statistics.update";
    private readonly IModel _channel;

    public StatisticsProducerService(IModel channel)
    {
        _channel = channel;
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _channel.ExchangeDeclare(ExchangeName, "topic", true, false, null);
    }

    public void Produce()
    {
        _channel.BasicPublish(ExchangeName, RoutingKey, basicProperties: null);
    }
}