using System.Text;
using Domain.Mongo.Statistics;
using Dota.Statistics.ForHero.Winrate.RabbitMQ.Consumers;
using Dota.Statistics.ForHero.Winrate.RabbitMQ.Producers;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dota.Statistics.ForMatches.RabbitMq.Consumers;

public class MatchStatisticConsumerService : IMatchStatisticConsumerService
{
    private const string ExchangeName = "update-statistics-topic";
    private const string QueueName = "statistics.match";
    private const string RoutingKey = "statistics.#";
    
    private readonly ILogger _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    private readonly IHeroStatisticProducerService _heroStatisticProducer;

    private readonly MongoDbContext _context;

    public MatchStatisticConsumerService(
        ILogger<HeroWinrateStatisticConsumerService> logger, 
        MongoDbContext context)
    {
        _logger = logger;
        _context = context;
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Создаём exchange типа topic
        _channel.ExchangeDeclare(exchange: ExchangeName, type: "topic", durable: true, autoDelete: false, arguments: null);

        // Создаём очередь
        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        // Привязываем очередь к топику
        _channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: RoutingKey);
        
        _consumer = new EventingBasicConsumer(_channel);
    }

    public void Consume()
    {
        _consumer.Received += (model, ea) =>
        {
            _logger.LogInformation("Updating match statistics");
        };
        _channel.BasicConsume(QueueName, autoAck: true, consumer: _consumer);
    }
}
