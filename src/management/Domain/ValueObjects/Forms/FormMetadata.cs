using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Immutable value object that aggregates form metadata fields.
/// </summary>
public sealed record FormMetadata
{
    private readonly FormName _name;
    private readonly FormDescription _description;
    private readonly FormCode _code;
    private readonly ITags _tags;

    /// <summary>
    /// Initializes the metadata aggregate with all required components.
    /// </summary>
    /// <param name="name">Form name value object.</param>
    /// <param name="description">Optional form description.</param>
    /// <param name="code">Form code value object.</param>
    /// <param name="tags">Tags associated with the form.</param>
    public FormMetadata(FormName name, FormDescription description, FormCode code, ITags tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        _name = name;
        _description = description;
        _code = code;
        _tags = tags;
    }

    /// <summary>
    /// Prints the form metadata fields into the provided media.
    /// </summary>
    /// <param name="media">Target media that receives the printed representation.</param>
    public void Print(IMedia media)
    {
        ArgumentNullException.ThrowIfNull(media);

        media
            .WriteString("name", _name.Value)
            .WriteString("description", _description.Value)
            .WriteString("code", _code.Token);

        _tags.Print(media, "tags");
    }
}

