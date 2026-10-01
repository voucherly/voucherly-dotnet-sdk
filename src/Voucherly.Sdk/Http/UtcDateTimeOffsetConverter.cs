using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Voucherly.Sdk.Http;

/// <summary>
/// Reads a date-time without an offset as UTC, which is what the API means by it.
/// The default converter takes the local offset instead, which shifts the instant and fails on 0001-01-01T00:00:00 east of Greenwich.
/// </summary>
internal sealed class UtcDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString();
        if (DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var value))
        {
            return value;
        }

        throw new JsonException($"'{text}' is not a date-time.");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) => writer.WriteStringValue(value);
}
