using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for FormMeta value object invariants.
/// </summary>
public sealed class FormMetaTests
{
    /// <summary>
    /// Verifies that constructor rejects null tags list.
    /// </summary>
    [Fact(DisplayName = "FormMeta cannot be created with null tags list")]
    public void FormMeta_cannot_be_created_with_null_tags_list()
    {
        Should.Throw<ArgumentNullException>(() => new FormMeta(new FormName("n-" + Guid.NewGuid()), string.Empty, null!, new FormCode("c-" + Guid.NewGuid())), "FormMeta accepted a null tags list which is incorrect");
    }
}
