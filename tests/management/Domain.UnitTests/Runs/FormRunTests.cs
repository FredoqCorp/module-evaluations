using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;
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
        var id = new RunId(Guid.CreateVersion7());
        var formId = new EvaluationFormId(Guid.NewGuid());
        var formCode = "code-✓-" + Guid.NewGuid();
        var formMeta = new FormMeta(new FormName("name✓"), "desc✓", System.Collections.Immutable.ImmutableList<string>.Empty, new FormCode(formCode));
        var snapshot = new RunFormSnapshot(formId, formMeta, new CascVel.Modules.Evaluations.Management.Domain.Entities.Policies.ArithmeticMeanPolicy(),
            System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms.IFormGroup>.Empty,
            System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms.IFormCriterion>.Empty);
        var meta = new RunMeta(snapshot, "op-✓-" + Guid.NewGuid(), null);
        var lc = new RunLifecycle(new Stamp("u-✓-" + Guid.NewGuid(), DateTime.UtcNow), new Stamp("s-✓-" + Guid.NewGuid(), DateTime.UtcNow), new Stamp("c-✓-" + Guid.NewGuid(), DateTime.UtcNow), new Stamp("p-✓-" + Guid.NewGuid(), DateTime.UtcNow));
        var ctx = new RunContext(System.Collections.Immutable.ImmutableDictionary<string, string>.Empty);
        var res = new RunResult(0m, System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs.IRunCriterionScore>.Empty);
        var state = new RunState(lc, ctx, res);
        var agg = new FormRun(id, meta, state);

        agg.Id().ShouldBe(id, "FormRun returned an unexpected identifier which is incorrect");
    }
}
