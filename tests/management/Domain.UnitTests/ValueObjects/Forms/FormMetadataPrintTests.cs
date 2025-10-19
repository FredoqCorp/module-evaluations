using CascVel.Modules.Evaluations.Management.Domain.UnitTests.TestDoubles;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.ValueObjects.Forms;

public sealed class FormMetadataPrintTests
{
    [Fact]
    public void Print_throws_when_media_is_null()
    {
        var metadata = CreateMetadata();

        var exception = Assert.Throws<ArgumentNullException>(() =>
            metadata.Print<object>(null!));

        Assert.Equal("media", exception.ParamName);
    }

    [Fact]
    public void Print_writes_name_to_media()
    {
        var metadata = new FormMetadata(
            new FormName("Performance Review"),
            new FormDescription("Annual evaluation"),
            new FormCode("PR-2024"),
            new Tags([]));

        var media = new FakeMedia();
        metadata.Print(media);

        Assert.Contains(media.Writes, w => w.Key == "name" && (string)w.Value == "Performance Review");
    }

    [Fact]
    public void Print_writes_description_to_media()
    {
        var metadata = new FormMetadata(
            new FormName("Test Form"),
            new FormDescription("Detailed description here"),
            new FormCode("TEST-001"),
            new Tags([]));

        var media = new FakeMedia();
        metadata.Print(media);

        Assert.Contains(media.Writes, w => w.Key == "description" && (string)w.Value == "Detailed description here");
    }

    [Fact]
    public void Print_writes_code_to_media()
    {
        var metadata = new FormMetadata(
            new FormName("Test Form"),
            new FormDescription("Description"),
            new FormCode("UNIQUE-CODE-123"),
            new Tags([]));

        var media = new FakeMedia();
        metadata.Print(media);

        Assert.Contains(media.Writes, w => w.Key == "code" && (string)w.Value == "UNIQUE-CODE-123");
    }

    [Fact]
    public void Print_writes_empty_description_when_not_provided()
    {
        var metadata = new FormMetadata(
            new FormName("Test Form"),
            new FormDescription(string.Empty),
            new FormCode("CODE-001"),
            new Tags([]));

        var media = new FakeMedia();
        metadata.Print(media);

        Assert.Contains(media.Writes, w => w.Key == "description" && ((string)w.Value).Length == 0);
    }

    [Fact]
    public void Print_calls_tags_print_method()
    {
        var metadata = new FormMetadata(
            new FormName("Test Form"),
            new FormDescription("Description"),
            new FormCode("CODE-001"),
            new Tags([new Tag("important"), new Tag("urgent")]));

        var media = new FakeMedia();
        metadata.Print(media);

        var tagsWrite = Assert.Single(media.Writes, w => w.Key == "tags");
        var tagsList = Assert.IsType<List<string>>(tagsWrite.Value);
        Assert.Equal(2, tagsList.Count);
        Assert.Contains("important", tagsList);
        Assert.Contains("urgent", tagsList);
    }

    [Fact]
    public void Print_writes_all_fields_in_correct_order()
    {
        var metadata = new FormMetadata(
            new FormName("Complete Form"),
            new FormDescription("Full description"),
            new FormCode("FULL-001"),
            new Tags([new Tag("tag1")]));

        var media = new FakeMedia();
        metadata.Print(media);

        Assert.Equal(4, media.Writes.Count);
        Assert.Equal("name", media.Writes[0].Key);
        Assert.Equal("description", media.Writes[1].Key);
        Assert.Equal("code", media.Writes[2].Key);
        Assert.Equal("tags", media.Writes[3].Key);
    }

    [Fact]
    public void Print_handles_unicode_characters_in_name()
    {
        var metadata = new FormMetadata(
            new FormName("Форма оценки 评估表"),
            new FormDescription("Description"),
            new FormCode("CODE-001"),
            new Tags([]));

        var media = new FakeMedia();
        metadata.Print(media);

        Assert.Contains(media.Writes, w => w.Key == "name" && (string)w.Value == "Форма оценки 评估表");
    }

    private static FormMetadata CreateMetadata()
    {
        return new FormMetadata(
            new FormName("Test Form"),
            new FormDescription("Test Description"),
            new FormCode("TEST-001"),
            new Tags([]));
    }
}
