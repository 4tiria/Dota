namespace Dota.Generator.BackgroundWorkers.TriggerStrategy;

public class ConstantTimeTriggerStrategy(TimeSpan interval) : ITriggerStrategy
{
    public async Task WaitForTriggerAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(interval, stoppingToken);
    }
}