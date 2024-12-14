using System.Text;
using RabbitMQ.Client;

namespace Dota.API.Statistics.RabbitMq.Producers;

public class StatisticsProducerService : IStatisticsProducerService
{
    //TODO: доставать _channel из DI
    private const string ExchangeName = "update-statistics-topic";

    private const string RoutingKey = "statistics.update";
    private readonly IModel _channel;

    public StatisticsProducerService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        
        _channel.ExchangeDeclare(exchange: ExchangeName, type: "topic", durable: true, autoDelete: false, arguments: null);
    }
    
    public void Produce()
    {
        _channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKey, basicProperties: null);
    }
}