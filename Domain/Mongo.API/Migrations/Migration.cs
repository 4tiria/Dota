using MongoDB.Driver;

namespace Domain.Mongo.API.Migration;

public interface IMigration
{
    int Version { get; }
    void Upgrade(MongoDbContext database, IClientSessionHandle session);
}