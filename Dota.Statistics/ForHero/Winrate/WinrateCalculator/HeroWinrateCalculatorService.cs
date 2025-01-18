using Domain.Mongo.API;
using Domain.Mongo.API.Models;
using Domain.Mongo.Statistics.Models.Entities;
using MongoDB.Driver;

namespace Dota.Statistics.ForHero.Winrate.WinrateCalculator;

public class HeroWinrateCalculatorService(
    MongoDbContext contextApi,
    Domain.Mongo.Statistics.MongoDbContext contextStatistics) : IHeroWinrateCalculatorService
{
    public async Task Calculate()
    {
        var heroResult = await contextApi.Heroes.FindAsync(FilterDefinition<Hero>.Empty);
        var matchResult = await contextApi.Matches.FindAsync(FilterDefinition<Match>.Empty);
        var heroes = heroResult.ToList();
        var matches = await matchResult.ToListAsync();

        foreach (var hero in heroes)
        {
            var heroMatches = matches
                .Where(match => match.Heroes.Any(heroInMatch => heroInMatch.Hero.Id == hero.Id))
                .Select(match => new { IsWin = match.Heroes.Find(h => h.Hero.Id == hero.Id)!.Side == match.WinnerSide })
                .ToList();

            var winrate = (float)heroMatches.Count(match => match.IsWin) / heroMatches.Count * 100;

            var heroStatistic = await contextStatistics.HeroStatistics.FindAsync(h => h.HeroId == hero.Id);
            if (!heroStatistic.Any())
                await contextStatistics.HeroStatistics.InsertOneAsync(new HeroStatistic
                {
                    HeroId = hero.Id,
                    Winrate = winrate,
                    LastUpdated = DateTimeOffset.UtcNow
                });
            else
                await contextStatistics.HeroStatistics.UpdateOneAsync(
                    h => h.HeroId == hero.Id,
                    Builders<HeroStatistic>.Update.Combine(
                        Builders<HeroStatistic>.Update.Set(h => h.Winrate, winrate),
                        Builders<HeroStatistic>.Update.Set(h => h.LastUpdated, DateTimeOffset.UtcNow)
                    ));
        }
    }
}