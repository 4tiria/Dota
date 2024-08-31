using Domain.NoSql;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using NoSql.Models;

namespace CoreModule.Matches.Repository;

public class MatchRepository(MongoDbContext context) : IMatchRepository
{
    private readonly IMongoCollection<Match> _matches = context.Matches;
    private readonly GridFSBucket _gridFS = context.GridFS;

    public async Task<List<Match>> GetMatchesAsync()
    {
        return await _matches.Find(match => true).ToListAsync();
    }

    public async Task<Match> GetMatchByIdAsync(ObjectId id)
    {
        return await _matches.Find(match => match.Id == id).FirstOrDefaultAsync();
    }

    //TODO:
    public async Task CreateMatchAsync(Match match)
    {
        //using (var transaction = context.Database.BeginTransaction())
        //{
        //    try
        //    {
        //        await _matches.InsertOneAsync(match);
        //        transaction.Commit();
        //    } 
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //    }
            
        //}
        
    }

    public async Task UpdateMatchAsync(Match updatedMatch)
    {
        await _matches.ReplaceOneAsync(match => match.Id == updatedMatch.Id, updatedMatch);
    }

    public async Task DeleteMatchAsync(ObjectId id)
    {
        await _matches.DeleteOneAsync(match => match.Id == id);
    }
}
