using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Media;

/// <summary>
/// Implements media abstraction by writing to a UTF-8 JSON writer.
/// Supports two modes: external writer (for streaming) or internal writer (for Output()).
/// </summary>
public sealed class JsonMediaWriter : IMedia<string>, IDisposable
{
    private readonly Utf8JsonWriter _writer;
    private readonly MemoryStream? _memoryStream;
    private readonly bool _ownsWriter;

    /// <summary>
    /// Initializes the media with the provided JSON writer.
    /// Use this constructor when writing to an external stream (e.g., HTTP response).
    /// </summary>
    /// <param name="writer">Underlying UTF-8 JSON writer for output.</param>
    public JsonMediaWriter(Utf8JsonWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        _writer = writer;
        _memoryStream = null;
        _ownsWriter = false;

        _writer.WriteStartObject();
    }

    /// <summary>
    /// Initializes the media with its own internal writer for JSON object generation.
    /// Use this constructor when you need to call Output() to get the JSON string.
    /// </summary>
    public JsonMediaWriter()
    {
        _memoryStream = new MemoryStream();
        _writer = new Utf8JsonWriter(_memoryStream, new JsonWriterOptions { Indented = false });
        _ownsWriter = true;

        _writer.WriteStartObject();
    }

    /// <summary>
    /// Writes a string value associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">String value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    public IMedia With(string key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);

        _writer.WriteString(key, value);
        return this;
    }

    /// <summary>
    /// Writes an optional string value associated with the specified key.
    /// If the option contains no value, the key is omitted from the output.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">Optional string value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    public IMedia With(string key, Option<string> value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        if (value.IsSome)
        {
            value.Map(v =>
            {
                _writer.WriteString(key, v);
                return v;
            });
        }

        return this;
    }

    /// <summary>
    /// Writes a GUID value associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">GUID value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    public IMedia With(string key, Guid value)
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
    public IMedia With(string key, int value)
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
    public IMedia With(string key, IEnumerable<string> values)
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

    /// <summary>
    /// Starts writing a nested object with the specified key.
    /// </summary>
    /// <param name="key">Property name or key for the object.</param>
    /// <param name="configure">Configuration action for the nested object.</param>
    public IMedia WithObject(string key, Action<IMedia> configure)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(configure);

        _writer.WriteStartObject(key);
        configure(this);
        _writer.WriteEndObject();
        return this;
    }

    /// <summary>
    /// Ends writing the current nested object.
    /// </summary>
    public IMedia EndObject()
    {
        _writer.WriteEndObject();
        return this;
    }

    /// <summary>
    /// Finalizes the JSON writing and returns the complete JSON string.
    /// Only supported when using the parameterless constructor.
    /// </summary>
    /// <returns>The complete JSON string.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when this method is called on an instance created with an external writer.
    /// </exception>
    public string Output()
    {
        if (!_ownsWriter || _memoryStream is null)
        {
            throw new InvalidOperationException(
                "Output() is only supported when using the parameterless constructor. " +
                "This instance was created with an external writer.");
        }

        _writer.WriteEndObject();
        _writer.Flush();
        return System.Text.Encoding.UTF8.GetString(_memoryStream.ToArray());
    }

    /// <summary>
    /// Disposes the internal writer and stream if owned by this instance.
    /// </summary>
    public void Dispose()
    {
        if (_ownsWriter)
        {
            _writer?.Dispose();
            _memoryStream?.Dispose();
        }
        else
        {
            _writer?.WriteEndObject();
        }
    }
}
