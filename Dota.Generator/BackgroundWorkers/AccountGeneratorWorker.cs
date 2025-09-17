using System.Text.Json;
using Confluent.Kafka;
using Dota.Generator.Model;
using StackExchange.Redis;

namespace Dota.Generator.BackgroundWorkers;

public class AccountGeneratorWorker(ILogger<AccountGeneratorWorker> logger, IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var redis = await ConnectionMultiplexer.ConnectAsync(configuration["Redis:Url"]!);
        var redisDatabase = redis.GetDatabase();
        var subscriber = redis.GetSubscriber();
        
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

            var accountJson = JsonSerializer.Serialize(account);
            var result = redisDatabase.StringSet($"account:{account.Id}", accountJson);
            await subscriber.PublishAsync(new RedisChannel("accounts", RedisChannel.PatternMode.Literal), accountJson);
            
            if (result)
            {
                logger.LogInformation("{CreationDate} Added account {Id} nickname {NickName}",
                    account.CreationDate, account.Id, account.NickName);
            }
            else
            {
                logger.LogInformation("Unable to set key for id {Id}", account.Id);
            }
        }
    }
}