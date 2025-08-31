using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Criteria;

/// <summary>
/// Unit tests for the Criterion entity ensuring method-based API behavior.
/// </summary>
public sealed class CriterionTests
{
    /// <summary>
    /// Ensures id text returned equals the provided identifier.
    /// </summary>
    [Fact]
    public void It_returns_the_same_id_text()
    {
        var guid = Guid.NewGuid();
        var id = new Uuid(guid);
        var text = new CriterionText("заголовок-✓-見出し-🛰️-" + Guid.NewGuid(), "описание-✓-説明-🧫-" + Guid.NewGuid());
        var options = new List<IChoice> { new Choice((ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1)) };
        var criterion = new Criterion(id, text, options);

        criterion.Id().Text().ShouldBe(id.Text(), "id text value returned is not equal to input");
    }

    /// <summary>
    /// Ensures title returned equals the provided value.
    /// </summary>
    [Fact]
    public void It_returns_the_same_title_value()
    {
        var title = "тест-✓-タイトル-🚀-" + Guid.NewGuid();
        var description = "описание-✓-説明-🧪-" + Guid.NewGuid();
        var criterion = new Criterion(new Uuid(Guid.NewGuid()), new CriterionText(title, description), new List<IChoice>());

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
        var criterion = new Criterion(new Uuid(Guid.NewGuid()), new CriterionText(title, description), new List<IChoice>());

        criterion.Description().ShouldBe(description, "description value returned is not equal to input");
    }

    /// <summary>
    /// Ensures options count returned equals the provided list size.
    /// </summary>
    [Fact]
    public void It_returns_the_same_options_count()
    {
        var count = System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, 5);
        var opts = new List<IChoice>();
        for (var i = 0; i < count; i++)
        {
            opts.Add(new Choice((ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1)));
        }

        var criterion = new Criterion(new Uuid(Guid.NewGuid()), new CriterionText("титул-✓-标题-🚀-" + Guid.NewGuid(), "описание-✓-説明-🧪-" + Guid.NewGuid()), opts);

        criterion.Options().Count.ShouldBe(count, "options count returned is not equal to input size");
    }
}

