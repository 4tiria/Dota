using DataAccess;
using MongoDB.Driver;
using NoSql;
using NoSql.Models;

namespace MySqlToMongoMigrator;

public class MatchTableHandler(MongoDbContext mongoDbContext, ApiContext postgreContext)
{
    public void Execute()
    {
        mongoDbContext.Matches.DeleteMany(match => true);

        var matches = postgreContext.Matches.AsEnumerable().ToList();
        foreach (var match in matches)
        {
            if (mongoDbContext.Matches.Find(m => m.End == match.End && m.Score == match.Score) != null)
            {
                mongoDbContext.Matches.DeleteOne(m => m.End == match.End && m.Score == match.Score);
            }

            mongoDbContext.Matches.InsertOne(new Match()
            {
                Start = match.Start,
                End = match.End,
                Score = match.Score,
                WinnerSide = match.WinnerSide,
                Heroes = match.Heroes.Select(hero => new HeroInMatch()
                {
                    Assists = hero.Assists,
                    Deaths = hero.Deaths,
                    Kills = hero.Kills,
                    Gold = hero.Gold,
                    XP = hero.XP,
                    Hero = new()
                    {
                        Id = hero.HeroId,
                        AttackType = hero.Hero.AttackType,
                        Image = hero.Hero.Image.Bytes,
                        MainAttribute = hero.Hero.MainAttribute,
                        Name = hero.Hero.Name,
                        Tags = hero.Hero.Tags.Select(x => x.Name).ToList(),
                    },
                    Side = hero.Side,
                }).ToList(),
            });
        }
    }
}
