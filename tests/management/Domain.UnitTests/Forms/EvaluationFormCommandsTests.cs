using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms;

/// <summary>
/// Tests for EvaluationForm domain commands behavior.
/// </summary>
public sealed class EvaluationFormCommandsTests
{
    /// <summary>
    /// Verifies that Publish changes status and sets tail to published.
    /// </summary>
    [Fact(DisplayName = "EvaluationForm publish sets status to published")]
    public void EvaluationForm_publish_sets_status_to_published()
    {
        var now = DateTime.UtcNow;
        var id = new EvaluationFormId(Guid.CreateVersion7());
        var meta = new FormMeta(new FormName("n-✓-" + Guid.NewGuid()), string.Empty, ImmutableList<string>.Empty, new FormCode("c-✓-" + Guid.NewGuid()));
        var life = new FormLifecycle(FormStatus.Draft, new Period(now.AddDays(-1), now.AddDays(1)), new FormAuditTail(FormAuditKind.Edited, new Stamp("e-✓-" + Guid.NewGuid(), now.AddMinutes(-1))));
        var form = new EvaluationForm(id, meta, life, ImmutableList<IFormGroup>.Empty, ImmutableList<IFormCriterion>.Empty, new ArithmeticMeanPolicyDefinition());

        var next = form.Publish(new Stamp("p-✓-" + Guid.NewGuid(), now));
        next.Lifecycle().Status.ShouldBe(FormStatus.Published, "EvaluationForm did not set published status which is incorrect");
    }

    /// <summary>
    /// Verifies that Edit after publish is denied.
    /// </summary>
    [Fact(DisplayName = "EvaluationForm edit is denied after publish")]
    public void EvaluationForm_edit_is_denied_after_publish()
    {
        var now = DateTime.UtcNow;
        var id = new EvaluationFormId(Guid.CreateVersion7());
        var meta = new FormMeta(new FormName("n-✓-" + Guid.NewGuid()), string.Empty, ImmutableList<string>.Empty, new FormCode("c-✓-" + Guid.NewGuid()));
        var life = new FormLifecycle(FormStatus.Published, new Period(now.AddDays(-1), now.AddDays(1)), new FormAuditTail(FormAuditKind.Published, new Stamp("p-✓-" + Guid.NewGuid(), now.AddMinutes(-1))));
        var form = new EvaluationForm(id, meta, life, ImmutableList<IFormGroup>.Empty, ImmutableList<IFormCriterion>.Empty, new ArithmeticMeanPolicyDefinition());

        Should.Throw<InvalidOperationException>(
            () => form.Edit(meta, ImmutableList<IFormGroup>.Empty, ImmutableList<IFormCriterion>.Empty, new ArithmeticMeanPolicyDefinition(), new Stamp("e2-✓-" + Guid.NewGuid(), now)),
            "EvaluationForm accepted edit after publish which is incorrect");
    }

    /// <summary>
    /// Verifies that Archive moves form to archived status.
    /// </summary>
    [Fact(DisplayName = "EvaluationForm archive sets status to archived")]
    public void EvaluationForm_archive_sets_status_to_archived()
    {
        var now = DateTime.UtcNow;
        var id = new EvaluationFormId(Guid.CreateVersion7());
        var meta = new FormMeta(new FormName("n-✓-" + Guid.NewGuid()), string.Empty, ImmutableList<string>.Empty, new FormCode("c-✓-" + Guid.NewGuid()));
        var life = new FormLifecycle(FormStatus.Draft, new Period(now.AddDays(-1), now.AddDays(1)), new FormAuditTail(FormAuditKind.Edited, new Stamp("e-✓-" + Guid.NewGuid(), now.AddMinutes(-1))));
        var form = new EvaluationForm(id, meta, life, ImmutableList<IFormGroup>.Empty, ImmutableList<IFormCriterion>.Empty, new ArithmeticMeanPolicyDefinition());

        var next = form.Archive(new Stamp("a-✓-" + Guid.NewGuid(), now));
        next.Lifecycle().Status.ShouldBe(FormStatus.Archived, "EvaluationForm did not set archived status which is incorrect");
    }
}
