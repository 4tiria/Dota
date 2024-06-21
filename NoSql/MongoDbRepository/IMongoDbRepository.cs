using MongoDB.Driver;
using NoSql.Models;

namespace NoSql.MongoDbRepository;

public interface IMongoDbRepository
{
    Task<List<News>> Get();
    Task Create(News news);
    Task Delete(Guid id);

    void DeleteAll();
}
