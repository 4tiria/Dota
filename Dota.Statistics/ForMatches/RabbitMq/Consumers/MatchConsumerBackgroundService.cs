using Dota.Statistics.ForHero.Winrate.RabbitMQ.Consumers;

namespace Dota.Statistics.ForMatches.RabbitMq.Consumers;

public class MatchConsumerBackgroundService(
    ILogger<MatchConsumerBackgroundService> logger, 
    IMatchStatisticConsumerService matchStatisticConsumerService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        matchStatisticConsumerService.Consume();
        logger.LogInformation("Listening for match statistics RabbitMQ messages on Dota.Statistics");
        return Task.CompletedTask;
    }
}