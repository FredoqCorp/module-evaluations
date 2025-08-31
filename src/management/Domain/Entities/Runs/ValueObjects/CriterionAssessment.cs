using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Criterion assessment: selected score and optional comment as an immutable value object.
/// Absence of an instance for a criterion means the criterion is skipped.
/// </summary>
public sealed record CriterionAssessment : ICriterionAssessment
{
    private readonly ushort _selectedScore;
    private readonly string _comment;

    /// <summary>
    /// Creates a criterion assessment with selected score and optional comment.
    /// </summary>
    public CriterionAssessment(ushort selectedScore, string? comment)
    {
        _selectedScore = selectedScore;
        _comment = comment ?? string.Empty;
    }

    /// <summary>
    /// Returns the selected score value.
    /// </summary>
    public ushort SelectedScore() => _selectedScore;

    /// <summary>
    /// Returns the comment as a non-null string.
    /// </summary>
    public string Comment() => _comment;
}
