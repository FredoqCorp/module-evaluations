using System.Text.Json;
using System.Text.Json.Serialization;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;

namespace CascVel.Modules.Evaluations.Management.Host.Infrastructure;

/// <summary>
/// JSON converter that serializes form aggregates through the printer pattern.
/// </summary>
internal sealed class FormJsonConverter : JsonConverter<IForm>
{
    /// <summary>
    /// Throws because deserialization of form aggregates is not supported.
    /// </summary>
    /// <param name="reader">Reader providing JSON data.</param>
    /// <param name="typeToConvert">Type to convert.</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>Nothing because the method always fails.</returns>
    /// <exception cref="NotSupportedException">Always thrown to prevent deserialization.</exception>
    public override IForm Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Deserialization of IForm is not supported");
    }

    /// <summary>
    /// Writes a form aggregate to JSON using the printer pattern.
    /// </summary>
    /// <param name="writer">Writer receiving JSON output.</param>
    /// <param name="value">Form aggregate to serialize.</param>
    /// <param name="options">Serializer options.</param>
    public override void Write(Utf8JsonWriter writer, IForm value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        using var media = new JsonMediaWriter(writer);
        value.Print(media);
    }
}
