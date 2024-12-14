using Dota.Statistics.ForHero.Winrate.RabbitMQ.Consumers;
using Dota.Statistics.ForHero.Winrate.RabbitMQ.Producers;
using Dota.Statistics.ForHero.Winrate.WinrateCalculator;
using Dota.Statistics.ForMatches.RabbitMq.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Domain.Mongo.Statistics.MongoDbSettings>(builder.Configuration.GetSection("MongoDBStatistics"));
builder.Services.Configure<Domain.Mongo.API.MongoDbSettings>(builder.Configuration.GetSection("MongoDBApi"));

builder.Services
    .AddTransient<Domain.Mongo.API.MongoDbContext>()
    .AddTransient<Domain.Mongo.Statistics.MongoDbContext>()
    .AddSingleton<IHeroStatisticProducerService, HeroStatisticProducerService>()
    .AddSingleton<IHeroStatisticConsumerService, HeroStatisticConsumerService>()
    .AddSingleton<IHeroStatisticConsumerService, HeroWinrateStatisticConsumerService>()
    .AddSingleton<IHeroWinrateCalculatorService, HeroWinrateCalculatorService>()
    .AddSingleton<IMatchStatisticConsumerService, MatchStatisticConsumerService>();

builder.Services.AddHostedService<HeroWinrateCalculatorBackgroundService>();
builder.Services.AddHostedService<HeroWinrateConsumerBackgroundService>();
builder.Services.AddHostedService<MatchConsumerBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
