using System.Text.Json;
using System.Text.Json.Serialization;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Host.Infrastructure;

/// <summary>
/// JSON converter that serializes form summaries using the printer pattern.
/// </summary>
internal sealed class FormSummaryJsonConverter : JsonConverter<IFormSummary>
{
    /// <summary>
    /// Reads and converts JSON to a form summary instance.
    /// </summary>
    /// <param name="reader">Reader providing JSON data.</param>
    /// <param name="typeToConvert">Type of object to convert.</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>Deserialized form summary instance.</returns>
    /// <exception cref="NotSupportedException">Deserialization is not supported for this converter.</exception>
    public override IFormSummary Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Deserialization of IFormSummary is not supported");
    }

    /// <summary>
    /// Writes a form summary instance to JSON using the printer pattern.
    /// </summary>
    /// <param name="writer">Writer receiving JSON output.</param>
    /// <param name="value">Form summary instance to serialize.</param>
    /// <param name="options">Serializer options.</param>
    public override void Write(Utf8JsonWriter writer, IFormSummary value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        var media = new JsonMediaWriter(writer);
        value.Print(media);

        writer.WriteEndObject();
    }
}
