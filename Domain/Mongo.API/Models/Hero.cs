using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Mongo.API.Models;

public class Hero
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string Name { get; set; }

    public string AttackType { get; set; }

    public string MainAttribute { get; set; }

    public byte[] Image { get; set; }

    public List<string> Tags { get; set; } = [];
}
