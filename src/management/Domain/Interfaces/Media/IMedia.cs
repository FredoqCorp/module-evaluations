using CascVel.Modules.Evaluations.Management.Domain.Common;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

/// <summary>
/// Behavioral contract for a media that receives structured data via fluent API.
/// </summary>
public interface IMedia
{
    /// <summary>
    /// Writes a string value associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">String value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    IMedia WriteString(string key, string value);

    /// <summary>
    /// Writes an optional string value associated with the specified key.
    /// If the option contains no value, the key is omitted from the output.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">Optional string value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    IMedia WriteOptionalString(string key, Option<string> value);

    /// <summary>
    /// Writes a GUID value associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">GUID value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    IMedia WriteGuid(string key, Guid value);

    /// <summary>
    /// Writes a 32-bit integer value associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="value">Integer value to write.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    IMedia WriteInt32(string key, int value);

    /// <summary>
    /// Writes an array of string values associated with the specified key.
    /// </summary>
    /// <param name="key">Property name or key.</param>
    /// <param name="values">Collection of string values to write as an array.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    IMedia WriteStringArray(string key, IEnumerable<string> values);

    /// <summary>
    /// Starts writing a nested object with the specified key.
    /// Must be followed by property writes and then EndObject().
    /// </summary>
    /// <returns>This media instance for fluent chaining.</returns>
    IMedia StartObject();

    /// <summary>
    /// Starts writing a nested object with the specified key.
    /// Must be followed by property writes and then EndObject().
    /// </summary>
    /// <param name="key">Property name or key for the object.</param>
    /// <returns>This media instance for fluent chaining.</returns>
    IMedia StartObject(string key);

    /// <summary>
    /// Ends writing the current nested object.
    /// Must be called after StartObject() and all property writes.
    /// </summary>
    /// <returns>This media instance for fluent chaining.</returns>
    IMedia EndObject();
}

/// <summary>
/// Behavioral contract for a media that receives structured data via fluent API and produces output.
/// </summary>
/// <typeparam name="TOutput">The type of output this media produces.</typeparam>
public interface IMedia<out TOutput> : IMedia
{
    /// <summary>
    /// Finalizes the media and returns the complete output.
    /// </summary>
    /// <returns>The final output representation.</returns>
    TOutput Output();
}
