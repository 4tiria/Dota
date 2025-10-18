using System.Reflection.Metadata;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Mongo.API;

public class Account
{
    [BsonId]
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public string NickName { get; set; }
    
    public string? Email { get; set; }
    
    //TODO: maybe GridFS?
    public Blob? Avatar { get; set; }
}