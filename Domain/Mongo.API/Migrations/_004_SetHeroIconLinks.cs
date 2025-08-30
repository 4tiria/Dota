using Dota.API.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Domain.Mongo.API.Migration;

public class _004_SetHeroIconLinks : IMigration
{
    private readonly string _iconsFullPath;
    private readonly string _iconsRelativePath;
    private readonly ILogger<_004_SetHeroIconLinks> _logger;

    private readonly Dictionary<string, string> _exclusions = new Dictionary<string, string>()
    {
        ["Anti-Mage"] = "Anti-Mage_icon.webp",
    };

    public _004_SetHeroIconLinks(ILogger<_004_SetHeroIconLinks> logger, IConfiguration configuration)
    {
        _iconsFullPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configuration["Assets:RelativePath"]!, "icons"));  
        _iconsRelativePath = Path.Combine(configuration["Assets:Url"]!, "icons");  
        _logger = logger;
        _logger.LogInformation("AppContext.BaseDirectory = {Path}", AppContext.BaseDirectory);
        _logger.LogInformation("_iconsPath = {Path}", _iconsFullPath);
        _logger.LogInformation("assets = {Path}", Path.Combine("app", "../assets"));
        _logger.LogInformation("assets = {Path}", Path.GetFullPath(Path.Combine("app", "../assets")));
    }
    
    public int Version => 4;
    public void Upgrade(MongoDbContext database, IClientSessionHandle session)
    {
        var heroes = database.Heroes.AsQueryable();

        foreach (var hero in heroes)
        {
            var fullPath = Path.Combine(_iconsFullPath, $"{hero.LocalizedName.ToCamelCaseWithUnderscore()}_icon.webp");
            var link = Path.Combine(_iconsRelativePath, $"{hero.LocalizedName.ToCamelCaseWithUnderscore()}_icon.webp");
            if (_exclusions.TryGetValue(hero.LocalizedName, out var exclusion))
            {
                fullPath = Path.Combine(_iconsFullPath, exclusion);
                link = Path.Combine(_iconsRelativePath, exclusion);
            }
    
            if (!File.Exists(fullPath))
            {
                _logger.LogInformation("Could not find icon {link}, skipping...", link);
                continue;
            }

            var filter = Builders<Hero>.Filter.Eq(h => h.Id, hero.Id);
            var update = Builders<Hero>.Update.Set(h => h.ImageLink, link);
            
            database.Heroes.UpdateOne(session, filter, update);
        }
    }
}