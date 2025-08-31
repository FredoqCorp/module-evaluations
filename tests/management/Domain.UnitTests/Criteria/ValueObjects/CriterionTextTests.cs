using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
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
}

