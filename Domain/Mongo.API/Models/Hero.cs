using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Mongo.API;

public class Hero
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string LocalizedName { get; set; }

    public string UnderscoreName { get; set; }
    public string AttackType { get; set; }

    public string MainAttribute { get; set; }

    public string ImageLink { get; set; }

    public List<string> Roles { get; set; } = [];
}