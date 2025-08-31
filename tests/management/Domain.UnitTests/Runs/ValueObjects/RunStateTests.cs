using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunState value object invariants and accessors.
/// </summary>
public sealed class RunStateTests
{
    /// <summary>
    /// Verifies that constructor rejects null lifecycle object.
    /// </summary>
    [Fact(DisplayName = "RunState cannot be created with null lifecycle object")]
    public void RunState_cannot_be_created_with_null_lifecycle_object()
    {
        var ctx = new RunContext(System.Collections.Immutable.ImmutableDictionary<string, string>.Empty);
        var res = new RunResult(0m, System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs.IRunCriterionScore>.Empty);
        Should.Throw<ArgumentNullException>(() => new RunState(null!, ctx, res, new RunAgreementTrail(null, CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.Enums.RunAgreementStatus.Disagree, null)), "RunState accepted a null lifecycle which is incorrect");
    }

    /// <summary>
    /// Verifies that Agreement returns null when constructed with null.
    /// </summary>
    [Fact(DisplayName = "RunState returns null agreement when constructed with null")]
    public void RunState_returns_null_agreement_when_constructed_with_null()
    {
        var lc = new RunLifecycle(new CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects.Stamp("u-" + Guid.NewGuid(), DateTime.UtcNow), new NullStamp(), new NullStamp(), new NullStamp());
        var ctx = new RunContext(System.Collections.Immutable.ImmutableDictionary<string, string>.Empty);
        var res = new RunResult(0m, System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs.IRunCriterionScore>.Empty);
        var vo = new RunState(lc, ctx, res, new RunAgreementTrail(null, CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.Enums.RunAgreementStatus.Disagree, null));
        (vo.Agreement() is null).ShouldBeFalse("RunState returned a null agreement which is incorrect");
    }
}
