using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms;

/// <summary>
/// Tests for EvaluationForm aggregate interface API.
/// </summary>
public sealed class EvaluationFormTests
{
    /// <summary>
    /// Verifies that Groups method returns the same count as provided.
    /// </summary>
    [Fact(DisplayName = "EvaluationForm returns the same groups count")]
    public void EvaluationForm_returns_the_same_groups_count()
    {
        var id = new Uuid();
        var meta = new FormMeta(new FormName("n-✓-" + Guid.NewGuid()), string.Empty, ImmutableList<string>.Empty, new FormCode("c-✓-" + Guid.NewGuid()));
        var audit = new AuditTrail(new Stamp("u-✓-" + Guid.NewGuid(), DateTime.UtcNow), new NullStamp(), new NullStamp());
        var life = new FormLifecycle(FormStatus.Draft, new NoPeriod(), audit);
        var groups = ImmutableList<IFormGroup>.Empty;
        var criteria = ImmutableList<IFormCriterion>.Empty;
        var agg = new EvaluationForm(id, meta, life, FormCalculationKind.ArithmeticMean, groups, criteria);

        ((IEvaluationForm)agg).Groups().Count.ShouldBe(0, "EvaluationForm returned an unexpected groups count which is incorrect");
    }
}
