using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunFormCriterion value object accessors.
/// </summary>
public sealed class RunFormCriterionTests
{
    /// <summary>
    /// Verifies that Id returns the same identifier.
    /// </summary>
    [Fact(DisplayName = "RunFormCriterion returns the same identifier")]
    public void RunFormCriterion_returns_the_same_identifier()
    {
        var id = new Uuid();
        var baseCriterion = new FormCriterion(
            id,
            new Criterion(
                new CriterionText("t✓", "d"),
                System.Collections.Immutable.ImmutableList<Interfaces.IChoice>.Empty
            ),
            new OrderIndex(0)
        );
        var rc = new RunFormCriterion(id, baseCriterion);
        rc.Id().ShouldBe(id, "RunFormCriterion returned an unexpected identifier which is incorrect");
    }
}

