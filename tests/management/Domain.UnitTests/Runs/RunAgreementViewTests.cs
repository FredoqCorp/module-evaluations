using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs;

/// <summary>
/// Tests for RunAgreementView invariants.
/// </summary>
public sealed class RunAgreementViewTests
{
    /// <summary>
    /// Verifies that ViewedAt returns the provided time.
    /// </summary>
    [Fact(DisplayName = "RunAgreementView returns the same viewed time")]
    public void RunAgreementView_returns_the_same_viewed_time()
    {
        var now = DateTime.UtcNow;
        var vo = new RunAgreementView(new AgreementId(Guid.CreateVersion7()), new RunId(Guid.CreateVersion7()), now);
        vo.ViewedAt().ShouldBe(now, "RunAgreementView returned an unexpected viewed time which is incorrect");
    }
}
