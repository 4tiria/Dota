using System.Text;
using Domain.Mongo.Statistics;
using Dota.Statistics.ForHero.Winrate.RabbitMQ.Producers;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dota.Statistics.ForHero.Winrate.RabbitMQ.Consumers;

public class HeroWinrateStatisticConsumerService : IHeroStatisticConsumerService
{
    private const string QueueName = "request-hero-statistics";
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    private readonly MongoDbContext _context;
    private readonly IHeroStatisticProducerService _heroStatisticProducer;

    private readonly ILogger _logger;

    public HeroWinrateStatisticConsumerService(
        ILogger<HeroWinrateStatisticConsumerService> logger,
        MongoDbContext context,
        IHeroStatisticProducerService heroStatisticProducer, IModel channel)
    {
        _logger = logger;
        _context = context;
        _heroStatisticProducer = heroStatisticProducer;
        _channel = channel;

        _channel.QueueDeclare(QueueName, true, false, false, null);
        _consumer = new EventingBasicConsumer(_channel);
    }

    public string Description => "Service calculating winrate of a single hero";

    public void Consume()
    {
        _consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var heroId = ObjectId.Parse(Encoding.UTF8.GetString(body));

            var heroStatistic = _context.HeroStatistics
                .Find(heroStatistic => heroStatistic.HeroId == heroId)
                .FirstOrDefault() ?? throw new Exception($"Hero Id {heroId} not found");

            _heroStatisticProducer.ProduceGetStatisticsResponse(heroStatistic);
        };
        _channel.BasicConsume(QueueName, true, _consumer);
    }
}