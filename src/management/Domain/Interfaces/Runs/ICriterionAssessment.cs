namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for a criterion assessment data.
/// </summary>
public interface ICriterionAssessment
{
    /// <summary>
    /// Returns whether the assessment contains a selected score.
    /// </summary>
    bool Present();

    /// <summary>
    /// Returns the selected score value.
    /// </summary>
    ushort SelectedScore();

    /// <summary>
    /// Returns the comment as a non-null string.
    /// </summary>
    string Comment();
}
