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
    [Fact(DisplayName = "FormName cannot return a whitespace name")]
    public void FormName_cannot_return_a_whitespace_name()
    {
        var vo = new FormName("   \t\n  ");
        Should.Throw<InvalidDataException>(() => vo.Name(), "FormName accepted a whitespace name which is incorrect");
    }
}

