using MongoDB.Driver;

namespace Domain.NoSql.Repositories.Migrations;

public class MigrationRepository(MongoDbContext context) : IMigrationRepository
{
    private readonly IMongoCollection<global::NoSql.Models.Migration> _migrations = context.Migrations;
    
    public int GetCurrentDbVersion()
    {
        var lastMigration = _migrations
            .Find(Builders<global::NoSql.Models.Migration>.Filter.Empty)
            .Sort(Builders<global::NoSql.Models.Migration>.Sort.Descending(a => a.Version))
            .FirstOrDefaultAsync();

        return lastMigration?.Result?.Version ?? 0;
    }
}