using System.Text.Json;
using Confluent.Kafka;
using Dota.Generator.Model;
using StackExchange.Redis;

namespace Dota.Generator.BackgroundWorkers;

public class RedisToKafkaWorker(ILogger<RedisToKafkaWorker> logger, IConfiguration configuration) : BackgroundService
{
    private readonly ProducerConfig _config = new()
    {
        BootstrapServers = configuration["Kafka:Url"]
    };
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var redis = await ConnectionMultiplexer.ConnectAsync(configuration["Redis:Url"]!);
        var subscriber = redis.GetSubscriber();
        
        await subscriber.SubscribeAsync(new RedisChannel("accounts", RedisChannel.PatternMode.Literal), async (channel, message) =>
        {
            try
            {
                using var producer = new ProducerBuilder<string, string>(_config).Build();
                var accountJson = message.ToString();
                var account = JsonSerializer.Deserialize<GenerateAccountRequest>(accountJson);
                if (account is null)
                {
                    logger.LogInformation("Account is null");
                    return;
                }

                var kafkaMessage = new Message<string, string>
                {
                    Key = account.Id.ToString(),
                    Value = accountJson
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
        });
    }
}