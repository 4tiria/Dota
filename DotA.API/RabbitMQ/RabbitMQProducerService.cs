using RabbitMQ.Client;
using System.Text;

namespace DotA.API.RabbitMQ;

public class RabbitMQProducerService : IRabbitMQProducerService
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    public RabbitMQProducerService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "my_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Produce()
    {
        var message = "Hello RabbitMQ!";
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "", routingKey: "my_queue", basicProperties: null, body: body);
    }
}
