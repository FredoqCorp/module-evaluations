using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunMeta value object invariants and accessors.
/// </summary>
public sealed class RunMetaTests
{
    /// <summary>
    /// Verifies that constructor rejects null snapshot.
    /// </summary>
    [Fact(DisplayName = "RunMeta cannot be created with null snapshot")]
    public void RunMeta_cannot_be_created_with_null_snapshot()
    {
        Should.Throw<ArgumentNullException>(() => new RunMeta(null!, "op-✓-" + Guid.NewGuid(), string.Empty), "RunMeta accepted a null snapshot which is incorrect");
    }

    /// <summary>
    /// Verifies that constructor rejects null runFor value.
    /// </summary>
    [Fact(DisplayName = "RunMeta cannot be created with null runFor value")]
    public void RunMeta_cannot_be_created_with_null_runFor_value()
    {
        var formId = new Identifiers.EvaluationFormId();
        var formCode = "code-✓-" + Guid.NewGuid();
        var meta = new FormMeta(new FormName("name✓"), "desc✓", System.Collections.Immutable.ImmutableList<string>.Empty, new FormCode(formCode));
        var snapshot = new RunFormSnapshot(formId, meta, new CascVel.Modules.Evaluations.Management.Domain.Entities.Policies.ArithmeticMeanPolicy(),
            System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms.IFormGroup>.Empty,
            System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms.IFormCriterion>.Empty);
        Should.Throw<ArgumentNullException>(() => new RunMeta(snapshot, null!, string.Empty), "RunMeta accepted a null runFor which is incorrect");
    }

    /// <summary>
    /// Verifies that SupervisorComment is normalized to non-null string.
    /// </summary>
    [Fact(DisplayName = "RunMeta normalizes null supervisor comment to empty string")]
    public void RunMeta_normalizes_null_supervisor_comment_to_empty_string()
    {
        var formId = new Identifiers.EvaluationFormId();
        var formCode = "code-✓-" + Guid.NewGuid();
        var meta = new FormMeta(new FormName("name✓"), "desc✓", System.Collections.Immutable.ImmutableList<string>.Empty, new FormCode(formCode));
        var snapshot = new RunFormSnapshot(formId, meta, new CascVel.Modules.Evaluations.Management.Domain.Entities.Policies.ArithmeticMeanPolicy(),
            System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms.IFormGroup>.Empty,
            System.Collections.Immutable.ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms.IFormCriterion>.Empty);
        var vo = new RunMeta(snapshot, "op-✓-" + Guid.NewGuid(), null);
        vo.SupervisorComment().ShouldBe(string.Empty, "RunMeta returned a null supervisor comment which is incorrect");
    }
}
