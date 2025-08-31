using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Criteria.ValueObjects;

/// <summary>
/// Unit tests for the Choice value object ensuring score and optional fields behavior.
/// </summary>
public sealed class ChoiceTests
{
    /// <summary>
    /// Ensures score returned equals the provided value.
    /// </summary>
    [Fact]
    public void It_returns_the_same_score_value()
    {
        var score = (ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1);
        var choice = new Choice(score);

        choice.Score().ShouldBe(score, "score value returned is not equal to input");
    }

    /// <summary>
    /// Ensures caption is empty when not provided.
    /// </summary>
    [Fact]
    public void It_returns_empty_caption_when_not_provided()
    {
        var score = (ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1);
        var choice = new Choice(score);

        choice.Caption().ShouldBe(string.Empty, "caption is not empty when not provided");
    }

    /// <summary>
    /// Ensures annotation is empty when not provided.
    /// </summary>
    [Fact]
    public void It_returns_empty_annotation_when_not_provided()
    {
        var score = (ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1);
        var choice = new Choice(score);

        choice.Annotation().ShouldBe(string.Empty, "annotation is not empty when not provided");
    }

    /// <summary>
    /// Ensures non ASCII caption is preserved.
    /// </summary>
    [Fact]
    public void It_preserves_non_ascii_caption()
    {
        var score = (ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1);
        var caption = "тест-✓-タイトル-🚀-" + Guid.NewGuid();
        var choice = new Choice(score, caption, string.Empty);

        choice.Caption().ShouldBe(caption, "caption value is not preserved for non ASCII input");
    }

    /// <summary>
    /// Ensures non ASCII annotation is preserved.
    /// </summary>
    [Fact]
    public void It_preserves_non_ascii_annotation()
    {
        var score = (ushort)System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, ushort.MaxValue + 1);
        var annotation = "примечание-✓-備考-🧪-" + Guid.NewGuid();
        var choice = new Choice(score, string.Empty, annotation);

        choice.Annotation().ShouldBe(annotation, "annotation value is not preserved for non ASCII input");
    }
}
