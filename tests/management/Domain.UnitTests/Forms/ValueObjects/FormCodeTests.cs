using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for FormCode value object null handling and accessors.
/// </summary>
public sealed class FormCodeTests
{
    /// <summary>
    /// Verifies that constructor rejects null value.
    /// </summary>
    [Fact(DisplayName = "FormCode cannot be created with null value")]
    public void FormCode_cannot_be_created_with_null_value()
    {
        Should.Throw<ArgumentNullException>(() => new FormCode(null!), "FormCode accepted a null value which is incorrect");
    }

    /// <summary>
    /// Verifies that Code returns the same string.
    /// </summary>
    [Fact(DisplayName = "FormCode returns the same string value")]
    public void FormCode_returns_the_same_string_value()
    {
        var v = "код-✓-コード-" + Guid.NewGuid();
        var vo = new FormCode(v);
        vo.Value.ShouldBe(v, "FormCode returned an unexpected string which is incorrect");
    }
}

