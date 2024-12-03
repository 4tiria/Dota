using Domain.NoSql.Helpers;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using NoSql;
using NoSql.Models;

namespace Domain.NoSql.Migration;

public class _002_AddHeroes : IMigration
{
    private readonly IMongoClient _client;
    private readonly IConfiguration _configuration;

    public _002_AddHeroes(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new MongoClient(configuration["MongoDB:ConnectionURI"]);
    }

    public int Version => 2;

    public void Upgrade(MongoDbContext database, IClientSessionHandle session)
    {
        var mongoDatabase = _client.GetDatabase(_configuration["MongoDB:DatabaseName"]);
        mongoDatabase.CreateCollection(session, "Heroes");

        var heroes = new List<Hero>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Abaddon",
                MainAttribute = "Strength",
                AttackType = "Melee",
                Tags = ["Support", "Carry", "Durable"]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Earthshaker",
                MainAttribute = "Strength",
                AttackType = "Melee",
                Tags = ["Support", "Initiator", "Disabler", "Nuker"]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Hoodwink",
                MainAttribute = "Agility",
                AttackType = "Range",
                Tags = ["Support", "Nuker", "Escape", "Disabler"]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Invoker",
                MainAttribute = "Intelligence",
                AttackType = "Range",
                Tags = ["Carry", "Nuker", "Disabler", "Escape", "Pusher"]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Naga Siren",
                MainAttribute = "Agility",
                AttackType = "Melee",
                Tags = ["Carry", "Support", "Pusher", "Disabler", "Initiator", "Escape"]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Shadow Demon",
                MainAttribute = "Strength",
                AttackType = "Range",
                Tags = ["Support", "Disabler", "Initiator", "Nuker"]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Slardar",
                MainAttribute = "Strength",
                AttackType = "Melee",
                Tags = ["Carry", "Durable", "Initiator", "Disabler", "Escape"]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Snapfire",
                MainAttribute = "Strength",
                AttackType = "Range",
                Tags = ["Support", "Nuker", "Disabler", "Escape"]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sven",
                MainAttribute = "Strength",
                AttackType = "Melee",
                Tags = ["Carry", "Disabler", "Initiator", "Durable", "Nuker"]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Timbersaw",
                MainAttribute = "Strength",
                AttackType = "Melee",
                Tags = ["Nuker", "Durable", "Escape"]
            },
        };

        database.Heroes.InsertMany(session, heroes);

        var matches = MockMatchGenerator.CreateMatches(database, 1);
        
        database.Matches.InsertMany(session, matches);
    }
}