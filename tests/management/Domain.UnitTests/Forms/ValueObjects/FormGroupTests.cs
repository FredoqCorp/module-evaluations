using Shouldly;
using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for FormGroup value object invariants.
/// </summary>
public sealed class FormGroupTests
{
    /// <summary>
    /// Verifies that constructor rejects null title.
    /// </summary>
    [Fact(DisplayName = "FormGroup cannot be created with null title")]
    public void FormGroup_cannot_be_created_with_null_title()
    {
        Should.Throw<ArgumentNullException>(() => new FormGroup(new FormGroupId(Guid.NewGuid()), null!, new OrderIndex(0), ImmutableList<IFormCriterion>.Empty, ImmutableList<IFormGroup>.Empty), "FormGroup accepted a null title which is incorrect");
    }

    
}
