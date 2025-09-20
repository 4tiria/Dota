namespace Dota.Generator.BackgroundWorkers.TriggerStrategy;

public interface ITriggerStrategy
{
    Task WaitForTriggerAsync(CancellationToken stoppingToken);
}