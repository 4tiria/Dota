using Domain.Mongo.API.Migrator;

namespace Dota.API;

public class Program
{
    public static void Main(string[] args)
    {
        var build = CreateHostBuilder(args).Build();

        if (args.Contains("--migrate"))
        {
            var migrator = build.Services.GetRequiredService<IMigratorService>();
            migrator.Execute();
        }

        build.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}