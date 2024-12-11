using System.Text;
using Dota.API.RabbitMQ;
using MongoDB.Bson;
using RabbitMQ.Client;

namespace Dota.API.Hero.RabbitMQ.Producers;

public class HeroProducerService : IHeroProducerService
{
    private const string QueueName = "request-hero-statistics";
    private readonly IModel _channel;

    public HeroProducerService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Produce(ObjectId heroId)
    {
        var body = Encoding.UTF8.GetBytes(heroId.ToString());
            
        _channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);
    }
}
