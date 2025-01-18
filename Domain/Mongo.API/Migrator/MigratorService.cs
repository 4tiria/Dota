using Domain.Mongo.API.Migration;
using Domain.Mongo.API.Repositories.Migrations;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Domain.Mongo.API.Migrator;

public class MigratorService(
    ILogger<MigratorService> logger,
    IEnumerable<IMigration> migrations,
    IMongoClient client,
    MongoDbContext context,
    IMigrationRepository migrationRepository) : IMigratorService
{
    public void Execute()
    {
        var lastMigration = migrationRepository.GetCurrentDbVersion();

        var migrationList = migrations.Where(migration => migration.Version > lastMigration).ToList();

        if (migrationList.Count == 0)
        {
            logger.LogInformation("No migrations to be applied");
            return;
        }

        if (lastMigration == 0) client.DropDatabase("dota");

        logger.LogInformation("Executing migrator service, applying {count} migrations", migrationList.Count);
        foreach (var migration in migrationList)
        {
            logger.LogInformation("Executing migration {version}/{count}: {name}",
                migration.Version, migrationList.Count + lastMigration, migration.GetType().Name);

            RunTransaction(session => { migration.Upgrade(context, session); });
            migrationRepository.Add(migration.Version);
        }
    }

    private void RunTransaction(Action<IClientSessionHandle> operation)
    {
        using var session = client.StartSession();
        session.StartTransaction();

        try
        {
            operation(session);
            session.CommitTransaction();
        }
        catch (Exception ex)
        {
            session.AbortTransaction();
            logger.LogError("Transaction aborted due to error: {error}", ex.Message);
            throw;
        }
    }
}