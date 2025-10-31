using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.TestDoubles;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Entities.Forms;

public sealed class FormSummaryTests
{
    [Fact]
    public void Constructor_throws_when_metadata_is_null()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new FormSummary(
                new FormId(),
                null!,
                CalculationType.Average,
                5,
                10));

        Assert.Equal("metadata", exception.ParamName);
    }

    [Fact]
    public void Constructor_throws_when_groups_count_is_negative()
    {
        var metadata = CreateMetadata();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FormSummary(
                new FormId(),
                metadata,
                CalculationType.Average,
                -1,
                10));

        Assert.Contains("groupsCount", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Constructor_throws_when_criteria_count_is_negative()
    {
        var metadata = CreateMetadata();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FormSummary(
                new FormId(),
                metadata,
                CalculationType.WeightedAverage,
                5,
                -5));

        Assert.Contains("criteriaCount", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Print_throws_when_media_is_null()
    {
        var summary = CreateValidSummary();

        var exception = Assert.Throws<ArgumentNullException>(() =>
            summary.Print<object>(null!));

        Assert.Equal("media", exception.ParamName);
    }

    [Fact]
    public void Print_writes_id_to_media()
    {
        var id = new FormId();
        var summary = new FormSummary(
            id,
            CreateMetadata(),
            CalculationType.Average,
            3,
            7);

        using var media = new FakeMedia();
        summary.Print(media);

        Assert.Contains(media.Writes, w => w.Key == "id" && w.Value.ToString() == id.Value.ToString());
    }

    [Fact]
    public void Print_writes_groups_count_to_media()
    {
        var summary = new FormSummary(
            new FormId(),
            CreateMetadata(),
            CalculationType.WeightedAverage,
            12,
            25);

        using var media = new FakeMedia();
        summary.Print(media);

        Assert.Contains(media.Writes, w => w.Key == "groupsCount" && (int)w.Value == 12);
    }

    [Fact]
    public void Print_writes_criteria_count_to_media()
    {
        var summary = new FormSummary(
            new FormId(),
            CreateMetadata(),
            CalculationType.Average,
            8,
            42);

        using var media = new FakeMedia();
        summary.Print(media);

        Assert.Contains(media.Writes, w => w.Key == "criteriaCount" && (int)w.Value == 42);
    }

    [Fact]
    public void Print_writes_calculation_type_to_media()
    {
        var summary = new FormSummary(
            new FormId(),
            CreateMetadata(),
            CalculationType.WeightedAverage,
            5,
            15);

        using var media = new FakeMedia();
        summary.Print(media);

        Assert.Contains(media.Writes, w => w.Key == "calculationType" && w.Value.ToString() == "WeightedAverage");
    }

    [Fact]
    public void Constructor_accepts_zero_groups_count()
    {
        var summary = new FormSummary(
            new FormId(),
            CreateMetadata(),
            CalculationType.Average,
            0,
            5);

        Assert.NotNull(summary);
    }

    [Fact]
    public void Constructor_accepts_zero_criteria_count()
    {
        var summary = new FormSummary(
            new FormId(),
            CreateMetadata(),
            CalculationType.WeightedAverage,
            3,
            0);

        Assert.NotNull(summary);
    }

    private static FormMetadata CreateMetadata()
    {
        return new FormMetadata(
            new FormName("Test Form"),
            new FormDescription("Test Description"),
            new FormCode("TEST-001"),
            new Tags([new Tag("test")]));
    }

    private static FormSummary CreateValidSummary()
    {
        return new FormSummary(
            new FormId(),
            CreateMetadata(),
            CalculationType.Average,
            5,
            10);
    }
}
