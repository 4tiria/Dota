using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoSql.Models;
using MongoDB.Driver.GridFS;
using NoSql;

namespace Domain.NoSql;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
        _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        GridFS = new GridFSBucket(_database);
    }

    public GridFSBucket GridFS { get; }
    public IMongoCollection<News> News => _database.GetCollection<News>("News");
    public IMongoCollection<Hero> Heroes => _database.GetCollection<Hero>("Heroes");
    public IMongoCollection<Match> Matches => _database.GetCollection<Match>("Matches");
    public IMongoCollection<global::NoSql.Models.Migration> Migrations => _database.GetCollection<global::NoSql.Models.Migration>("Migrations");
}
