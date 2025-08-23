using System.Security.Cryptography;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms;

/// <summary>
/// Tests for FormGroup methods AddChilds, AddCriteria and form attachment behavior
/// </summary>
public sealed class FormGroupTests
{
    /// <summary>
    /// Verifies that AddChilds sets Parent and propagates form attachment when set
    /// </summary>
    [Fact(DisplayName = "FormGroup adds child groups setting parent and propagating form attachment")]
    public void FormGroup_adds_child_groups_setting_parent_and_propagating_form_attachment()
    {
        var form = new EvaluationForm
        {
            Id = RandomNumberGenerator.GetInt32(1, int.MaxValue),
            Meta = new FormMeta { Name = new FormName { Value = "Καλημέρα" }, Tags = [], Code = new FormCode { Value = "Ωμέγα" } },
            Lifecycle = new FormLifecycle { Audit = new AuditTrail { Created = new Stamp { UserId = "u", At = DateTime.UtcNow } } },
        };
        var root = new FormGroup { Id = 1, Title = "t", Order = new OrderIndex { Value = 0 } };
        var child = new FormGroup { Id = 2, Title = "c", Order = new OrderIndex { Value = 1 } };
        root.AddChilds([child]);
        form.AddGroups([root]);
        ReferenceEquals(child.Form, form).ShouldBeTrue("FormGroup did not propagate form attachment to child which is incorrect");
    }

    /// <summary>
    /// Verifies that AddCriteria appends items to group collection
    /// </summary>
    [Fact(DisplayName = "FormGroup adds criteria appending them to collection")]
    public void FormGroup_adds_criteria_appending_them_to_collection()
    {
        var g = new FormGroup { Id = 1, Title = "t", Order = new OrderIndex { Value = 0 } };
        var fc = new FormCriterion { Id = 5, Criterion = new Entities.Criteria.Criterion { Id = 7, Text = new Domain.Entities.Criteria.CriterionText { Title = new Entities.Criteria.CriterionTitle { Value = "Τίτλος" }, Description = new Domain.Entities.Criteria.CriterionDescription { Value = "Περιγραφή" } }, Options = [] }, Order = new OrderIndex { Value = 0 } };
        g.AddCriteria([fc]);
        g.Criteria.Count.ShouldBe(1, "FormGroup did not append criterion which is incorrect");
    }

    /// <summary>
    /// Verifies that AddChilds fails fast on null argument
    /// </summary>
    [Fact(DisplayName = "FormGroup cannot accept null in AddChilds")]
    public void FormGroup_cannot_accept_null_in_AddChilds()
    {
        var g = new FormGroup { Id = 1, Title = "t", Order = new OrderIndex { Value = 0 } };
        Should.Throw<ArgumentNullException>(() => g.AddChilds(null!), "FormGroup accepted a null enumerable which is incorrect");
    }

    /// <summary>
    /// Verifies that AddCriteria fails fast on null argument
    /// </summary>
    [Fact(DisplayName = "FormGroup cannot accept null in AddCriteria")]
    public void FormGroup_cannot_accept_null_in_AddCriteria()
    {
        var g = new FormGroup { Id = 1, Title = "t", Order = new OrderIndex { Value = 0 } };
        Should.Throw<ArgumentNullException>(() => g.AddCriteria(null!), "FormGroup accepted a null enumerable which is incorrect");
    }
}
