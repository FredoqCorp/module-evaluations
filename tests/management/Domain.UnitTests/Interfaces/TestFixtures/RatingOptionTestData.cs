using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;
using CascVel.Modules.Evaluations.Management.Host.Models.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

/// <summary>
/// Test data factory for rating option scenarios.
/// </summary>
internal static class RatingOptionTestData
{
    internal static IRatingOption OptionWithScore(ushort score) =>
        new TestRatingOption(new SimpleRatingScore(score));

    internal static IRatingOption RandomOption() =>
        new TestRatingOption(
            new SimpleRatingScore((ushort)Random.Shared.Next(1, 10)));
}

/// <summary>
/// Test double for rating option interface.
/// </summary>
internal sealed record TestRatingOption(SimpleRatingScore ScoreValue) : IRatingOption
{
    public SimpleRatingLabel Label { get; init; } = new SimpleRatingLabel("Test Label");
    public SimpleRatingAnnotation Annotation { get; init; } = new SimpleRatingAnnotation("Test Annotation");

    public int Score => ScoreValue.Value();

    public bool Matches(SimpleRatingScore score) =>
        ScoreValue.Value() == score.Value();

    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        media.With("score", ScoreValue.Value());
        media.With("label", Label.Text());
        media.With("annotation", Annotation.Text());
        return media;
    }
}
