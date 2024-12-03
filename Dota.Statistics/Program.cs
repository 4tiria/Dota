using Dota.Statistics.RabbitMQ;
using Dota.Statistics.RabbitMQConsumerService;
using Dota.Statistics.Services.BackgroundWorkers;

var builder = WebApplication.CreateBuilder(args);

// builder.WebHost.UseKestrel(options =>
// {
//     options.ListenAnyIP(80); 
// });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Domain.Mongo.Statistics.MongoDbSettings>(builder.Configuration.GetSection("MongoDBStatistics"));
builder.Services.Configure<Domain.Mongo.API.MongoDbSettings>(builder.Configuration.GetSection("MongoDBApi"));

builder.Services
    .AddTransient<Domain.Mongo.API.MongoDbContext>()
    .AddTransient<Domain.Mongo.Statistics.MongoDbContext>()
    .AddSingleton<IRabbitMQConsumerService, RabbitMQConsumerService>()
    .AddSingleton<IWinrateCalculatorService, WinrateCalculatorService>();

builder.Services.AddHostedService<WinrateCalculatorBackgroundService>();

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
