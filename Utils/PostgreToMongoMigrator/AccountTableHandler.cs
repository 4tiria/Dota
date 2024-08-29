﻿using DataAccess;
using MongoDB.Driver;
using NoSql;
using NoSql.Models;

namespace MySqlToMongoMigrator;

public class AccountTableHandler(MongoDbContext mongoDbContext, ApiContext postgreContext)
{
    public void Execute()
    {
        mongoDbContext..DeleteMany(hero => true);

        var heroes = postgreContext.Heroes.AsEnumerable().ToList();
        foreach (var hero in heroes)
        {
            if (mongoDbContext.Heroes.Find(x => x.Id == hero.Id) != null)
            {
                mongoDbContext.Heroes.DeleteOne(h => h.Id == hero.Id);
            }

            mongoDbContext.Heroes.InsertOne(new Hero()
            {
                Id = hero.Id,
                AttackType = hero.AttackType,
                MainAttribute = hero.MainAttribute,
                Name = hero.Name,
                Tags = hero.Tags.Select(x => x.Name).ToList(),
                Image = hero.Image.Bytes
            });
        }
    }
}
