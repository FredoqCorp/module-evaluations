using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;
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
        var gid = new FormGroupId(Guid.NewGuid());
        var c0 = new FormCriterionId(Guid.NewGuid());
        var c1 = new FormCriterionId(Guid.NewGuid());
        var c2 = new FormCriterionId(Guid.NewGuid());

        var meta = new FormMeta(new FormName("nm✓"), string.Empty, ImmutableList<string>.Empty, new FormCode("cd✓"));
        var tail = new FormAuditTail(FormAuditKind.Published, new Stamp("u✓", DateTime.UtcNow));
        var life = new FormLifecycle(new Period(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), tail);

        var g = new FormGroup(gid, "G✓", new OrderIndex(0), new FormCriteriaList(ImmutableList<FormCriterion>.Empty.Add(new FormCriterion(c1, new Criterion(new CriterionText("t1", "d1"), ImmutableList<Choice>.Empty), new OrderIndex(0))).Add(new FormCriterion(c2, new Criterion(new CriterionText("t2", "d2"), ImmutableList<Choice>.Empty), new OrderIndex(1)))), new FormGroupList(ImmutableList<FormGroup>.Empty));
        var rootCrit = new FormCriterion(c0, new Criterion(new CriterionText("t0", "d0"), ImmutableList<Choice>.Empty), new OrderIndex(0));

        var weights = ImmutableDictionary.CreateBuilder<Guid, Weight>();
        // root siblings: group and c0
        weights[gid.Value] = new Weight(7_000);
        weights[c0.Value] = new Weight(3_000);
        // inside group: c1 and c2
        weights[c1.Value] = new Weight(6_000);
        weights[c2.Value] = new Weight(4_000);

        var def = new WeightedMeanPolicyDefinition(weights.ToImmutable());
        var form = new EvaluationForm(new EvaluationFormId(Guid.CreateVersion7()), meta, life, new FormGroupList([g]), new FormCriteriaList(ImmutableList<FormCriterion>.Empty.Add(rootCrit)), def);
        var snapshot = form.Snapshot();

        // expected: groupScore = 50*0.6 + 100*0.4 = 70; total = 100*0.3 + 70*0.7 = 79
        var snapshotGroup = snapshot.Groups().Group(snapshot.Groups().Ids()[0].Value);
        var scores = ImmutableList.Create<Interfaces.Runs.IRunCriterionScore>(
            new RunCriterionScore(snapshot.Criteria().Ids()[0], false, new CriterionAssessment(100, "a")),
            new RunCriterionScore(snapshotGroup.Criteria.Ids()[0], false, new CriterionAssessment(50, "b")),
            new RunCriterionScore(snapshotGroup.Criteria.Ids()[1], false, new CriterionAssessment(100, "c"))
        );
        snapshot.Policy().Total(snapshot, scores).ShouldBe(79m, "Weighted mean returned an unexpected total which is incorrect");
    }
}
