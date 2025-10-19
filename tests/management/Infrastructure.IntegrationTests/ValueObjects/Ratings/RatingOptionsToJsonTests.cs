using System.Linq;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.ValueObjects.Ratings;

/// <summary>
/// Tests for RatingOptions Print method with JSON output.
/// </summary>
public sealed class RatingOptionsToJsonTests
{
    [Fact]
    public void ToJson_ReturnsValidJsonObject()
    {
        // Arrange
        var options = new[]
        {
            new RatingOption(
                new RatingScore(5),
                new RatingLabel("Excellent"),
                new RatingAnnotation("Outstanding")),
            new RatingOption(
                new RatingScore(3),
                new RatingLabel("Good"),
                new RatingAnnotation("Satisfactory"))
        };
        var ratingOptions = new RatingOptions(options);

        // Act
        using var media = new JsonMediaWriter();
        ratingOptions.Print(media);
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
    public void ToJson_ContainsExpectedFields()
    {
        // Arrange
        var options = new[]
        {
            new RatingOption(
                new RatingScore(5),
                new RatingLabel("Excellent"),
                new RatingAnnotation("Outstanding performance"))
        };
        var ratingOptions = new RatingOptions(options);

        // Act
        using var media = new JsonMediaWriter();
        ratingOptions.Print(media);
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
    public void ToJson_HandlesEmptyAnnotation()
    {
        // Arrange
        var options = new[]
        {
            new RatingOption(
                new RatingScore(3),
                new RatingLabel("Average"),
                new RatingAnnotation(""))
        };
        var ratingOptions = new RatingOptions(options);

        // Act
        using var media = new JsonMediaWriter();
        ratingOptions.Print(media);
        var json = media.Output();
        var parsed = JsonDocument.Parse(json);
        var firstElement = parsed.RootElement.GetProperty("0");

        // Assert
        Assert.True(firstElement.TryGetProperty("annotation", out var annotationProperty));
        Assert.Equal("", annotationProperty.GetString());
    }

    [Fact]
    public void ToJson_PreservesOrder()
    {
        // Arrange
        var options = new[]
        {
            new RatingOption(new RatingScore(5), new RatingLabel("Five"), new RatingAnnotation("")),
            new RatingOption(new RatingScore(4), new RatingLabel("Four"), new RatingAnnotation("")),
            new RatingOption(new RatingScore(3), new RatingLabel("Three"), new RatingAnnotation("")),
            new RatingOption(new RatingScore(2), new RatingLabel("Two"), new RatingAnnotation("")),
            new RatingOption(new RatingScore(1), new RatingLabel("One"), new RatingAnnotation(""))
        };
        var ratingOptions = new RatingOptions(options);

        // Act
        using var media = new JsonMediaWriter();
        ratingOptions.Print(media);
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
