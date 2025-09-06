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
    [Fact(DisplayName = "FormCode cannot return a whitespace code")]
    public void FormCode_cannot_return_a_whitespace_code()
    {
        var vo = new FormCode("   \r\n  ");
        Should.Throw<InvalidDataException>(() => vo.Code(), "FormCode accepted a whitespace code which is incorrect");
    }
}

