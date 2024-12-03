using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Domain.Mongo.Statistics.Models.Entities;

public class HeroStatistic
{
    [Key]
    public ObjectId HeroId { get; set; }

    public float Winrate { get; set; }
    
    public DateTimeOffset LastUpdated { get; set; }
}