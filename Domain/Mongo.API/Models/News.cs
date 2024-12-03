using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace Domain.Mongo.API.Models;

public class News
{
    [BsonId]
    public ObjectId Id { get; set; }
    public long Time { get; set; }
    public string Name { get; set; }
}
