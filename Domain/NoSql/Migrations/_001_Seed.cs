using Domain.NoSql.Helpers;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using NoSql;
using NoSql.Models;

namespace Domain.NoSql.Migration;

public class _001_Seed : IMigration
{
    private readonly IMongoClient _client;
    private readonly IConfiguration _configuration;

    public _001_Seed(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new MongoClient(configuration["MongoDB:ConnectionURI"]);
    }

    public int Version => 1;

    public void Upgrade(MongoDbContext database, IClientSessionHandle session)
    {
        var mongoDatabase = _client.GetDatabase(_configuration["MongoDB:DatabaseName"]);
        mongoDatabase.CreateCollection(session, "Migrations");
    }
}