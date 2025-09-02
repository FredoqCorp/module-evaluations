using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for FormCriterion value object invariants.
/// </summary>
public sealed class FormCriterionTests
{
    /// <summary>
    /// Verifies that constructor rejects null criterion object.
    /// </summary>
    [Fact(DisplayName = "FormCriterion cannot be created with null criterion object")]
    public void FormCriterion_cannot_be_created_with_null_criterion_object()
    {
        Should.Throw<ArgumentNullException>(() => new FormCriterion(null!, new OrderIndex(0)), "FormCriterion accepted a null criterion which is incorrect");
    }
}
