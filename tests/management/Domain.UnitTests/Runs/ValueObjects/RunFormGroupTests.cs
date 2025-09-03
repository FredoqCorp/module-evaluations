using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunFormGroup value object accessors.
/// </summary>
public sealed class RunFormGroupTests
{
    /// <summary>
    /// Verifies that Id returns the same identifier.
    /// </summary>
    [Fact(DisplayName = "RunFormGroup returns the same identifier")]
    public void RunFormGroup_returns_the_same_identifier()
    {
        var id = new Uuid();
        var rg = new RunFormGroup(id, "g✓", new OrderIndex(0), ImmutableList<Interfaces.Runs.IRunFormCriterion>.Empty, ImmutableList<Interfaces.Runs.IRunFormGroup>.Empty);
        rg.Id().ShouldBe(id, "RunFormGroup returned an unexpected identifier which is incorrect");
    }
}

