using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable composition of score, label, and annotation for use inside rating scales.
/// </summary>
public sealed record RatingOption : IRatingOption
{
    private readonly RatingScore score;
    private readonly RatingLabel label;
    private readonly RatingAnnotation annotation;

    /// <summary>
    /// Creates a rating option from the provided components without accepting null.
    /// </summary>
    /// <param name="scoreValue">Numeric score value object.</param>
    /// <param name="labelValue">Label value object.</param>
    /// <param name="annotationValue">Annotation value object.</param>
    public RatingOption(RatingScore scoreValue, RatingLabel labelValue, RatingAnnotation annotationValue)
    {
        score = scoreValue;
        label = labelValue;
        annotation = annotationValue;
    }

    /// <summary>
    /// Returns the numeric score associated with this option.
    /// </summary>
    /// <returns>Value object describing the numeric score.</returns>
    public RatingScore Score()
    {
        return score;
    }

    /// <summary>
    /// Returns the label configured by the designer.
    /// </summary>
    /// <returns>Value object with human readable label.</returns>
    public RatingLabel Label()
    {
        return label;
    }

    /// <summary>
    /// Returns the optional annotation attached to this option.
    /// </summary>
    /// <returns>Value object describing the annotation.</returns>
    public RatingAnnotation Annotation()
    {
        return annotation;
    }
}
