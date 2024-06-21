using MongoDB.Bson;
using NoSql.Models;

namespace CoreModule.Heroes.Repository;

public interface IHeroRepository
{
    Task CreateHeroAsync(Hero hero);
    Task DeleteHeroAsync(ObjectId id);
    Task<Hero> GetHeroByIdAsync(ObjectId id);
    Task<List<Hero>> GetHeroesAsync();
    Task UpdateHeroAsync(Hero updatedHero);
    void DeleteAll();
}
