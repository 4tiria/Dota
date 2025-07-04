using Domain.Mongo.Statistics;
using Dota.Statistics.ForHero.Winrate.RabbitMQ.Consumers;
using Dota.Statistics.ForHero.Winrate.RabbitMQ.Producers;
using Dota.Statistics.ForHero.Winrate.WinrateCalculator;
using Dota.Statistics.ForMatches.RabbitMq.Consumers;
using RabbitMQ.Client;

#region services

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{env}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDBStatistics"));
builder.Services.Configure<Domain.Mongo.API.MongoDbSettings>(builder.Configuration.GetSection("MongoDBApi"));

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");
builder.Services
    .AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
    {
        HostName = rabbitMqSettings["HostName"],
        UserName = rabbitMqSettings["UserName"],
        Password = rabbitMqSettings["Password"]
    })
    .AddSingleton<IConnection>(serviceProvider =>
        serviceProvider.GetRequiredService<IConnectionFactory>().CreateConnection())
    
    .AddSingleton<IModel>(
        serviceProvider => serviceProvider.GetRequiredService<IConnection>().CreateModel())
    .AddTransient<Domain.Mongo.API.MongoDbContext>()
    .AddTransient<MongoDbContext>()
    .AddSingleton<IHeroStatisticProducerService, HeroStatisticProducerService>()
    .AddSingleton<IHeroStatisticConsumerService, HeroStatisticConsumerService>()
    .AddSingleton<IHeroStatisticConsumerService, HeroWinrateStatisticConsumerService>()
    .AddSingleton<IHeroWinrateCalculatorService, HeroWinrateCalculatorService>()
    .AddSingleton<IMatchStatisticConsumerService, MatchStatisticConsumerService>();

builder.Services.AddHostedService<HeroWinrateCalculatorBackgroundService>();
builder.Services.AddHostedService<HeroWinrateConsumerBackgroundService>();
builder.Services.AddHostedService<MatchConsumerBackgroundService>();


#endregion

var app = builder.Build();

#region app

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion