namespace Dota.Statistics.ForHero.Winrate.WinrateCalculator;

public class WinrateCalculatorBackgroundService(ILogger<WinrateCalculatorBackgroundService> logger, IWinrateCalculatorService calculatorService) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Recalculating heroes winrate {time}", DateTimeOffset.UtcNow);
                
                await calculatorService.Calculate();
                
                await Task.Delay(_interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка: {message}", ex.Message);
            }
        }
    }
}