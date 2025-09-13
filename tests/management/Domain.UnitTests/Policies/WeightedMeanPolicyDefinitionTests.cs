using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Policies;

/// <summary>
/// Tests for WeightedMeanPolicyDefinition invariants.
/// </summary>
public sealed class WeightedMeanPolicyDefinitionTests
{
    /// <summary>
    /// Verifies that Verify throws when weights sum is not 100 percent for siblings.
    /// </summary>
    [Fact(DisplayName = "Weighted definition throws on incorrect weights sum for siblings")]
    public void Weighted_definition_throws_on_incorrect_weights_sum_for_siblings()
    {
        var gid = new FormGroupId(Guid.NewGuid());
        var cid = new FormCriterionId(Guid.NewGuid());
        var id = new EvaluationFormId(Guid.CreateVersion7());
        var meta = new FormMeta(new FormName("n✓"), string.Empty, ImmutableList<string>.Empty, new FormCode("c✓"));
        var tail = new FormAuditTail(FormAuditKind.Published, new Stamp("u✓", DateTime.UtcNow));
        var life = new FormLifecycle(new Period(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), tail);
        var group = new FormGroup(gid, "G✓", new OrderIndex(0), new FormCriteriaList(ImmutableList<FormCriterion>.Empty), new FormGroupList(ImmutableList<FormGroup>.Empty));
        var crit = new FormCriterion(cid, new Criterion(new CriterionText("T✓", "D"), ImmutableList<Choice>.Empty), new OrderIndex(1));
        var form = new EvaluationForm(id, meta, life, new FormGroupList(ImmutableList<FormGroup>.Empty.Add(group)), new FormCriteriaList(ImmutableList<FormCriterion>.Empty.Add(crit)), new ArithmeticMeanPolicyDefinition());

        var map = ImmutableDictionary.CreateBuilder<Guid, Weight>();
        map[gid.Value] = new Weight(6_000);
        map[cid.Value] = new Weight(3_000); // sums to 9_000, not 10_000
        var def = new WeightedMeanPolicyDefinition(map.ToImmutable());

        Should.Throw<InvalidDataException>(() => def.Verify(form), "Weighted definition accepted incorrect weights sum which is incorrect");
    }
}
