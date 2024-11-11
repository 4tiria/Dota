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
        _client.DropDatabase(session, "dota");

        var mongoDatabase = _client.GetDatabase(_configuration["MongoDB:DatabaseName"]);
        mongoDatabase.CreateCollection(session, "Heroes");

        var heroes = new List<Hero>
        {
            new()
            {
                Name = "Abaddon",
                MainAttribute = "Strength",
                AttackType = "Melee",
                Tags = ["Support", "Carry", "Durable"]
            },
            new()
            {
                Name = "Earthshaker",
                MainAttribute = "Strength",
                AttackType = "Melee",
                Tags = ["Support", "Initiator", "Disabler", "Nuker"]
            },
            new()
            {
                Name = "Hoodwink",
                MainAttribute = "Agility",
                AttackType = "Range",
                Tags = ["Support", "Nuker", "Escape", "Disabler"]
            },
            new()
            {
                Name = "Invoker",
                MainAttribute = "Intelligence",
                AttackType = "Range",
                Tags = ["Carry", "Nuker", "Disabler", "Escape", "Pusher"]
            },
            new()
            {
                Name = "Naga Siren",
                MainAttribute = "Agility",
                AttackType = "Melee",
                Tags = ["Carry", "Support", "Pusher", "Disabler", "Initiator", "Escape"]
            },
            new()
            {
                Name = "Shadow Demon",
                MainAttribute = "Strength",
                AttackType = "Range",
                Tags = ["Support", "Disabler", "Initiator", "Nuker"]
            },
            new()
            {
                Name = "Slardar",
                MainAttribute = "Strength",
                AttackType = "Melee",
                Tags = ["Carry", "Durable", "Initiator", "Disabler", "Escape"]
            },
            new()
            {
                Name = "Snapfire",
                MainAttribute = "Strength",
                AttackType = "Range",
                Tags = ["Support", "Nuker", "Disabler", "Escape"]
            },
            new()
            {
                Name = "Sven",
                MainAttribute = "Strength",
                AttackType = "Melee",
                Tags = ["Carry", "Disabler", "Initiator", "Durable", "Nuker"]
            },
            new()
            {
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