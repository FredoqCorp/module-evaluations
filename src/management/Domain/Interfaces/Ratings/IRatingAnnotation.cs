namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

/// <summary>
/// Provides access to the explanatory annotation of a rating option.
/// </summary>
public interface IRatingAnnotation
{
    /// <summary>
    /// Reads the annotation text.
    /// </summary>
    /// <returns>Rating annotation string.</returns>
    string Text();
}
