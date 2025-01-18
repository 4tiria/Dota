using Domain.Mongo.API.Helpers;
using MongoDB.Driver;

namespace Domain.Mongo.API.Migration;

public class _003_AddMockMatches : IMigration
{
    public int Version => 3;
    
    public void Upgrade(MongoDbContext database, IClientSessionHandle session)
    {
        var matches = MockMatchGenerator.CreateMatches(database, 1);
        
        database.Matches.InsertMany(session, matches);
    }
}