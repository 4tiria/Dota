using System.Text.Json;
using Dota.Generator.BackgroundWorkers.TriggerStrategy;
using StackExchange.Redis;
using Dota.Generator.Application.Commands.GenerateAccount;

namespace Dota.Generator.BackgroundWorkers;

public class AccountGeneratorWorker(ILogger<AccountGeneratorWorker> logger, IConfiguration configuration, ITriggerStrategy triggerStrategy) : BackgroundService
{
    private readonly Random _random = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var redis = await ConnectionMultiplexer.ConnectAsync(configuration["Redis:Url"]!);
        var redisDatabase = redis.GetDatabase();
        var subscriber = redis.GetSubscriber();

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

            var account = new GenerateAccountRequest
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                NickName = allNicknames[_random.Next(0, allNicknames.Length - 1)],
                Email = allEmails[_random.Next(0, allEmails.Length - 1)],
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