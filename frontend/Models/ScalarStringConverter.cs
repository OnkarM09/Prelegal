using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prelegal.Frontend.Models;

/// <summary>
/// Reads a JSON scalar (string, number, or boolean) as a string. The template
/// schema allows "default" to be any scalar (e.g. term_months = 24), so this
/// normalizes them all to the string form used for placeholder substitution.
/// </summary>
public sealed class ScalarStringConverter : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Number => reader.TryGetInt64(out var l)
                ? l.ToString(System.Globalization.CultureInfo.InvariantCulture)
                : reader.GetDouble().ToString(System.Globalization.CultureInfo.InvariantCulture),
            JsonTokenType.True => "true",
            JsonTokenType.False => "false",
            _ => reader.GetString()
        };
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        => writer.WriteStringValue(value);
}
