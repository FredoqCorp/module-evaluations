namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

/// <summary>
/// Represents a readable title for a criterion definition.
/// </summary>
public interface ICriterionTitle
{
    /// <summary>
    /// Reads the title string.
    /// </summary>
    /// <returns>Criterion title string.</returns>
    string Text();
}
