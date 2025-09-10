using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for deriving audit tail from form lifecycle.
/// </summary>
public sealed class FormLifecycleTailTests
{
    /// <summary>
    /// Verifies that Tail returns published when status is Published.
    /// </summary>
    [Fact(DisplayName = "Form lifecycle tail is published when status is published")]
    public void Form_lifecycle_tail_is_published_when_status_is_published()
    {
        var now = DateTime.UtcNow;
        var life = new FormLifecycle(new Period(now.AddDays(-1), now.AddDays(1)), new FormAuditTail(FormAuditKind.Published, new Stamp("p-✓-" + Guid.NewGuid(), now)));
        life.Tail.Kind().ShouldBe(FormAuditKind.Published, "Form lifecycle tail returned an unexpected kind which is incorrect");
    }

    /// <summary>
    /// Verifies that Tail returns archived when status is Archived.
    /// </summary>
    [Fact(DisplayName = "Form lifecycle tail is archived when status is archived")]
    public void Form_lifecycle_tail_is_archived_when_status_is_archived()
    {
        var now = DateTime.UtcNow;
        var life = new FormLifecycle(new Period(now.AddDays(-1), now.AddDays(1)), new FormAuditTail(FormAuditKind.Archived, new Stamp("a-✓-" + Guid.NewGuid(), now.AddMinutes(-1))));
        life.Tail.Kind().ShouldBe(FormAuditKind.Archived, "Form lifecycle tail returned an unexpected kind which is incorrect");
    }
}
