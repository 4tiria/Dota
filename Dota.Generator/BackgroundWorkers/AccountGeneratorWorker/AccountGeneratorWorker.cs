using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Dota.Generator.BackgroundWorkers.TriggerStrategy;
using StackExchange.Redis;
using Dota.Generator.Application.Commands.GenerateAccount;

namespace Dota.Generator.BackgroundWorkers;

public class AccountGeneratorWorker(ILogger<AccountGeneratorWorker> logger, IConfiguration configuration, ITriggerStrategy triggerStrategy) : BackgroundService
{
    private readonly Random _random = new();
    
    private readonly ProducerConfig _config = new()
    {
        //can be configured, but I'm too lazy for it
        BootstrapServers = configuration["Kafka:Url:Localhost"]
    };
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //TODO: relocate to Redis, so to use multiple producers
        var allNicknames = await File.ReadAllLinesAsync(
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configuration["Assets:Nicknames"]!)),
            cancellationToken: stoppingToken);    
        
        var allEmails = await File.ReadAllLinesAsync(
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configuration["Assets:Emails"]!)),
            cancellationToken: stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await triggerStrategy.WaitForTriggerAsync(stoppingToken);

            try
            {
                using var producer = new ProducerBuilder<string, string>(_config).Build();
                
                var account = new GenerateAccountRequest
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.UtcNow,
                    NickName = allNicknames[_random.Next(0, allNicknames.Length - 1)],
                    Email = allEmails[_random.Next(0, allEmails.Length - 1)],
                    Avatar = null
                };

                var kafkaMessage = new Message<string, string>
                {
                    Key = account.Id.ToString(),
                    Value = JsonSerializer.Serialize(account)
                };
                
                var result = await producer.ProduceAsync("accounts", kafkaMessage, stoppingToken);
                logger.LogInformation("Sent account {Id} to Kafka partition {Partition} offset {Offset}",
                    account.Id, result.Partition, result.Offset);
            }
            catch (JsonException e)
            {
                logger.LogError(e, "Unable to deserialize account: {Reason}", e.Message);
            }
            catch (ProduceException<string, string> e)
            {
                logger.LogError(e, "Delivery failed: {Reason}", e.Error.Reason);
                throw;
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }
    }
}