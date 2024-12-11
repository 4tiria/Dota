using Domain.Mongo.Statistics.Models.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace Domain.Mongo.Statistics;

public class MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings)
{
    private readonly IMongoDatabase _database = new MongoClient(mongoDbSettings.Value.ConnectionURI)
        .GetDatabase(mongoDbSettings.Value.DatabaseName);

    public IMongoCollection<HeroStatistic> HeroStatistics => _database.GetCollection<HeroStatistic>("heroStatistic");
}
