using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Domain.NoSql.Auth.Models.Entities;

namespace Domain.NoSql.Auth;

public class MongoDbContext(IOptions<MongoDbSettings> mongoDBSettings)
{
    private readonly IMongoDatabase _database = new MongoClient(mongoDBSettings.Value.ConnectionURI)
        .GetDatabase(mongoDBSettings.Value.DatabaseName);

    public IMongoCollection<Account> Accounts => _database.GetCollection<Account>("account");
    public IMongoCollection<RefreshToken> RefreshTokens => _database.GetCollection<RefreshToken>("refreshToken");
}
