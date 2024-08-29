using MongoDB.Driver;
using NoSql.Models;

namespace NoSql.Repositories.NewsRepository;

public class NewsRepository(MongoDbContext context) : INewsRepository
{
    public async Task<List<News>> GetAsync()
    {
        return await context.News.Find(news => true).ToListAsync();
    }

    public async Task CreateAsync(News news)
    {
        await context.News.InsertOneAsync(news);
    }

    public async Task DeleteAsync(Guid id)
    {
        await context.News.DeleteOneAsync(Builders<News>.Filter.Eq("Id", id));
    }

    public void DeleteAll()
    {
        context.News.DeleteMany(Builders<News>.Filter.Empty);
    }
}
