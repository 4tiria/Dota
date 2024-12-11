using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dota.API.Hero.RabbitMQ.Consumers;

public class HeroConsumerService : IHeroConsumerService
{
    private const string QueueName = "response-hero-statistics";

    private readonly ILogger _logger;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    public HeroConsumerService(ILogger<HeroConsumerService> logger)
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
        Consume();
    }

    public void Consume()
    {
        _consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Consumed message: {message}", message);
        };
        _channel.BasicConsume(QueueName, autoAck: true, consumer: _consumer);
    }
}
