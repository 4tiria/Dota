using Dota.Generator.Application.Commands.GenerateAccount;
using Dota.Generator.BackgroundWorkers;
using Dota.Generator.BackgroundWorkers.TriggerStrategy;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

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

builder.Services.AddMassTransit(busRegistrationConfiguration =>
{
    busRegistrationConfiguration.UsingInMemory();

    busRegistrationConfiguration.AddRider(rider =>
    {
        rider.AddProducer<string, GenerateAccountRequest>("accounts");

        rider.UsingKafka((context, kafkaFactoryConfigurator) =>
        {
            kafkaFactoryConfigurator.Host(builder.Configuration["Kafka:Url:localhost"]);
        });
    });
});

var app = builder.Build();
var bus = app.Services.GetRequiredService<IBusControl>();
await bus.StartAsync();
try
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.Run();
}
finally
{
    await bus.StopAsync();
}