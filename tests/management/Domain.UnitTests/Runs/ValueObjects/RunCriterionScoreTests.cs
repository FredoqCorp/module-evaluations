using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
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
        var baseCriterion = new CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects.FormCriterion(
            new Uuid(),
            new CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects.Criterion(
                new CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects.CriterionText("тест✓", "описание✓"),
                System.Collections.Immutable.ImmutableList<IChoice>.Empty
            ),
            new CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects.OrderIndex(0)
        );
        var rc = new RunFormCriterion(new Uuid(), baseCriterion);
        var vo = new RunCriterionScore(rc, true, null);
        vo.Skipped().ShouldBeTrue("RunCriterionScore returned a false skipped value which is incorrect");
    }
}
