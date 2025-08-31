using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs;

/// <summary>
/// Tests for FormRun aggregate interface API.
/// </summary>
public sealed class FormRunTests
{
    /// <summary>
    /// Verifies that Id method returns the same identifier.
    /// </summary>
    [Fact(DisplayName = "FormRun returns the same identifier")]
    public void FormRun_returns_the_same_identifier()
    {
        var id = new Uuid();
        var formRef = new RunFormRef(new Uuid(), "code-✓-" + Guid.NewGuid());
        var meta = new RunMeta(formRef, "op-✓-" + Guid.NewGuid(), null);
        var lc = new RunLifecycle(new Stamp("u-✓-" + Guid.NewGuid(), DateTime.UtcNow), new NullStamp(), new NullStamp(), new NullStamp());
        var ctx = new RunContext(System.Collections.Immutable.ImmutableDictionary<string, string>.Empty);
        var res = new RunResult(0m, System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs.IRunCriterionScore>.Empty);
        var state = new RunState(lc, ctx, res, new RunAgreementTrail(null, CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.Enums.RunAgreementStatus.Disagree, null));
        var agg = new FormRun(id, meta, state);

        ((IFormRun)agg).Id().ShouldBe(id, "FormRun returned an unexpected identifier which is incorrect");
    }
}
