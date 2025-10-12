using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Host.Infrastructure;

/// <summary>
/// Implements media abstraction by writing to a UTF-8 JSON writer.
/// </summary>
internal sealed class JsonMediaWriter : IMedia
{
    private readonly Utf8JsonWriter _writer;

    /// <summary>
    /// Initializes the media with the provided JSON writer.
    /// </summary>
    /// <param name="writer">Underlying UTF-8 JSON writer for output.</param>
    public JsonMediaWriter(Utf8JsonWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        _writer = writer;
    }

    /// <summary>
    /// Writes a string value associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">String value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    public IMedia WriteString(string key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);

        _writer.WriteString(key, value);
        return this;
    }

    /// <summary>
    /// Writes a GUID value associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">GUID value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    public IMedia WriteGuid(string key, Guid value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        _writer.WriteString(key, value);
        return this;
    }

    /// <summary>
    /// Writes a 32-bit integer value associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">Integer value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    public IMedia WriteInt32(string key, int value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        _writer.WriteNumber(key, value);
        return this;
    }

    /// <summary>
    /// Writes an array of string values associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="values">Collection of string values to write as an array.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    public IMedia WriteStringArray(string key, IEnumerable<string> values)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(values);

        _writer.WriteStartArray(key);
        foreach (var value in values)
        {
            _writer.WriteStringValue(value);
        }
        _writer.WriteEndArray();

        return this;
    }
}
