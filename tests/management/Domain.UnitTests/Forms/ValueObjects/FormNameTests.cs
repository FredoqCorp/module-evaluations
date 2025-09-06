using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for FormName value object null handling and accessors.
/// </summary>
public sealed class FormNameTests
{
    /// <summary>
    /// Verifies that constructor rejects null value.
    /// </summary>
    [Fact(DisplayName = "FormName cannot be created with null value")]
    public void FormName_cannot_be_created_with_null_value()
    {
        Should.Throw<ArgumentNullException>(() => new FormName(null!), "FormName accepted a null value which is incorrect");
    }

    /// <summary>
    /// Verifies that Name returns the same string.
    /// </summary>
    [Fact(DisplayName = "FormName returns the same string value")]
    public void FormName_returns_the_same_string_value()
    {
        var v = "имя-✓-名前-" + Guid.NewGuid();
        var vo = new FormName(v);
        vo.Name().ShouldBe(v, "FormName returned an unexpected string which is incorrect");
    }
}

