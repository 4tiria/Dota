using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NoSql.Models;

public class Match
{
    [BsonId]
    public ObjectId Id { get; set; }

    public virtual List<HeroInMatch> Heroes { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string Score { get; set; }

    public string WinnerSide { get; set; }

}
