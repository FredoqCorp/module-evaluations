namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

/// <summary>
/// Provides access to the numeric score assigned to a rating option.
/// </summary>
public interface IRatingScore
{
    /// <summary>
    /// Reads the numeric score.
    /// </summary>
    /// <returns>Rating score value.</returns>
    int Value();
}
