namespace Dota.Generator.BackgroundWorkers.TriggerStrategy;

public class ConsoleTriggerStrategy: ITriggerStrategy
{
    public async Task WaitForTriggerAsync(CancellationToken stoppingToken)
    {
        await Task.Run(Console.ReadLine, stoppingToken);
    }
}