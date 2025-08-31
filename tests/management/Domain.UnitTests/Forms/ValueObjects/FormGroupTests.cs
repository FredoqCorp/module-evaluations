using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using Shouldly;
using System.Collections.Immutable;

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
        Should.Throw<ArgumentNullException>(() => new FormGroup(null!, new OrderIndex(0), new ZeroWeight(), ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.IFormCriterion>.Empty, ImmutableList<CascVel.Modules.Evaluations.Management.Domain.Interfaces.IFormGroup>.Empty), "FormGroup accepted a null title which is incorrect");
    }

    
}
