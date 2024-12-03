using Domain.Mongo.API.Models;

namespace Domain.Mongo.API.Repositories.NewsRepository;

public interface INewsRepository
{
    Task CreateAsync(News news);
    void DeleteAll();
    Task DeleteAsync(Guid id);
    Task<List<News>> GetAsync();
}
