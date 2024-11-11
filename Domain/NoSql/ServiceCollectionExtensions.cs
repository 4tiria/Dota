using CoreModule.Heroes.Repository;
using CoreModule.Matches.Repository;
using Domain.NoSql.Migration;
using Domain.NoSql.Migrator;
using Domain.NoSql.Repositories.Migrations;
using Domain.NoSql.Repositories.NewsRepository;
using Domain.NoSql.Seeds;
using Dota.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoSql;

namespace Domain.NoSql;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNoSql(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddTransient<MongoDbContext>()
            .AddSingleton<IMongoClient>(sp => new MongoClient(configuration.GetSection("MongoDB")["ConnectionURI"]))
            .AddTransient<IMigration, _001_Seed>()
            .AddTransient<IMigration, _002_AddWinrate>()
            
            .AddTransient<IMigrationRepository, MigrationRepository>()
            .AddTransient<IMigratorService, MigratorService>()
            .AddTransient<IHeroRepository, HeroRepository>()
            .AddTransient<IMatchRepository, MatchRepository>()
            .AddTransient<INewsRepository, NewsRepository>()
            .AddTransient<ISeed, NewsSeed>();
    }
}