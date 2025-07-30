using Dota.API.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Domain.Mongo.API.Migration;

public class _004_SetHeroIconLinks : IMigration
{
    private readonly string _iconsPath;
    private readonly ILogger<_004_SetHeroIconLinks> _logger;

    private readonly Dictionary<string, string> _exclusions = new Dictionary<string, string>()
    {
        ["Anti-Mage"] = "Anti-Mage_icon.webp",
    };

    public _004_SetHeroIconLinks(ILogger<_004_SetHeroIconLinks> logger, IConfiguration configuration)
    {
        _iconsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configuration["Assets:RelativePath"]!, "icons"));  
        _logger = logger;
        _logger.LogInformation("AppContext.BaseDirectory = {Path}", AppContext.BaseDirectory);
        _logger.LogInformation("_iconsPath = {Path}", _iconsPath);
        _logger.LogInformation("assets = {Path}", Path.Combine("app", "../assets"));
        _logger.LogInformation("assets = {Path}", Path.GetFullPath(Path.Combine("app", "../assets")));
    }
    
    public int Version => 4;
    public void Upgrade(MongoDbContext database, IClientSessionHandle session)
    {
        var heroes = database.Heroes.AsQueryable();

        foreach (var hero in heroes)
        {
            var link = Path.Combine(_iconsPath, $"{hero.LocalizedName.ToCamelCaseWithUnderscore()}_icon.webp");
            if (_exclusions.TryGetValue(hero.LocalizedName, out var exclusion))
            {
                link = Path.Combine(_iconsPath, exclusion);
            }
    
            if (!File.Exists(link))
            {
                _logger.LogInformation($"Could not find icon {link}, skipping...");
                continue;
            }

            var filter = Builders<Hero>.Filter.Eq(h => h.Id, hero.Id);
            var update = Builders<Hero>.Update.Set(h => h.ImageLink, link);
            
            database.Heroes.UpdateOne(session, filter, update);
        }
    }
}