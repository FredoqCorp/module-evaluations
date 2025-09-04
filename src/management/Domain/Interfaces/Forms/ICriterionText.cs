namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Contract for a textual content of a criterion represented by title and description.
/// </summary>
public interface ICriterionText
{
    /// <summary>
    /// Returns the human readable title string.
    /// </summary>
    string Title();

    /// <summary>
    /// Returns the detailed description string.
    /// </summary>
    string Description();
}

