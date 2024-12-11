using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dota.Statistics.ForHero.Winrate.RabbitMQ.Producers;

public class HeroStatisticProducerService : IHeroStatisticProducerService
{
    private const string QueueName = "response-hero-statistics";

    private readonly ILogger _logger;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    public HeroStatisticProducerService(ILogger<HeroStatisticProducerService> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _consumer = new EventingBasicConsumer(_channel);
    }

    public void ProduceGetStatisticsResponse(Domain.Mongo.Statistics.Models.Entities.HeroStatistic heroStatistic)
    {
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(heroStatistic));
            
        _channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);
    }
}
