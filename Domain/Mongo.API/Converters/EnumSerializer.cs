using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Domain.Mongo.API.Converters;

public class EnumToStringSerializer<TEnum> : SerializerBase<TEnum> where TEnum : Enum
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEnum value)
    {
        context.Writer.WriteString(value.ToString());
    }

    public override TEnum Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadString();
        return (TEnum)Enum.Parse(typeof(TEnum), value);
    }
}