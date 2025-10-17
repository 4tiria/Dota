using Dota.API.Hero.RabbitMq.Consumers;
using Dota.API.Hero.RabbitMq.DLX;
using Dota.API.Statistics.RabbitMq.DLX;

namespace Dota.API.Hero.RabbitMq;

public class HeroBackgroundService(
    ILogger<HeroBackgroundService> logger,
    IHeroConsumerService heroConsumerService,
    IStatisticsDlxService statisticsDlxService,
    IHeroDlxService heroDlxService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            heroConsumerService.Consume();
            logger.LogInformation("Listening for RabbitMQ messages on Dota.API");

            heroDlxService.Consume();
            logger.LogInformation("DLX for Hero messages configured on Dota.API");

            statisticsDlxService.Consume();
            logger.LogInformation("DLX for Statistics messages configured on Dota.API");
        }
        catch (Exception e)
        {
            logger.LogCritical(e.Message);
        }
        
        return Task.CompletedTask;
    }
}