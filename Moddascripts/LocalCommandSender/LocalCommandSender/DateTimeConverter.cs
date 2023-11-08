using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocalCommandSender;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private static readonly string[] _knownFormats =
    {
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ssK",
        "yyyy-MM-ddTHH:mm:ss.fK",
        "yyyy-MM-ddTHH:mm:ss.ffK",
        "yyyy-MM-ddTHH:mm:ss.fffK",
        "yyyy-MM-ddTHH:mm:ss.ffffK",
        "yyyy-MM-ddTHH:mm:ss.fffffK",
        "yyyy-MM-ddTHH:mm:ss.ffffffK",
        "yyyy-MM-ddTHH:mm:ss.ffffffKK",
        "yyyy-MM-ddTHH:mm:ss.fffffffK",
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string s = reader.GetString()!;

        foreach (string format in _knownFormats)
        {
            if (DateTime.TryParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime dateTime))
            {
                return dateTime;
            }
        }

        // Hail mary parse
        return DateTime.TryParse(s, out DateTime dt)
            ? dt
            : throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        string isoFormat = value.ToUtc().ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ", CultureInfo.InvariantCulture);

        writer.WriteStringValue(isoFormat);
    }
}