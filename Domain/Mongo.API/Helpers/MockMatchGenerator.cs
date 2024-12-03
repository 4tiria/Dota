using Domain.Mongo.API.Models;
using Dota.API.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Domain.Mongo.API.Helpers;

public static class MockMatchGenerator
{
    private static readonly Random _random = new();
    
    public static List<Match> CreateMatches(MongoDbContext context, int count)
    {
        var result = new List<Match>();
        
        for (var i = 0; i < count; i++)
        {
            var randomDuration = _random.Next(1000 * 60 * 15, 1000 * 60 * 60);
            var randomDateStart = DateTime.Now.AddHours(_random.NextDouble() * 8 - 4);
            var randomDateEnd = randomDateStart.AddMilliseconds(randomDuration);
            var radiantScore = _random.Next(1, 40);
            var direScore = _random.Next(1, 40);
            var winner = (_random.NextDouble() * radiantScore / direScore) > 0.5 ? "radiant" : "dire";
            var match = new Match()
            {
                Start = randomDateStart,
                End = randomDateEnd,
                Heroes = GenerateHeroesInMatch(context, radiantScore, direScore),
                Score = $"{radiantScore}-{direScore}",
                WinnerSide = winner,
            };
            
            result.Add(match);
        }

        return result;
    }
    
    private static List<HeroInMatch> GenerateHeroesInMatch(MongoDbContext context, int radiantScore, int direScore)
    {
        var heroPull = context.Heroes
            .Aggregate<Hero>(new[] 
            { 
                new BsonDocument
                {
                    { "$sample", new BsonDocument { { "size", 10 } } }
                }
            })
            .ToList();

        var radiant = heroPull.Sample(5).Shuffle();
        var dire = heroPull.Except(radiant).ToList().Shuffle();

        var radiantKills = radiantScore.SplitNumber(radiant.Count, 0);
        var radiantDeaths = direScore.SplitNumber(dire.Count, 0);
        var direKills = direScore.SplitNumber(dire.Count, 0);
        var direDeaths = radiantScore.SplitNumber(radiant.Count, 0);

        var radiantTeam = GenerateHeroStatistics(context,"radiant", radiant, radiantKills, radiantDeaths, radiantScore);
        var direTeam = GenerateHeroStatistics(context,"dire", dire, direKills, direDeaths, direScore);

        return [.. radiantTeam, .. direTeam];
    }

    private static List<HeroInMatch> GenerateHeroStatistics(MongoDbContext context, string side, List<Hero> heroes, List<int> kills, List<int> deaths, int score)
    {
        var result = new List<HeroInMatch>();
        for (var i = 0; i < heroes.Count; i++)
        {
            var currentHero = new HeroInMatch()
            {
                Hero = heroes[i],
                Kills = kills[i],
                Deaths = deaths[i],
                Assists = _random.Next(score / 2, score),
                Gold = (int)((_random.NextDouble() * 10000 + 5000) * Math.Sqrt(kills[i] / (double)score)),
                XP = (int)((_random.NextDouble() * 8000 + 7000) * Math.Sqrt(kills[i] / (double)score)),
                Side = side,
            };

            result.Add(currentHero);
        }

        return result;
    }
}