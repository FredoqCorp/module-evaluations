using CascVel.Modules.Evaluations.Management.Domain.Entities.Agreements;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Agreements.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs;

/// <summary>
/// Tests for RunAgreementDecision invariants.
/// </summary>
public sealed class RunAgreementDecisionTests
{
    /// <summary>
    /// Verifies that Comment is normalized to non-null string.
    /// </summary>
    [Fact(DisplayName = "RunAgreementDecision normalizes null comment to empty string")]
    public void RunAgreementDecision_normalizes_null_comment_to_empty_string()
    {
        var now = DateTime.UtcNow;
        var vo = new RunAgreementDecision(new AgreementId(Guid.CreateVersion7()), new RunId(Guid.CreateVersion7()), RunAgreementStatus.Agree, now, null);
        vo.Comment().ShouldBe(string.Empty, "RunAgreementDecision returned a null comment which is incorrect");
    }
}
