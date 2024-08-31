﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoSql.Models;
using MongoDB.Driver.GridFS;
using NoSql;

namespace Domain.NoSql;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> mongoDBSettings)
    {
        var client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        _database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        GridFS = new GridFSBucket(_database);
    }

    public GridFSBucket GridFS { get; }
    public IMongoCollection<News> News => _database.GetCollection<News>("News");
    public IMongoCollection<Hero> Heroes => _database.GetCollection<Hero>("Heroes");
    public IMongoCollection<Match> Matches => _database.GetCollection<Match>("Matches");
}
