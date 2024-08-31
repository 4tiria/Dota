using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.NoSql.Auth.Models.Entities;

public class RefreshToken
{
    [BsonId]
    public Guid Id { get; set; }

    public string Token { get; set; }

    [Obsolete("Планирую избавиться от хранения jti для простоты")]
    public string AccessTokenId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpireDate { get; set; }

    public bool Used { get; set; }

    public bool Invalidated { get; set; }

    public Guid AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public virtual Account Account { get; set; }
}