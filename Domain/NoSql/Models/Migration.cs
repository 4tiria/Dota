using MongoDB.Bson.Serialization.Attributes;

namespace NoSql.Models;

public class Migration
{
    [BsonId]
    public int Id { get; set; }
    
    public int Version { get; set; }
    
    public DateTime AppliedOn { get; set; }
    
    public string Description { get; set; }
}