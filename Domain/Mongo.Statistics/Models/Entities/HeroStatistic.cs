using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Mongo.Statistics.Models.Entities;

public class HeroStatistic
{
    [BsonId]
    public ObjectId HeroId { get; set; }

    public float Winrate { get; set; }
    
    public DateTimeOffset LastUpdated { get; set; }
}