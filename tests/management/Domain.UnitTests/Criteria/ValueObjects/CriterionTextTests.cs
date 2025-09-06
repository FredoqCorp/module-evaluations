using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Criteria.ValueObjects;

/// <summary>
/// Unit tests for the CriterionText value object ensuring title and description behavior.
/// </summary>
public sealed class CriterionTextTests
{
    /// <summary>
    /// Ensures title returned equals the provided value.
    /// </summary>
    [Fact]
    public void It_returns_the_same_title_value()
    {
        var title = "титул-✓-标题-🚀-" + Guid.NewGuid();
        var description = "описание-✓-説明-🧪-" + Guid.NewGuid();
        var text = new CriterionText(title, description);

        text.Title().ShouldBe(title, "title value returned is not equal to input");
    }

    /// <summary>
    /// Ensures description returned equals the provided value.
    /// </summary>
    [Fact]
    public void It_returns_the_same_description_value()
    {
        var title = "заголовок-✓-見出し-🛰️-" + Guid.NewGuid();
        var description = "описание-✓-説明-🧫-" + Guid.NewGuid();
        var text = new CriterionText(title, description);

        text.Description().ShouldBe(description, "description value returned is not equal to input");
    }

    /// <summary>
    /// Ensures title accessor fails fast when created with null.
    /// </summary>
    [Fact(DisplayName = "CriterionText cannot be created with null title")]
    public void CriterionText_cannot_be_created_with_null_title()
    {
        var description = "описание-✓-説明-🧪-" + Guid.NewGuid();

        Should.Throw<ArgumentNullException>(() => new CriterionText(null!, description), "CriterionText accepted a null title which is incorrect");
    }

    /// <summary>
    /// Ensures title accessor fails fast when created with whitespace.
    /// </summary>
    [Fact(DisplayName = "CriterionText cannot return title when created with whitespace")]
    public void CriterionText_cannot_return_title_when_created_with_whitespace()
    {
        var description = "описание-✓-説明-🧪-" + Guid.NewGuid();
        var text = new CriterionText("  \t\n  ", description);

        Should.Throw<InvalidDataException>(() => text.Title(), "CriterionText accepted an empty title which is incorrect");
    }

    /// <summary>
    /// Ensures description accessor fails fast when created with null.
    /// </summary>
    [Fact(DisplayName = "CriterionText cannot be created with null description")]
    public void CriterionText_cannot_be_created_with_null_description()
    {
        var title = "титул-✓-标题-🚀-" + Guid.NewGuid();

        Should.Throw<ArgumentNullException>(() => new CriterionText(title, null!), "CriterionText accepted a null description which is incorrect");
    }
}
