using System.Collections.Immutable;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Contract for a domain criterion exposing identity, text parts and available options.
/// </summary>
public interface ICriterion
{
    /// <summary>
    /// Returns the human readable title string.
    /// </summary>
    string Title();

    /// <summary>
    /// Returns the detailed description string.
    /// </summary>
    string Description();

    /// <summary>
    /// Returns the list of available options for scoring.
    /// </summary>
    IImmutableList<IChoice> Options();
}
