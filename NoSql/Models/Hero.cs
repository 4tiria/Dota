using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace NoSql.Models;

[Table("hero")]
public class Hero
{
    [BsonId]
    public int Id { get; set; }

    public string Name { get; set; }

    public string AttackType { get; set; }

    public string MainAttribute { get; set; }

    public virtual List<Tag> Tags { get; set; } = new List<Tag>();

    public virtual HeroImage Image { get; set; }
}
