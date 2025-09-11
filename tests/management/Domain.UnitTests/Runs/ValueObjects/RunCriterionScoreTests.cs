using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunCriterionScore value object accessors.
/// </summary>
public sealed class RunCriterionScoreTests
{
    /// <summary>
    /// Verifies that Skipped returns the provided value.
    /// </summary>
    [Fact(DisplayName = "RunCriterionScore returns the same skipped flag value")]
    public void RunCriterionScore_returns_the_same_skipped_flag_value()
    {
        
        var rc = new FormCriterion(
            new FormCriterionId(Guid.NewGuid()),
            new Criterion(
                new CriterionText("тест✓", "описание✓"),
                System.Collections.Immutable.ImmutableList<Choice>.Empty
            ),
            new OrderIndex(0)
        );
        var vo = new RunCriterionScore(rc, true, new NoAssessment());
        vo.Skipped().ShouldBeTrue("RunCriterionScore returned a false skipped value which is incorrect");
    }
}
