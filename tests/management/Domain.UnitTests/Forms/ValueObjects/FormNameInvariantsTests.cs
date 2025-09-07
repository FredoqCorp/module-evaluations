using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for FormName invariants.
/// </summary>
public sealed class FormNameInvariantsTests
{
    /// <summary>
    /// Verifies that Name throws on whitespace.
    /// </summary>
    [Fact(DisplayName = "FormName cannot be created with a whitespace name")]
    public void FormName_cannot_be_created_with_a_whitespace_name()
    {
        Should.Throw<ArgumentException>(() => new FormName("   \t\n  "), "FormName accepted a whitespace name which is incorrect");
    }
}

