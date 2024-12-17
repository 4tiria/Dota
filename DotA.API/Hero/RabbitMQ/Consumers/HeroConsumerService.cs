using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dota.API.Hero.RabbitMq.Consumers;

public class HeroConsumerService : IHeroConsumerService
{
    private const string QueueName = "response-hero-statistics";
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    private readonly ILogger _logger;

    public HeroConsumerService(ILogger<HeroConsumerService> logger, IModel channel)
    {
        _logger = logger;
        _channel = channel;

        _channel.QueueDeclare(QueueName, true, false, false, null);
        _consumer = new EventingBasicConsumer(_channel);
    }

    public void Consume()
    {
        _consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Consumed message: {message}", message);
        };
        _channel.BasicConsume(QueueName, true, _consumer);
    }
}