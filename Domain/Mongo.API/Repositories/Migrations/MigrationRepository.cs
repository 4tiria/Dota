using MongoDB.Driver;

namespace Domain.Mongo.API.Repositories.Migrations;

public class MigrationRepository(MongoDbContext context) : IMigrationRepository
{
    private readonly IMongoCollection<Models.Migration> _migrations = context.Migrations;

    public int GetCurrentDbVersion()
    {
        var lastMigration = _migrations
            .Find(Builders<Models.Migration>.Filter.Empty)
            .Sort(Builders<Models.Migration>.Sort.Descending(migration => migration.Version))
            .FirstOrDefaultAsync();

        return lastMigration?.Result?.Version ?? 0;
    }

    public void Add(int version, string description = null)
    {
        _migrations.InsertOne(new Models.Migration
        {
            Id = version,
            Version = version,
            Description = description,
            AppliedOn = DateTime.UtcNow
        });
    }
}