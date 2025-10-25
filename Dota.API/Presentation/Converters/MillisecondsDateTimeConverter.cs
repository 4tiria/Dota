using Newtonsoft.Json;

namespace Dota.API.Presentation.Converters;

public class MillisecondsDateTimeConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) =>
        objectType == typeof(DateTime) || objectType == typeof(DateTime?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        if (reader.TokenType == JsonToken.Integer)
        {
            var ms = Convert.ToInt64(reader.Value);
            return DateTimeOffset.FromUnixTimeMilliseconds(ms).LocalDateTime;
        }
        if (reader.TokenType == JsonToken.String && long.TryParse((string?)reader.Value, out var v))
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(v).LocalDateTime;
        }
        return serializer.Deserialize(reader, typeof(DateTime));
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null) { writer.WriteNull(); return; }
        var dt = (DateTime)value;
        var ms = new DateTimeOffset(dt.ToUniversalTime()).ToUnixTimeMilliseconds();
        writer.WriteValue(ms);
    }
}