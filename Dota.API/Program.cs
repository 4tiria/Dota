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
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}