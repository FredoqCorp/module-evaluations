namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using Interfaces;

/// <summary>
/// A selectable option carrying a raw discrete score used in evaluation without any range validation.
/// </summary>
public sealed record Choice : IChoice
{
    private readonly ushort _score;
    private readonly string _caption;
    private readonly string _annotation;

    /// <summary>
    /// Creates a choice with the provided score and encapsulated caption and annotation.
    /// </summary>
    public Choice(ushort score, string caption, string annotation)
    {
        ArgumentNullException.ThrowIfNull(caption);
        ArgumentNullException.ThrowIfNull(annotation);

        _score = score;
        _caption = caption;
        _annotation = annotation;
    }

    /// <summary>
    /// Creates a choice with the provided score and no caption or annotation.
    /// </summary>
    public Choice(ushort score) : this(score, string.Empty, string.Empty)
    {
    }

    /// <summary>
    /// Returns the score value associated with this choice.
    /// </summary>
    public ushort Score()
    {
        return _score;
    }

    /// <summary>
    /// Returns the caption string.
    /// </summary>
    public string Caption()
    {
        return _caption;
    }

    /// <summary>
    /// Returns the annotation string.
    /// </summary>
    public string Annotation()
    {
        return _annotation;
    }
}
