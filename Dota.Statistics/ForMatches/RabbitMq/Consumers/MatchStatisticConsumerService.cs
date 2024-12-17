using Domain.Mongo.Statistics;
using Dota.Statistics.ForHero.Winrate.RabbitMQ.Consumers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dota.Statistics.ForMatches.RabbitMq.Consumers;

public class MatchStatisticConsumerService : IMatchStatisticConsumerService
{
    private const string ExchangeName = "update-statistics-topic";
    private const string QueueName = "statistics.match";
    private const string RoutingKey = "statistics.#";
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    private readonly MongoDbContext _context;

    private readonly ILogger _logger;

    public MatchStatisticConsumerService(
        ILogger<HeroWinrateStatisticConsumerService> logger,
        MongoDbContext context, IModel channel)
    {
        _logger = logger;
        _context = context;
        _channel = channel;

        _channel.ExchangeDeclare(ExchangeName, "topic", true, false, null);
        _channel.QueueDeclare(QueueName, true, false, false, null);
        _channel.QueueBind(QueueName, ExchangeName, RoutingKey);

        _consumer = new EventingBasicConsumer(_channel);
    }

    public void Consume()
    {
        _consumer.Received += (model, ea) => { _logger.LogInformation("Updating match statistics"); };
        _channel.BasicConsume(QueueName, true, _consumer);
    }
}