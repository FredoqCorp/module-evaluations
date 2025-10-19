using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

/// <summary>
/// Test data factory for rating option scenarios.
/// </summary>
internal static class RatingOptionTestData
{
    internal static IRatingOption OptionWithScore(ushort score) =>
        new TestRatingOption(new RatingScore(score));

    internal static IRatingOption RandomOption() =>
        new TestRatingOption(
            new RatingScore((ushort)Random.Shared.Next(1, 10)));
}

/// <summary>
/// Test double for rating option interface.
/// </summary>
internal sealed record TestRatingOption(RatingScore ScoreValue) : IRatingOption
{
    public RatingLabel Label { get; init; } = new RatingLabel("Test Label");
    public RatingAnnotation Annotation { get; init; } = new RatingAnnotation("Test Annotation");

    public int Score => ScoreValue.Value;

    public bool Matches(RatingScore score) =>
        ScoreValue.Value == score.Value;

    public void Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        
        media.WriteInt32("score", ScoreValue.Value);
        media.WriteString("label", Label.Value);
        media.WriteString("annotation", Annotation.Text);
    }
}
