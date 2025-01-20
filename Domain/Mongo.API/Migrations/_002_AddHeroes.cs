using AutoMapper;
using Domain.Mongo.API.Helpers;
using Domain.Mongo.API.Mappers.Hero.DTO;
using Domain.Mongo.API.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Domain.Mongo.API.Migration;

public class _002_AddHeroes : IMigration
{
    private readonly IMongoClient _client;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly string _heroesJsonPath;

    public _002_AddHeroes(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _client = new MongoClient(configuration["MongoDB:ConnectionURI"]);
        _mapper = mapper;
        _heroesJsonPath = Path.Combine(AppContext.BaseDirectory, "assets", "seed", "heroes.json");
    }

    public int Version => 2;

    public void Upgrade(MongoDbContext database, IClientSessionHandle session)
    {
        var mongoDatabase = _client.GetDatabase(_configuration["MongoDB:DatabaseName"]);
        mongoDatabase.CreateCollection(session, "Heroes");

        var json = File.ReadAllText(_heroesJsonPath);
        var seedHeroDtoList = JsonConvert.DeserializeObject<List<SeedHeroDto>>(json);
        var allHeroes = _mapper.Map<List<Hero>>(seedHeroDtoList);

        database.Heroes.InsertMany(session, allHeroes);
    }
}