using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
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
        var gid = new Uuid();
        var cid = new Uuid();
        var id = new EvaluationFormId(Guid.CreateVersion7());
        var meta = new FormMeta(new FormName("n✓"), string.Empty, ImmutableList<string>.Empty, new FormCode("c✓"));
        var audit = new AuditTrail(new Stamp("u✓", DateTime.UtcNow), new NullStamp(), new NullStamp());
        var life = new FormLifecycle(FormStatus.Published, new NoPeriod(), audit);
        var group = new FormGroup(gid, "G✓", new OrderIndex(0), ImmutableList<IFormCriterion>.Empty, ImmutableList<IFormGroup>.Empty);
        var crit = new FormCriterion(cid, new Criterion(new CriterionText("T✓", "D"), ImmutableList<IChoice>.Empty), new OrderIndex(1));
        var form = new EvaluationForm(id, meta, life, ImmutableList<IFormGroup>.Empty.Add(group), ImmutableList<IFormCriterion>.Empty.Add(crit), new ArithmeticMeanPolicyDefinition());

        var map = ImmutableDictionary.CreateBuilder<string, Weight>();
        map[gid.Text()] = new Weight(6_000);
        map[cid.Text()] = new Weight(3_000); // sums to 9_000, not 10_000
        var def = new WeightedMeanPolicyDefinition(map.ToImmutable());

        Should.Throw<InvalidDataException>(() => def.Verify(form), "Weighted definition accepted incorrect weights sum which is incorrect");
    }
}
