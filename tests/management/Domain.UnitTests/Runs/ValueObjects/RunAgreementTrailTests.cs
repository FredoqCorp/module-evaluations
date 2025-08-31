using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunAgreementTrail value object accessors.
/// </summary>
public sealed class RunAgreementTrailTests
{
    /// <summary>
    /// Verifies that Status returns the provided value.
    /// </summary>
    [Fact(DisplayName = "RunAgreementTrail returns the same status value")]
    public void RunAgreementTrail_returns_the_same_status_value()
    {
        var vo = new RunAgreementTrail(null, RunAgreementStatus.Agree, null);
        vo.Status().ShouldBe(RunAgreementStatus.Agree, "RunAgreementTrail returned an unexpected status which is incorrect");
    }
}
