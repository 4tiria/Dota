using NoSql.Models;

namespace CoreModule.Heroes.Repository;

public interface IHeroRepository
{
    Task CreateHeroAsync(Hero hero);
    Task DeleteHeroAsync(int id);
    Task<Hero> GetHeroByIdAsync(int id);
    Task<List<Hero>> GetHeroesAsync();
    Task UpdateHeroAsync(Hero updatedHero);
    void DeleteAll();
}
