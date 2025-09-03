using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Policies;

/// <summary>
/// Tests for WeightedMeanPolicy calculation over a hierarchical structure.
/// </summary>
public sealed class WeightedMeanPolicyTests
{
    /// <summary>
    /// Verifies that Total returns expected weighted mean across root and nested groups.
    /// </summary>
    [Fact(DisplayName = "Weighted mean returns expected hierarchical total")]
    public void Weighted_mean_returns_expected_hierarchical_total()
    {
        var gid = new Uuid();
        var c0 = new Uuid();
        var c1 = new Uuid();
        var c2 = new Uuid();

        var meta = new FormMeta(new FormName("nm✓"), string.Empty, ImmutableList<string>.Empty, new FormCode("cd✓"));
        var audit = new AuditTrail(new Stamp("u✓", DateTime.UtcNow), new NullStamp(), new NullStamp());
        var life = new FormLifecycle(FormStatus.Published, new NoPeriod(), audit);

        var g = new FormGroup(gid, "G✓", new OrderIndex(0), ImmutableList<IFormCriterion>.Empty.Add(new FormCriterion(c1, new Criterion(new CriterionText("t1", "d1"), ImmutableList<IChoice>.Empty), new OrderIndex(0))).Add(new FormCriterion(c2, new Criterion(new CriterionText("t2", "d2"), ImmutableList<IChoice>.Empty), new OrderIndex(1))), ImmutableList<IFormGroup>.Empty);
        var rootCrit = new FormCriterion(c0, new Criterion(new CriterionText("t0", "d0"), ImmutableList<IChoice>.Empty), new OrderIndex(0));

        var weights = ImmutableDictionary.CreateBuilder<string, Weight>();
        // root siblings: group and c0
        weights[gid.Text()] = new Weight(7_000);
        weights[c0.Text()] = new Weight(3_000);
        // inside group: c1 and c2
        weights[c1.Text()] = new Weight(6_000);
        weights[c2.Text()] = new Weight(4_000);

        var def = new WeightedMeanPolicyDefinition(weights.ToImmutable());
        var form = new EvaluationForm(new Uuid(), meta, life, ImmutableList<IFormGroup>.Empty.Add(g), ImmutableList<IFormCriterion>.Empty.Add(rootCrit), def);
        var snapshot = form.Snapshot();

        // expected: groupScore = 50*0.6 + 100*0.4 = 70; total = 100*0.3 + 70*0.7 = 79
        var scores = ImmutableList.Create<Interfaces.Runs.IRunCriterionScore>(
            new RunCriterionScore(snapshot.Criteria()[0], false, new CriterionAssessment(100, "a")),
            new RunCriterionScore(snapshot.Groups()[0].Criteria()[0], false, new CriterionAssessment(50, "b")),
            new RunCriterionScore(snapshot.Groups()[0].Criteria()[1], false, new CriterionAssessment(100, "c"))
        );
        snapshot.Policy().Total(snapshot, scores).ShouldBe(79m, "Weighted mean returned an unexpected total which is incorrect");
    }
}

