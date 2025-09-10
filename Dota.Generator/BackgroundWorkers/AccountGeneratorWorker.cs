using System.Text.Json;
using Confluent.Kafka;
using Dota.Generator.Model;

namespace Dota.Generator.BackgroundWorkers;

public class AccountGeneratorWorker(ILogger<AccountGeneratorWorker> logger, IConfiguration configuration) : BackgroundService
{
    private readonly ProducerConfig _config = new()
    {
        BootstrapServers = configuration["Kafka:Url"]
    };
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var producer = new ProducerBuilder<string, string>(_config).Build();
        
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
                NickName = $"User_{Guid.NewGuid():N}"[..8],
                Email = null,
                Avatar = null
            };
            
            var message = new Message<string, string>
            {
                Key = account.Id.ToString(),
                Value = JsonSerializer.Serialize(account)
            };

            try
            {
                var result = await producer.ProduceAsync("accounts", message, stoppingToken);
                logger.LogInformation("Sent account {Id} to Kafka partition {Partition} offset {Offset}",
                    account.Id, result.Partition, result.Offset);
            }
            catch (ProduceException<string, string> e)
            {
                logger.LogError(e, "Delivery failed: {Reason}", e.Error.Reason);
                throw;
            }
            
            logger.LogInformation("Generated account {@Account}", account);
        }
    }
}