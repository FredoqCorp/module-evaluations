namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

/// <summary>
/// Provides access to the textual label assigned to a rating option.
/// </summary>
public interface IRatingLabel
{
    /// <summary>
    /// Reads the label text.
    /// </summary>
    /// <returns>Rating label string.</returns>
    string Text();
}
