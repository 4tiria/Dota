using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Mongo.API;

public class Account
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string Nickname { get; set; }
}