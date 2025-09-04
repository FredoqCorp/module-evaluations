using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using Interfaces;
using System.Collections.Immutable;

/// <summary>
/// Human-facing meta information as an immutable value object.
/// </summary>
public sealed record FormMeta : IFormMeta
{
    private readonly IFormName _name;
    private readonly string _description;
    private readonly IImmutableList<string> _tags;
    private readonly IFormCode _code;

    /// <summary>
    /// Creates a form meta with name, optional description, tags and code.
    /// </summary>
    public FormMeta(IFormName name, string? description, IImmutableList<string> tags, IFormCode code)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(code);

        _name = name;
        _description = description ?? string.Empty;

        _tags = tags;
        _code = code;
    }

    /// <summary>
    /// Returns the form name value object.
    /// </summary>
    public IFormName Name()
    {
        return _name;
    }

    /// <summary>
    /// Returns the optional description as a non-null string.
    /// </summary>
    public string Description()
    {
        return _description;
    }

    /// <summary>
    /// Returns the case-insensitive tags list.
    /// </summary>
    public IImmutableList<string> Tags()
    {
        return _tags;
    }

    /// <summary>
    /// Returns the globally unique form code value object.
    /// </summary>
    public IFormCode Code()
    {
        return _code;
    }
}
