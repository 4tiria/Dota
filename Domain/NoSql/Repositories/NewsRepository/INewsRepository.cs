using NoSql.Models;

namespace NoSql.Repositories.NewsRepository;

public interface INewsRepository
{
    Task CreateAsync(News news);
    void DeleteAll();
    Task DeleteAsync(Guid id);
    Task<List<News>> GetAsync();
}
