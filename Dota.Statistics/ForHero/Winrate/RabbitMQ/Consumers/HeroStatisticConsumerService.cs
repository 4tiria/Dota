using Domain.Mongo.Statistics;
using Dota.Statistics.ForHero.Winrate.WinrateCalculator;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dota.Statistics.ForHero.Winrate.RabbitMQ.Consumers;

public class HeroStatisticConsumerService : IHeroStatisticConsumerService
{
    private const string ExchangeName = "update-statistics-topic";
    private const string QueueName = "statistics.hero";
    private const string RoutingKey = "statistics.#";
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly EventingBasicConsumer _consumer;
    private readonly MongoDbContext _context;
    private readonly IHeroWinrateCalculatorService _heroWinrateCalculatorService;

    private readonly ILogger _logger;

    public HeroStatisticConsumerService(
        ILogger<HeroStatisticConsumerService> logger,
        MongoDbContext context,
        IHeroWinrateCalculatorService heroWinrateCalculatorService)
    {
        _logger = logger;
        _context = context;
        _heroWinrateCalculatorService = heroWinrateCalculatorService;
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Создаём exchange типа topic
        _channel.ExchangeDeclare(ExchangeName, "topic", true, false, null);

        // Создаём очередь
        _channel.QueueDeclare(QueueName, true, false, false, null);

        // Привязываем очередь к топику
        _channel.QueueBind(QueueName, ExchangeName, RoutingKey);

        _consumer = new EventingBasicConsumer(_channel);
    }

    public string Description => "Service updating statistics of all heroes";

    public void Consume()
    {
        _consumer.Received += (model, ea) =>
        {
            _logger.LogInformation("Updating hero statistics");

            // TODO: вызывать более общий сервис для обновления статистики героя, а не только винрейта
            _heroWinrateCalculatorService.Calculate();
        };
        _channel.BasicConsume(QueueName, true, _consumer);
    }
}