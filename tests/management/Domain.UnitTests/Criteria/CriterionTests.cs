using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Criteria;

/// <summary>
/// Unit tests for the Criterion entity ensuring method-based API behavior.
/// </summary>
public sealed class CriterionTests
{
    /// <summary>
    /// Ensures title returned equals the provided value.
    /// </summary>
    [Fact]
    public void It_returns_the_same_title_value()
    {
        var title = "тест-✓-タイトル-🚀-" + Guid.NewGuid();
        var description = "описание-✓-説明-🧪-" + Guid.NewGuid();
        var criterion = new Criterion(new CriterionText(title, description), System.Collections.Immutable.ImmutableList<IChoice>.Empty);

        criterion.Title().ShouldBe(title, "title value returned is not equal to input");
    }

    /// <summary>
    /// Ensures description returned equals the provided value.
    /// </summary>
    [Fact]
    public void It_returns_the_same_description_value()
    {
        var title = "заголовок-✓-見出し-🛰️-" + Guid.NewGuid();
        var description = "описание-✓-説明-🧫-" + Guid.NewGuid();
        var criterion = new Criterion(new CriterionText(title, description), System.Collections.Immutable.ImmutableList<IChoice>.Empty);

        criterion.Description().ShouldBe(description, "description value returned is not equal to input");
    }

    /// <summary>
    /// Ensures options count returned equals the provided list size.
    /// </summary>
    [Fact]
    public void It_returns_the_same_options_count()
    {
        var count = System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, 5);
        var builder = System.Collections.Immutable.ImmutableList.CreateBuilder<IChoice>();
        for (var i = 0; i < count; i++)
        {
            builder.Add(new Choice((ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1)));
        }

        var criterion = new Criterion(new CriterionText("титул-✓-标题-🚀-" + Guid.NewGuid(), "описание-✓-説明-🧪-" + Guid.NewGuid()), builder.ToImmutable());

        criterion.Options().Count.ShouldBe(count, "options count returned is not equal to input size");
    }

    /// <summary>
    /// Ensures options accessor fails fast when created with null options list.
    /// </summary>
    [Fact(DisplayName = "Criterion cannot be created with null options list")]
    public void Criterion_cannot_be_created_with_null_options_list()
    {
        Should.Throw<ArgumentNullException>(
            () => new Criterion(new CriterionText("титул-✓-标题-🚀-" + Guid.NewGuid(), "описание-✓-説明-🧪-" + Guid.NewGuid()), null!),
            "Criterion accepted a null options list which is incorrect");
    }
}
