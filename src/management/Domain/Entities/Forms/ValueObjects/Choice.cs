namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

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
    public ushort Score() => _score;

    /// <summary>
    /// Returns the caption string or an empty string when absent.
    /// </summary>
    public string Caption() => _caption;

    /// <summary>
    /// Returns the annotation string or an empty string when absent.
    /// </summary>
    public string Annotation() => _annotation;
}
