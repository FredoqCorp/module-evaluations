using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunResult value object invariants and accessors.
/// </summary>
public sealed class RunResultTests
{
    /// <summary>
    /// Verifies that constructor rejects null criteria list.
    /// </summary>
    [Fact(DisplayName = "RunResult cannot be created with null criteria list")]
    public void RunResult_cannot_be_created_with_null_criteria_list()
    {
        Should.Throw<ArgumentNullException>(() => new RunResult(0m, null!), "RunResult accepted a null criteria list which is incorrect");
    }

    /// <summary>
    /// Verifies that Criteria returns the same count as provided.
    /// </summary>
    [Fact(DisplayName = "RunResult returns the same criteria count")]
    public void RunResult_returns_the_same_criteria_count()
    {
        var builder = ImmutableList.CreateBuilder<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs.IRunCriterionScore>();
        var runCriterion = new FormCriterion(
            new Identifiers.FormCriterionId(),
            new Criterion(
                new CriterionText("тест✓", "описание✓"),
                ImmutableList<IChoice>.Empty
            ),
            new OrderIndex(0)
        );
        builder.Add(new RunCriterionScore(runCriterion, false, new NoAssessment()));
        var vo = new RunResult(0m, builder.ToImmutable());
        vo.Criteria().Count.ShouldBe(1, "RunResult returned an unexpected criteria count which is incorrect");
    }
}
