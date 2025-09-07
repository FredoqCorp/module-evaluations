using System.Collections.Immutable;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Human-facing meta information as an immutable value object.
/// </summary>
public sealed record FormMeta
{
    /// <summary>
    /// Creates a form meta with name, optional description, tags and code.
    /// </summary>
    public FormMeta(FormName name, string? description, IImmutableList<string> tags, FormCode code)
    {
        ArgumentNullException.ThrowIfNull(tags);

        Name = name;
        Description = description ?? string.Empty;
        Tags = tags;
        Code = code;
    }

    /// <summary>
    /// Returns the form name value object.
    /// </summary>
    public FormName Name { get; }

    /// <summary>
    /// Returns the description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Returns the case-insensitive tags list.
    /// </summary>
    public IImmutableList<string> Tags { get; }

    /// <summary>
    /// Returns the globally unique form code value object.
    /// </summary>
    public FormCode Code { get; }
}
