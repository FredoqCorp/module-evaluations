using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunMeta value object invariants and accessors.
/// </summary>
public sealed class RunMetaTests
{
    /// <summary>
    /// Verifies that constructor rejects null form reference.
    /// </summary>
    [Fact(DisplayName = "RunMeta cannot be created with null form reference")]
    public void RunMeta_cannot_be_created_with_null_form_reference()
    {
        Should.Throw<ArgumentNullException>(() => new RunMeta(null!, "op-✓-" + Guid.NewGuid(), string.Empty), "RunMeta accepted a null form reference which is incorrect");
    }

    /// <summary>
    /// Verifies that constructor rejects null runFor value.
    /// </summary>
    [Fact(DisplayName = "RunMeta cannot be created with null runFor value")]
    public void RunMeta_cannot_be_created_with_null_runFor_value()
    {
        var form = new RunFormRef(new CascVel.Modules.Evaluations.Management.Domain.Identifiers.Uuid(), "code-✓-" + Guid.NewGuid());
        Should.Throw<ArgumentNullException>(() => new RunMeta(form, null!, string.Empty), "RunMeta accepted a null runFor which is incorrect");
    }

    /// <summary>
    /// Verifies that SupervisorComment is normalized to non-null string.
    /// </summary>
    [Fact(DisplayName = "RunMeta normalizes null supervisor comment to empty string")]
    public void RunMeta_normalizes_null_supervisor_comment_to_empty_string()
    {
        var form = new RunFormRef(new CascVel.Modules.Evaluations.Management.Domain.Identifiers.Uuid(), "code-✓-" + Guid.NewGuid());
        var vo = new RunMeta(form, "op-✓-" + Guid.NewGuid(), null);
        vo.SupervisorComment().ShouldBe(string.Empty, "RunMeta returned a null supervisor comment which is incorrect");
    }
}
