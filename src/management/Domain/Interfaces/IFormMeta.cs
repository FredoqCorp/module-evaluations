namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

using System.Collections.Immutable;

/// <summary>
/// Contract for human-facing metadata of a form.
/// </summary>
public interface IFormMeta
{
    /// <summary>
    /// Returns the form name value object.
    /// </summary>
    IFormName Name();

    /// <summary>
    /// Returns the optional description as a non-null string.
    /// </summary>
    string Description();

    /// <summary>
    /// Returns the case-insensitive tags list.
    /// </summary>
    IImmutableList<string> Tags();

    /// <summary>
    /// Returns the globally unique form code value object.
    /// </summary>
    IFormCode Code();
}
