using Domain.Mongo.API.Models;
using Domain.Mongo.API.Repositories.NewsRepository;
using Dota.Common;

namespace Domain.Mongo.API.Seeds;

public class NewsSeed(INewsRepository newsRepository) : ISeed
{
    public void SeedData()
    {
        newsRepository.DeleteAll();
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
            newsRepository.CreateAsync(news).ConfigureAwait(true);
        }
    }
}
