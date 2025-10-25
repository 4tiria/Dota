using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dota.API.Presentation.Converters;

public class NullableDateTimeEpochConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        if (reader.TokenType == JsonTokenType.Number)
        {
            var ms = reader.GetInt64();
            return DateTime.UnixEpoch.AddMilliseconds(ms).ToLocalTime();
        }
        if (reader.TokenType == JsonTokenType.String && long.TryParse(reader.GetString(), out var v))
        {
            return DateTime.UnixEpoch.AddMilliseconds(v).ToLocalTime();
        }
        return reader.GetDateTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null) { writer.WriteNullValue(); return; }
        var utc = value.Value.ToUniversalTime();
        var ms = (long)(utc - DateTime.UnixEpoch).TotalMilliseconds;
        writer.WriteNumberValue(ms);
    }
}