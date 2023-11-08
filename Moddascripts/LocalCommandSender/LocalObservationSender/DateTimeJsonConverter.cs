using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocalObservationSender;

public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    private string _format;

    public DateTimeJsonConverter(string format)
    {
        _format = format;
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string s = reader.GetString()!;

        return DateTime.TryParse(s, out DateTime dt)
            ? dt
            : throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, DateTime dateTime, JsonSerializerOptions options)
    {
        var utc = dateTime.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
            : dateTime.ToUniversalTime();

        string isoFormat = utc.ToString(_format, CultureInfo.InvariantCulture);

        writer.WriteStringValue(isoFormat);
    }
}