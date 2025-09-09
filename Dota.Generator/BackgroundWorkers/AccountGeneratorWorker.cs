using Dota.Generator.Model;

namespace Dota.Generator.BackgroundWorkers;

public class AccountGeneratorWorker(ILogger<AccountGeneratorWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var line = await Task.Run(Console.ReadLine, stoppingToken);
            if (line == null)
            {
                continue;
            }

            var account = new GenerateAccountRequest
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                NickName = $"User_{Guid.NewGuid():N}".Substring(0, 8),
                Email = null,
                Avatar = null
            };

            // TODO: логика отправки на эндпоинт
            
            logger.LogInformation("Generated account {@Account}", account);
        }
    }
}