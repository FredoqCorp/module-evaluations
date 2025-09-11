namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// A selectable option carrying a raw discrete score used in evaluation without any range validation.
/// </summary>
public sealed record Choice
{
    /// <summary>
    /// Creates a choice with the provided score and encapsulated caption and annotation.
    /// </summary>
    public Choice(ushort score, string caption, string annotation)
    {
        ArgumentNullException.ThrowIfNull(caption);
        ArgumentNullException.ThrowIfNull(annotation);

        Score = score;
        Caption = caption;
        Annotation = annotation;
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
    public ushort Score { get; }

    /// <summary>
    /// Returns the caption string.
    /// </summary>
    public string Caption { get; }

    /// <summary>
    /// Returns the annotation string.
    /// </summary>
    public string Annotation { get; }

}
