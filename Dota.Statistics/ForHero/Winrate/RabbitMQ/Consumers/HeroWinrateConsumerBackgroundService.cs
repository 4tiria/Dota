namespace Dota.Statistics.ForHero.Winrate.RabbitMQ.Consumers;

public class HeroWinrateConsumerBackgroundService(
    ILogger<HeroWinrateConsumerBackgroundService> logger, 
    IEnumerable<IHeroStatisticConsumerService> heroStatisticConsumerServices) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var consumer in heroStatisticConsumerServices)
        {
            consumer.Consume();
            logger.LogInformation("Listening to RabbitMQ messages for {service}", consumer.Description);
        }
        
        return Task.CompletedTask;
    }
}