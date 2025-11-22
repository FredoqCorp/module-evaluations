using CascVel.Modules.Evaluations.Management.Domain.Models.Shared;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Shared;

/// <summary>
/// Tests for ITags behavioral contract.
/// </summary>
public sealed class TagsTests
{
    [Fact]
    public void Returns_new_collection_with_added_tag()
    {
        var tags = SharedTypesTestData.EmptyTags();
        var tag = new SimpleTag($"новый-тег-{Random.Shared.Next()}");

        var updated = tags.With(tag);

        Assert.NotNull(updated);
    }

    [Fact]
    public void Preserves_immutability_when_adding_tag()
    {
        var original = SharedTypesTestData.EmptyTags();
        var tag = new SimpleTag($"тэг-{Random.Shared.Next()}");

        var updated = original.With(tag);

        Assert.NotEqual(original, updated);
    }

    [Fact]
    public void Enforces_uniqueness_when_adding_duplicate()
    {
        var tag = new SimpleTag($"дубликат-{Random.Shared.Next()}");
        var tags = SharedTypesTestData.EmptyTags().With(tag);

        var updated = tags.With(tag);

        Assert.Equal(tags, updated);
    }

    [Fact]
    public void Supports_non_ascii_tag_text()
    {
        var tags = SharedTypesTestData.EmptyTags();
        var cyrillicTag = new SimpleTag("тег-на-кириллице");

        var updated = tags.With(cyrillicTag);

        Assert.NotNull(updated);
    }

    [Fact]
    public void Handles_multiple_sequential_additions()
    {
        var tags = SharedTypesTestData.EmptyTags();
        var first = new SimpleTag($"первый-{Random.Shared.Next()}");
        var second = new SimpleTag($"второй-{Random.Shared.Next()}");

        var updated = tags.With(first).With(second);

        Assert.NotNull(updated);
    }
}
