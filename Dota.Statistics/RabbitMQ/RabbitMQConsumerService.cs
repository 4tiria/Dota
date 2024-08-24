using Dota.Statistics.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Dota.Statistics.RabbitMQConsumerService;

public class RabbitMQConsumerService : IRabbitMQConsumerService
{
    const string QUEUE_NAME = "my_queue";

    private readonly ILogger _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    public RabbitMQConsumerService(ILogger<RabbitMQConsumerService> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "QUEUE_NAME", durable: false, exclusive: false, autoDelete: false, arguments: null);
        _consumer = new EventingBasicConsumer(_channel);
    }

    public void Consume()
    {
        _consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Consumed message: {message}");
        };
        _channel.BasicConsume(QUEUE_NAME, autoAck: true, consumer: _consumer);
    }
}
