using Domain.Mongo.Statistics.Models.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace Domain.Mongo.Statistics;

public class MongoDbContext(IOptions<MongoDbSettings> mongoDBSettings)
{
    private readonly IMongoDatabase _database = new MongoClient(mongoDBSettings.Value.ConnectionURI)
        .GetDatabase(mongoDBSettings.Value.DatabaseName);

    public IMongoCollection<HeroStatistic> HeroStatistics => _database.GetCollection<HeroStatistic>("heroStatistic");
}
