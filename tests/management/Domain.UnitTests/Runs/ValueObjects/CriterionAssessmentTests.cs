using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for CriterionAssessment value object invariants and accessors.
/// </summary>
public sealed class CriterionAssessmentTests
{
    /// <summary>
    /// Verifies that SelectedScore returns the provided value.
    /// </summary>
    [Fact(DisplayName = "CriterionAssessment returns the same selected score value")]
    public void CriterionAssessment_returns_the_same_selected_score_value()
    {
        var score = (ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1);
        var vo = new CriterionAssessment(score, string.Empty);
        vo.SelectedScore().ShouldBe(score, "CriterionAssessment returned an unexpected selected score which is incorrect");
    }

    /// <summary>
    /// Verifies that Comment is normalized to non-null string.
    /// </summary>
    [Fact(DisplayName = "CriterionAssessment normalizes null comment to empty string")]
    public void CriterionAssessment_normalizes_null_comment_to_empty_string()
    {
        var score = (ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1);
        var vo = new CriterionAssessment(score, null);
        vo.Comment().ShouldBe(string.Empty, "CriterionAssessment returned a null comment which is incorrect");
    }
}
