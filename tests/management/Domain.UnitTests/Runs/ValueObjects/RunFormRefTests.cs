using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunFormRef value object invariants and accessors.
/// </summary>
public sealed class RunFormRefTests
{
    /// <summary>
    /// Verifies that constructor rejects null code.
    /// </summary>
    [Fact(DisplayName = "RunFormRef cannot be created with null form code")]
    public void RunFormRef_cannot_be_created_with_null_form_code()
    {
        Should.Throw<ArgumentNullException>(() => new RunFormRef(new Uuid(), null!), "RunFormRef accepted a null code which is incorrect");
    }

    /// <summary>
    /// Verifies that FormId returns the same identifier.
    /// </summary>
    [Fact(DisplayName = "RunFormRef returns the same form identifier")]
    public void RunFormRef_returns_the_same_form_identifier()
    {
        var id = new Uuid();
        var vo = new RunFormRef(id, "code-✓-" + Guid.NewGuid());
        vo.FormId().ShouldBe(id, "RunFormRef returned an unexpected form identifier which is incorrect");
    }
}

