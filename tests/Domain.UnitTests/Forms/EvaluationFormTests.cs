namespace CascVel.Module.Evaluations.Management.Domain.UnitTests.Forms;

using System.Globalization;
using System.Security.Cryptography;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Entities.Criteria;
using Shouldly;

/// <summary>
/// Tests for EvaluationForm aggregate methods AddGroups and AddCriteria
/// </summary>
public sealed class EvaluationFormTests
{
    private static string Id() => RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue).ToString("X", CultureInfo.InvariantCulture);

    /// <summary>
    /// Verifies that AddGroups attaches groups to the form and sets parent references transitively
    /// </summary>
    [Fact(DisplayName = "EvaluationForm attaches groups to the form and propagates attachment to nested groups")]
    public void EvaluationForm_attaches_groups_to_the_form_and_propagates_attachment_to_nested_groups()
    {
        var form = new EvaluationForm
        {
            Id = RandomNumberGenerator.GetInt32(1, int.MaxValue),
            Meta = new FormMeta { Name = new FormName { Value = Id() }, Tags = [], Code = new FormCode { Value = Id() } },
            Lifecycle = new FormLifecycle { Audit = new AuditTrail { Created = new Stamp { UserId = Id(), At = DateTime.UtcNow } } },
        };
        var child = new FormGroup { Id = 2, Title = Id(), Order = new OrderIndex { Value = 1 } };
        var root = new FormGroup { Id = 1, Title = Id(), Order = new OrderIndex { Value = 0 } };
        root.AddChilds([child]);
        form.AddGroups([root]);
        ReferenceEquals(child.Form, form).ShouldBeTrue("EvaluationForm did not attach nested groups which is incorrect");
    }

    /// <summary>
    /// Verifies that AddCriteria adds items to the form collection in order
    /// </summary>
    [Fact(DisplayName = "EvaluationForm adds criteria preserving insertion order")]
    public void EvaluationForm_adds_criteria_preserving_insertion_order()
    {
        var form = new EvaluationForm
        {
            Id = RandomNumberGenerator.GetInt32(1, int.MaxValue),
            Meta = new FormMeta { Name = new FormName { Value = Id() }, Tags = [], Code = new FormCode { Value = Id() } },
            Lifecycle = new FormLifecycle { Audit = new AuditTrail { Created = new Stamp { UserId = Id(), At = DateTime.UtcNow } } },
        };
        var text = new CriterionText { Title = new CriterionTitle { Value = Id() }, Description = new CriterionDescription { Value = Id() } };
        var c1 = new Criterion { Id = 1, Text = text, Options = [], Automation = null };
        var c2 = new Criterion { Id = 2, Text = text, Options = [], Automation = null };
        var f1 = new FormCriterion { Id = 11, Criterion = c1, Order = new OrderIndex { Value = 0 } };
        var f2 = new FormCriterion { Id = 12, Criterion = c2, Order = new OrderIndex { Value = 1 } };
        form.AddCriteria([f1, f2]);
        string seq = string.Join(',', form.Criteria.Select(x => x.Id));
        seq.ShouldBe("11,12", "EvaluationForm did not preserve criteria order which is incorrect");
    }
}
