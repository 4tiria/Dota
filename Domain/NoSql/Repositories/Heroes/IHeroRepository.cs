using NoSql.Models;

namespace CoreModule.Heroes.Repository;

public interface IHeroRepository
{
    Task CreateHeroAsync(Hero hero);
    Task DeleteHeroAsync(Guid id);
    Task<Hero> GetHeroByIdAsync(Guid id);
    Task<List<Hero>> GetHeroesAsync();
    Task UpdateHeroAsync(Hero updatedHero);
    void DeleteAll();
}
