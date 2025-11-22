using System.Collections.Immutable;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;
using CascVel.Modules.Evaluations.Management.Host.Models.Ratings;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.ValueObjects.Ratings;

/// <summary>
/// Tests for RatingOptions Print method with JSON output.
/// </summary>
public sealed class RatingOptionsToJsonTests
{
    [Fact]
    public async Task ToJson_ReturnsValidJsonObject()
    {
        // Arrange
        IRatingOption[] options =
        [
            new SimpleRatingOption(
                new SimpleRatingScore(5),
                new SimpleRatingLabel("Excellent"),
                new SimpleRatingAnnotation("Outstanding")),
            new SimpleRatingOption(
                new SimpleRatingScore(3),
                new SimpleRatingLabel("Good"),
                new SimpleRatingAnnotation("Satisfactory"))
        ];
        var ratingOptions = new SimpleRatingOptions(options.ToImmutableList());

        // Act
        using var media = new JsonMediaWriter();
        await ratingOptions.Print(media);
        var json = media.Output();

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);

        // Verify it's valid JSON
        var parsed = JsonDocument.Parse(json);
        Assert.Equal(JsonValueKind.Object, parsed.RootElement.ValueKind);
        Assert.True(parsed.RootElement.TryGetProperty("0", out _));
        Assert.True(parsed.RootElement.TryGetProperty("1", out _));
    }

    [Fact]
    public async Task ToJson_ContainsExpectedFields()
    {
        // Arrange
        IRatingOption[] options =
        [
            new SimpleRatingOption(
                new SimpleRatingScore(5),
                new SimpleRatingLabel("Excellent"),
                new SimpleRatingAnnotation("Outstanding performance"))
        ];
        var ratingOptions = new SimpleRatingOptions(options.ToImmutableList());

        // Act
        using var media = new JsonMediaWriter();
        await ratingOptions.Print(media);
        var json = media.Output();
        var parsed = JsonDocument.Parse(json);
        var firstElement = parsed.RootElement.GetProperty("0");

        // Assert
        Assert.True(firstElement.TryGetProperty("score", out var scoreProperty));
        Assert.Equal(5, scoreProperty.GetInt32());

        Assert.True(firstElement.TryGetProperty("label", out var labelProperty));
        Assert.Equal("Excellent", labelProperty.GetString());

        Assert.True(firstElement.TryGetProperty("annotation", out var annotationProperty));
        Assert.Equal("Outstanding performance", annotationProperty.GetString());
    }

    [Fact]
    public async Task ToJson_HandlesEmptyAnnotation()
    {
        // Arrange
        IRatingOption[] options =
        [
            new SimpleRatingOption(
                new SimpleRatingScore(3),
                new SimpleRatingLabel("Average"),
                new SimpleRatingAnnotation(""))
        ];
        var ratingOptions = new SimpleRatingOptions(options.ToImmutableList());

        // Act
        using var media = new JsonMediaWriter();
        await ratingOptions.Print(media);
        var json = media.Output();
        var parsed = JsonDocument.Parse(json);
        var firstElement = parsed.RootElement.GetProperty("0");

        // Assert
        Assert.True(firstElement.TryGetProperty("annotation", out var annotationProperty));
        Assert.Equal("", annotationProperty.GetString());
    }

    [Fact]
    public async Task ToJson_PreservesOrder()
    {
        // Arrange
        IRatingOption[] options =
        [
            new SimpleRatingOption(new SimpleRatingScore(5), new SimpleRatingLabel("Five"), new SimpleRatingAnnotation("")),
            new SimpleRatingOption(new SimpleRatingScore(4), new SimpleRatingLabel("Four"), new SimpleRatingAnnotation("")),
            new SimpleRatingOption(new SimpleRatingScore(3), new SimpleRatingLabel("Three"), new SimpleRatingAnnotation("")),
            new SimpleRatingOption(new SimpleRatingScore(2), new SimpleRatingLabel("Two"), new SimpleRatingAnnotation("")),
            new SimpleRatingOption(new SimpleRatingScore(1), new SimpleRatingLabel("One"), new SimpleRatingAnnotation(""))
        ];
        var ratingOptions = new SimpleRatingOptions(options.ToImmutableList());

        // Act
        using var media = new JsonMediaWriter();
        await ratingOptions.Print(media);
        var json = media.Output();
        var parsed = JsonDocument.Parse(json);
        var properties = parsed.RootElement.EnumerateObject().ToArray();

        // Assert
        Assert.Equal(5, properties.Length);
        Assert.Equal("0", properties[0].Name);
        Assert.Equal(5, properties[0].Value.GetProperty("score").GetInt32());
        Assert.Equal("1", properties[1].Name);
        Assert.Equal(4, properties[1].Value.GetProperty("score").GetInt32());
        Assert.Equal("2", properties[2].Name);
        Assert.Equal(3, properties[2].Value.GetProperty("score").GetInt32());
        Assert.Equal("3", properties[3].Name);
        Assert.Equal(2, properties[3].Value.GetProperty("score").GetInt32());
        Assert.Equal("4", properties[4].Name);
        Assert.Equal(1, properties[4].Value.GetProperty("score").GetInt32());
    }
}
