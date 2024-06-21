using Dota.Common;
using NoSql.Models;
using NoSql.MongoDbRepository;

namespace NoSql.Seeds;

public class NewsSeed : ISeed
{
    private readonly IMongoDbRepository _repository;

    public NewsSeed(IMongoDbRepository repository)
    {
        _repository = repository;
    }

    public void SeedData()
    {
        _repository.DeleteAll();
        var collection = new List<News>()
        {
            new()
            {
                Name = "Earth Spirit",
                Time = DateTime.Now.AddHours(-24).Ticks,
            },

            new()
            {
                Name = "Warframe",
                Time = DateTime.Now.AddHours(-12).Ticks,
            },

            new()
            {
                Name = "Darkwood",
                Time = DateTime.Now.Ticks,
            }
        };

        foreach (var news in collection)
        {
            _repository.Create(news).ConfigureAwait(true);
        }
    }
}
