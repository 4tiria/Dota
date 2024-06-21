using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using NoSql;
using NoSql.Models;

namespace CoreModule.Heroes.Repository;

public class HeroRepository(MongoDbContext context) : IHeroRepository
{
    private readonly IMongoCollection<Hero> _heroes = context.Heroes;
    private readonly IMongoCollection<Match> _matches = context.Matches;
    private readonly GridFSBucket _gridFS = context.GridFS;

    public async Task<List<Hero>> GetHeroesAsync()
    {
        return await _heroes.Find(hero => true).ToListAsync();
    }

    public async Task<Hero> GetHeroByIdAsync(ObjectId id)
    {
        return await _heroes.Find(hero => hero.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateHeroAsync(Hero hero)
    {
        await _heroes.InsertOneAsync(hero);
    }

    public async Task UpdateHeroAsync(Hero updatedHero)
    {
        var existingHero = await GetHeroByIdAsync(updatedHero.Id);
        if (existingHero != null)
        {
            await _heroes.ReplaceOneAsync(hero => hero.Id == updatedHero.Id, updatedHero);
        }
    }

    public async Task DeleteHeroAsync(ObjectId id)
    {
        var hero = await GetHeroByIdAsync(id);
        if (hero != null)
        {
            await _heroes.DeleteOneAsync(h => h.Id == id);
        }
    }

    //public async Task CreateHeroAsync(Hero hero, byte[] imageData, string fileName)
    //{
    //    hero.ImageFileId = await UploadImageAsync(imageData, fileName);
    //    await _heroes.InsertOneAsync(hero);
    //}

    //public async Task UpdateHeroAsync(ObjectId id, Hero updatedHero, byte[] imageData, string fileName)
    //{
    //    var existingHero = await GetHeroByIdAsync(id);
    //    if (existingHero != null)
    //    {
    //        if (imageData != null && imageData.Length > 0)
    //        {
    //            await DeleteImageAsync(existingHero.ImageFileId);
    //            updatedHero.ImageFileId = await UploadImageAsync(imageData, fileName);
    //        }
    //        await _heroes.ReplaceOneAsync(hero => hero.Id == id, updatedHero);
    //    }
    //}

    //public async Task DeleteHeroAsync(ObjectId id)
    //{
    //    var hero = await GetHeroByIdAsync(id);
    //    if (hero != null)
    //    {
    //        await DeleteImageAsync(hero.ImageFileId);
    //        await _heroes.DeleteOneAsync(h => h.Id == id);
    //    }
    //}

    public async Task<ObjectId> UploadImageAsync(byte[] imageData, string fileName)
    {
        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "filename", fileName }
            }
        };

        return await _gridFS.UploadFromBytesAsync(fileName, imageData, options);
    }

    public async Task<byte[]> DownloadImageAsync(ObjectId id)
    {
        return await _gridFS.DownloadAsBytesAsync(id);
    }

    public async Task DeleteImageAsync(ObjectId id)
    {
        await _gridFS.DeleteAsync(id);
    }

    public void DeleteAll()
    {
        _heroes.DeleteMany(hero => true);
    }
}
