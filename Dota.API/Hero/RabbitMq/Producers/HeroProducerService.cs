using System.Text;
using Dota.API.RabbitMQ;
using MongoDB.Bson;
using RabbitMQ.Client;

namespace Dota.API.Hero.RabbitMq.Producers;

public class HeroProducerService : IHeroProducerService
{
    private const string QueueName = "request-hero-statistics";
    private readonly IModel _channel;

    public HeroProducerService(IModel channel)
    {
        _channel = channel;
        _channel.QueueDeclare(QueueName, true, false, false, null);
    }

    public void Produce(ObjectId heroId)
    {
        var body = Encoding.UTF8.GetBytes(heroId.ToString());

        _channel.BasicPublish("", QueueName, null, body);
    }
}