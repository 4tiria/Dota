using MongoDB.Driver;
using NoSql.Models;

namespace Domain.NoSql.Migration;

public class _002_AddWinrate : IMigration
{
    private Random _random = new Random();
    
    public int Version => 2;
    
    public void Upgrade(MongoDbContext database, IClientSessionHandle session)
    {
        var updateDefinition = Builders<Hero>.Update.Set(h => h.Winrate, _random.NextDouble() * 100);
        database.Heroes.UpdateMany(session, Builders<Hero>.Filter.Empty, updateDefinition);
    }
}