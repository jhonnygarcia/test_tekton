using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi.AppStart
{
    public class DateTimeToUTCConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (value.Kind != DateTimeKind.Utc || value.Kind != DateTimeKind.Unspecified)
                writer.WriteStringValue(value.ToUniversalTime().ToString("o"));
            else
                writer.WriteStringValue(value.ToString("o"));
        }
    }
}
