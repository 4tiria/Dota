using Domain.Mongo.API.Converters;
using Domain.Mongo.API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Mongo.API;

public class Hero
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string LocalizedName { get; set; }

    public string UnderscoreName { get; set; }

    [BsonSerializer(typeof(EnumToStringSerializer<AttackType>))]
    public AttackType AttackType { get; set; }

    [BsonSerializer(typeof(EnumToStringSerializer<MainAttribute>))]
    public MainAttribute MainAttribute { get; set; }

    public string ImageLink { get; set; }

    public List<string> Roles { get; set; } = [];
}