using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NoSql.Models;

namespace NoSql.MongoDbRepository;

public class MongoDbRepository : IMongoDbRepository
{

    private readonly IMongoCollection<News> _newsCollection;

    public MongoDbRepository(IOptions<MongoDBSettings> mongoDBSettings)
    {
        var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _newsCollection = database.GetCollection<News>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<News>> Get()
    {
        return await _newsCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task Create(News news)
    {
        await _newsCollection.InsertOneAsync(news);
    }

    public async Task Delete(Guid id)
    {
        await _newsCollection.DeleteOneAsync(Builders<News>.Filter.Eq("Id", id));
    }

    public void DeleteAll()
    {
        _newsCollection.DeleteMany(Builders<News>.Filter.Empty);
    }
}
