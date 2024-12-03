using MongoDB.Driver;

namespace Domain.Mongo.API.Repositories.Migrations;

public class MigrationRepository(MongoDbContext context) : IMigrationRepository
{
    private readonly IMongoCollection<global::Domain.Mongo.API.Models.Migration> _migrations = context.Migrations;
    
    public int GetCurrentDbVersion()
    {
        var lastMigration = _migrations
            .Find(Builders<global::Domain.Mongo.API.Models.Migration>.Filter.Empty)
            .Sort(Builders<global::Domain.Mongo.API.Models.Migration>.Sort.Descending(a => a.Version))
            .FirstOrDefaultAsync();

        return lastMigration?.Result?.Version ?? 0;
    }

    public void Add(int version, string description = null)
    {
        _migrations.InsertOne(new global::Domain.Mongo.API.Models.Migration
        {
            Id = version, 
            Version = version, 
            Description = description, 
            AppliedOn = DateTime.UtcNow
        });
    }
}