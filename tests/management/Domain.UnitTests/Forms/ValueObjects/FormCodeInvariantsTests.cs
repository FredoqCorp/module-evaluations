using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for FormCode invariants.
/// </summary>
public sealed class FormCodeInvariantsTests
{
    /// <summary>
    /// Verifies that Code throws on whitespace.
    /// </summary>
    [Fact(DisplayName = "FormCode cannot be created with a whitespace code")]
    public void FormCode_cannot_be_created_with_a_whitespace_code()
    {
        Should.Throw<ArgumentException>(() => new FormCode("   \r\n  "), "FormCode accepted a whitespace code which is incorrect");
    }
}

