using Domain.Mongo.API.Models;
using MongoDB.Bson;

namespace Domain.Mongo.API.Matches.Repository;

public interface IMatchRepository
{
    Task CreateMatchAsync(Match match);
    Task DeleteMatchAsync(ObjectId id);
    Task<Match> GetMatchByIdAsync(ObjectId id);
    Task<List<Match>> GetMatchesAsync();
    Task UpdateMatchAsync(Match updatedMatch);
}
