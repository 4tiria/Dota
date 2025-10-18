using Domain.Mongo.API.Heroes.Repository;
using Domain.Mongo.API.Matches.Repository;
using Domain.Mongo.API.Migration;
using Domain.Mongo.API.Migrator;
using Domain.Mongo.API.Repositories.Migrations;
using Domain.Mongo.API.Repositories.NewsRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Domain.Mongo.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNoSql(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddTransient<MongoDbContext>()
            .AddSingleton<IMongoClient>(sp => new MongoClient(configuration.GetSection("MongoDB")["ConnectionURI"]))
            .AddTransient<IMigration, _001_Seed>()
            .AddTransient<IMigration, _002_AddHeroes>()
            .AddTransient<IMigration, _003_AddMockMatches>()
            .AddTransient<IMigration, _004_SetHeroIconLinks>()
            .AddTransient<IMigrationRepository, MigrationRepository>()
            .AddTransient<IMigratorService, MigratorService>()
            .AddTransient<IHeroRepository, HeroRepository>()
            .AddTransient<IMatchRepository, MatchRepository>()
            .AddTransient<INewsRepository, NewsRepository>();
    }
}