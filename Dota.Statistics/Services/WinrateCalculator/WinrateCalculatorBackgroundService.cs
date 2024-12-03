namespace Dota.Statistics.Services.BackgroundWorkers;

public class WinrateCalculatorBackgroundService(ILogger<WinrateCalculatorBackgroundService> logger, IWinrateCalculatorService calculatorService) : BackgroundService
{
    private readonly ILogger<WinrateCalculatorBackgroundService> _logger = logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Recalculating heroes winrate {time}", DateTimeOffset.UtcNow);

                // Можно вызвать метод, например:
                await calculatorService.Calculate();
                
                await Task.Delay(_interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Прерывание задачи
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                // Логируй ошибку и продолжай цикл
            }
        }
    }
}