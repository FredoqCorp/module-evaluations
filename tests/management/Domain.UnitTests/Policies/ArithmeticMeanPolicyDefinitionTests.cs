using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Policies;

/// <summary>
/// Tests for ArithmeticMeanPolicyDefinition invariants.
/// </summary>
public sealed class ArithmeticMeanPolicyDefinitionTests
{
    /// <summary>
    /// Verifies that Verify does not throw for any form structure.
    /// </summary>
    [Fact(DisplayName = "Arithmetic mean definition does not throw on verify")]
    public void Arithmetic_mean_definition_does_not_throw_on_verify()
    {
        var id = new EvaluationFormId(Guid.CreateVersion7());
        var meta = new FormMeta(new FormName("name✓"), string.Empty, ImmutableList<string>.Empty, new FormCode("code✓"));
        var audit = new AuditTrail(new Stamp("user✓", DateTime.UtcNow), new NullStamp(), new NullStamp());
        var life = new FormLifecycle(FormStatus.Draft, new NoPeriod(), audit);
        var form = new EvaluationForm(id, meta, life, ImmutableList<IFormGroup>.Empty, ImmutableList<IFormCriterion>.Empty, new ArithmeticMeanPolicyDefinition());

        Should.NotThrow(() => new ArithmeticMeanPolicyDefinition().Verify(form), "Arithmetic mean definition threw unexpectedly which is incorrect");
    }
}
