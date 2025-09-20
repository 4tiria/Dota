using Dota.Generator.BackgroundWorkers;
using Dota.Generator.BackgroundWorkers.TriggerStrategy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddHostedService<RedisToKafkaWorker>();
builder.Services.AddHostedService<AccountGeneratorWorker>();

builder.Services.AddSingleton<ITriggerStrategy>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var type = configuration["Trigger:Type"]!;
    return type.ToLower() switch
    {
        "console" => new ConsoleTriggerStrategy(),
        "timer" => new ConstantTimeTriggerStrategy(TimeSpan.FromSeconds(
            int.TryParse(configuration["Trigger:IntervalSeconds"], out var seconds) ? seconds : 30)),
        _ => throw new InvalidOperationException($"Unknown trigger type: {type}")
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();