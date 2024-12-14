namespace Dota.API.Hero.RabbitMq.Consumers;

public class ConsumerBackgroundService(ILogger<ConsumerBackgroundService> logger, IHeroConsumerService heroConsumerService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        heroConsumerService.Consume();
        logger.LogInformation("Listening for RabbitMQ messages on Dota.API");
        return Task.CompletedTask;
    }
}