using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that aggregates form metadata fields.
/// </summary>
public sealed record FormMetadata
{
    private readonly FormName _name;
    private readonly Option<FormDescription> _description;
    private readonly FormCode _code;
    private readonly ITags _tags;

    /// <summary>
    /// Initializes the metadata aggregate with all required components.
    /// </summary>
    /// <param name="name">Form name value object.</param>
    /// <param name="description">Optional form description.</param>
    /// <param name="code">Form code value object.</param>
    /// <param name="tags">Tags associated with the form.</param>
    public FormMetadata(FormName name, Option<FormDescription> description, FormCode code, ITags tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        _name = name;
        _description = description;
        _code = code;
        _tags = tags;
    }

    /// <summary>
    /// Returns the form name.
    /// </summary>
    /// <returns>Form name value object.</returns>
    public FormName Name()
    {
        return _name;
    }

    /// <summary>
    /// Returns the optional form description.
    /// </summary>
    /// <returns>Optional form description value object.</returns>
    public Option<FormDescription> Description()
    {
        return _description;
    }

    /// <summary>
    /// Returns the form code.
    /// </summary>
    /// <returns>Form code value object.</returns>
    public FormCode Code()
    {
        return _code;
    }

    /// <summary>
    /// Returns the tags associated with the form.
    /// </summary>
    /// <returns>Tags value object.</returns>
    public ITags Tags()
    {
        return _tags;
    }
}

