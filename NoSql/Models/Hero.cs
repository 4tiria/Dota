using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NoSql.Enums;

namespace NoSql.Models;

public class Hero
{
    [BsonId]
    public int Id { get; set; }

    public string Name { get; set; }

    public string AttackType { get; set; }

    public string MainAttribute { get; set; }

    public List<string> Tags { get; set; } = [];

    public byte[] Image { get; set; }
}
