using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for the FormAuditTail transitions and guards.
/// </summary>
public sealed class FormAuditTailTests
{
    /// <summary>
    /// Verifies that an edit is allowed after creation.
    /// </summary>
    [Fact(DisplayName = "Form audit tail allows edit after creation")]
    public void Form_audit_tail_allows_edit_after_creation()
    {
        var created = new FormAuditTail(FormAuditKind.Created, new Stamp("u-✓-" + Guid.NewGuid(), DateTime.UtcNow));
        var next = created.Accept(FormAuditKind.Edited, new Stamp("e-✓-" + Guid.NewGuid(), DateTime.UtcNow.AddSeconds(1)));
        next.Kind().ShouldBe(FormAuditKind.Edited, "Form audit tail produced an unexpected kind which is incorrect");
    }

    /// <summary>
    /// Verifies that publish is denied after archive.
    /// </summary>
    [Fact(DisplayName = "Form audit tail denies publish after archive")]
    public void Form_audit_tail_denies_publish_after_archive()
    {
        var edited = new FormAuditTail(FormAuditKind.Edited, new Stamp("u-✓-" + Guid.NewGuid(), DateTime.UtcNow));
        var archived = edited.Accept(FormAuditKind.Archived, new Stamp("a-✓-" + Guid.NewGuid(), DateTime.UtcNow.AddSeconds(1)));
        Should.Throw<InvalidOperationException>(() => archived.Accept(FormAuditKind.Published, new Stamp("p-✓-" + Guid.NewGuid(), DateTime.UtcNow.AddSeconds(2))), "Form audit tail accepted publish after archive which is incorrect");
    }

    /// <summary>
    /// Verifies that time cannot go backwards.
    /// </summary>
    [Fact(DisplayName = "Form audit tail denies timestamp regression")]
    public void Form_audit_tail_denies_timestamp_regression()
    {
        var created = new FormAuditTail(FormAuditKind.Created, new Stamp("u-✓-" + Guid.NewGuid(), DateTime.UtcNow));
        Should.Throw<InvalidOperationException>(() => created.Accept(FormAuditKind.Edited, new Stamp("e-✓-" + Guid.NewGuid(), created.Stamp().At.AddSeconds(-1))), "Form audit tail accepted a timestamp regression which is incorrect");
    }
}

