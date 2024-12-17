using System.Text;
using Domain.Mongo.Statistics.Models.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Dota.Statistics.ForHero.Winrate.RabbitMQ.Producers;

public class HeroStatisticProducerService : IHeroStatisticProducerService
{
    private const string QueueName = "response-hero-statistics";
    private readonly IModel _channel;

    private readonly ILogger _logger;

    public HeroStatisticProducerService(ILogger<HeroStatisticProducerService> logger, IModel channel)
    {
        _logger = logger;
        _channel = channel;

        _channel.QueueDeclare(QueueName, true, false, false, null);
    }

    public void ProduceGetStatisticsResponse(HeroStatistic heroStatistic)
    {
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(heroStatistic));

        _logger.LogInformation("Sending message to {queue}", QueueName);
        _channel.BasicPublish("", QueueName, null, body);
    }
}