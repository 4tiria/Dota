using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dota.API.Statistics.RabbitMq.DLX;

public class StatisticsDlxService : IStatisticsDlxService
{
    private const string DlxQueue = "dlx.statistics";
    private const string ExchangeName = "statistics-dlx";
    private const string RoutingKey = "statistics-dead";

    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
    private readonly ILogger<StatisticsDlxService> _logger;

    public StatisticsDlxService(IModel channel, ILogger<StatisticsDlxService> logger)
    {
        _channel = channel;
        _logger = logger;
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _consumer = new EventingBasicConsumer(_channel);
    }

    public void Consume()
    {
        _consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogWarning("Received Statistics DLX {message}", message);

            if (ea.BasicProperties.Headers != null && ea.BasicProperties.Headers.ContainsKey("x-death"))
            {
                var xDeath = ea.BasicProperties.Headers["x-death"] as List<object>;
                foreach (var deathInfo in xDeath)
                {
                    var deathDict = deathInfo as IDictionary<string, object>;
                    _logger.LogInformation("DLX Reason: {reason}",
                        Encoding.UTF8.GetString((byte[])deathDict["reason"]));

                    _logger.LogInformation("Original Queue: {queue}",
                        Encoding.UTF8.GetString((byte[])deathDict["queue"]));

                    _logger.LogInformation("Exchange: {exchange}",
                        Encoding.UTF8.GetString((byte[])deathDict["exchange"]));

                    _logger.LogInformation("Time: {time}", deathDict["time"]);
                }
            }
        };

        _channel.BasicConsume(
            DlxQueue,
            false,
            _consumer
        );
    }

    public void Configure()
    {
        _channel.ExchangeDeclare(ExchangeName, "topic", true, false, null);
        _channel.QueueDeclare(DlxQueue, true, false, false);
        _channel.QueueBind(DlxQueue, ExchangeName, RoutingKey);
    }
}