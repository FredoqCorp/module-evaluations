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
/// Tests for ArithmeticMeanPolicy calculation.
/// </summary>
public sealed class ArithmeticMeanPolicyTests
{
    /// <summary>
    /// Verifies that Total returns arithmetic mean of present criterion scores.
    /// </summary>
    [Fact(DisplayName = "Arithmetic mean returns expected total")]
    public void Arithmetic_mean_returns_expected_total()
    {
        var id = new EvaluationFormId(Guid.CreateVersion7());
        var meta = new FormMeta(new FormName("nm✓"), string.Empty, ImmutableList<string>.Empty, new FormCode("cd✓"));
        var audit = new AuditTrail(new Stamp("u✓", DateTime.UtcNow), new Stamp("s✓", DateTime.UtcNow), new Stamp("c✓", DateTime.UtcNow));
        var life = new FormLifecycle(FormStatus.Published, new NoPeriod(), audit);

        var c1 = new FormCriterion(new FormCriterionId(Guid.NewGuid()), new Criterion(new CriterionText("t1✓", "d1"), ImmutableList<IChoice>.Empty), new OrderIndex(0));
        var c2 = new FormCriterion(new FormCriterionId(Guid.NewGuid()), new Criterion(new CriterionText("t2✓", "d2"), ImmutableList<IChoice>.Empty), new OrderIndex(1));
        var form = new EvaluationForm(id, meta, life, ImmutableList<IFormGroup>.Empty, ImmutableList<IFormCriterion>.Empty.Add(c1).Add(c2), new ArithmeticMeanPolicyDefinition());

        var snapshot = form.Snapshot();
        var s = ImmutableList.Create<Interfaces.Runs.IRunCriterionScore>(
            new RunCriterionScore(snapshot.Criteria()[0], false, new CriterionAssessment(10, "a")),
            new RunCriterionScore(snapshot.Criteria()[1], false, new CriterionAssessment(30, "b"))
        );
        snapshot.Policy().Total(snapshot, s).ShouldBe(20m, "Arithmetic mean returned an unexpected total which is incorrect");
    }
}
