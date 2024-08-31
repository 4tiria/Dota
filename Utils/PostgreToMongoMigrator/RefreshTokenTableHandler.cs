﻿
using Domain.NoSql.Auth;
using Domain.NoSql.Auth.Models;
using MongoDB.Driver;

namespace MySqlToMongoMigrator;

public class RefreshTokenTableHandler(MongoDbContext mongoDbContext, AuthContext mySqlContext)
{
    public void Execute()
    {
        mongoDbContext.RefreshTokens.DeleteMany(hero => true);

        var refreshTokens = mySqlContext.RefreshTokens.AsEnumerable().ToList();
        foreach (var refreshToken in refreshTokens)
        {
            if (mongoDbContext.RefreshTokens.Find(x => x.Token == refreshToken.Token) != null)
            {
                mongoDbContext.RefreshTokens.DeleteOne(h => h.Token == refreshToken.Token);
            }

            mongoDbContext.RefreshTokens.InsertOne(new()
            {
                Token = refreshToken.Token,
                AccessTokenId = refreshToken.AccessTokenId,
                AccountId = refreshToken.AccountId,
                CreationDate = refreshToken.CreationDate,
                ExpireDate = refreshToken.ExpireDate,
                Invalidated = refreshToken.Invalidated,
                Used = refreshToken.Used,
            });
        }
    }
}
