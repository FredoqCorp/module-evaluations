using CascVel.Modules.Evaluations.Management.Domain.Models.Shared;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.TestDoubles;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.ValueObjects.Shared;

public sealed class TagsPrintTests
{
    [Fact]
    public void Print_throws_when_media_is_null()
    {
        var tags = new Tags([new Tag("test")]);

        var exception = Assert.Throws<ArgumentNullException>(() =>
            tags.Print<object>(null!, "tags"));

        Assert.Equal("media", exception.ParamName);
    }

    [Fact]
    public void Print_throws_when_key_is_null()
    {
        var tags = new Tags([new Tag("test")]);
        using var media = new FakeMedia();

        var exception = Assert.Throws<ArgumentNullException>(() =>
            tags.Print(media, null!));

        Assert.Contains("key", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Print_throws_when_key_is_empty()
    {
        var tags = new Tags([new Tag("test")]);
        using var media = new FakeMedia();

        var exception = Assert.Throws<ArgumentException>(() =>
            tags.Print(media, string.Empty));

        Assert.Contains("key", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Print_throws_when_key_is_whitespace()
    {
        var tags = new Tags([new Tag("test")]);
        using var media = new FakeMedia();

        var exception = Assert.Throws<ArgumentException>(() =>
            tags.Print(media, "   "));

        Assert.Contains("key", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Print_writes_empty_array_when_no_tags()
    {
        var tags = new Tags([]);
        using var media = new FakeMedia();

        tags.Print(media, "tags");

        var write = Assert.Single(media.Writes, w => w.Key == "tags");
        var array = Assert.IsType<List<string>>(write.Value);
        Assert.Empty(array);
    }

    [Fact]
    public void Print_writes_single_tag_to_array()
    {
        var tags = new Tags([new Tag("important")]);
        using var media = new FakeMedia();

        tags.Print(media, "tags");

        var write = Assert.Single(media.Writes, w => w.Key == "tags");
        var array = Assert.IsType<List<string>>(write.Value);
        Assert.Equal("important", Assert.Single(array));
    }

    [Fact]
    public void Print_writes_multiple_tags_to_array()
    {
        var tags = new Tags([
            new Tag("urgent"),
            new Tag("reviewed"),
            new Tag("approved")
        ]);
        using var media = new FakeMedia();

        tags.Print(media, "labels");

        var write = Assert.Single(media.Writes, w => w.Key == "labels");
        var array = Assert.IsType<List<string>>(write.Value);
        Assert.Equal(3, array.Count);
        Assert.Contains("urgent", array);
        Assert.Contains("reviewed", array);
        Assert.Contains("approved", array);
    }

    [Fact]
    public void Print_preserves_tag_order()
    {
        var tags = new Tags([
            new Tag("first"),
            new Tag("second"),
            new Tag("third")
        ]);
        using var media = new FakeMedia();

        tags.Print(media, "tags");

        var write = Assert.Single(media.Writes, w => w.Key == "tags");
        var array = Assert.IsType<List<string>>(write.Value);
        Assert.Equal(["first", "second", "third"], array);
    }

    [Fact]
    public void Print_handles_special_characters_in_tags()
    {
        var tags = new Tags([
            new Tag("tag-with-dash"),
            new Tag("tag_with_underscore"),
            new Tag("tag.with.dots")
        ]);
        using var media = new FakeMedia();

        tags.Print(media, "tags");

        var write = Assert.Single(media.Writes, w => w.Key == "tags");
        var array = Assert.IsType<List<string>>(write.Value);
        Assert.Contains("tag-with-dash", array);
        Assert.Contains("tag_with_underscore", array);
        Assert.Contains("tag.with.dots", array);
    }
}
