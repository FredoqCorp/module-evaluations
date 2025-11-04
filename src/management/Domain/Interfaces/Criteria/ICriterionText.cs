namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

/// <summary>
/// Provides access to the textual body of a criterion.
/// </summary>
public interface ICriterionText
{
    /// <summary>
    /// Reads the criterion body.
    /// </summary>
    /// <returns>Criterion body string.</returns>
    string Text();
}
