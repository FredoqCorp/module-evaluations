using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

/// <summary>
/// Test data factory for shared value types.
/// </summary>
internal static class SharedTypesTestData
{
    internal static IBasisPoints RandomBasisPoints() =>
        new TestBasisPoints((ushort)Random.Shared.Next(1, 10000));

    internal static IPercent RandomPercent() =>
        new TestPercent((decimal)Random.Shared.NextDouble() * 100);

    internal static IWeight RandomWeight() =>
        new TestWeight(RandomPercent());

    internal static ITags EmptyTags() =>
        new TestTags([]);

    internal static ITags TagsWithSingle() =>
        new TestTags([new Tag($"tag-{Random.Shared.Next()}")]);
}

/// <summary>
/// Test double for basis points interface.
/// </summary>
internal sealed record TestBasisPoints(ushort Value) : IBasisPoints
{
    public IPercent Percent() =>
        new TestPercent(Value / 100m);

    public decimal Apply(decimal value) =>
        value * (Value / 10000m);
}

/// <summary>
/// Test double for percent interface.
/// </summary>
internal sealed record TestPercent(decimal Value) : IPercent
{
    public IBasisPoints Basis() =>
        new TestBasisPoints((ushort)(Value * 100));
}

/// <summary>
/// Test double for weight interface.
/// </summary>
internal sealed record TestWeight(IPercent Percentage) : IWeight
{
    public IPercent Percent() =>
        Percentage;

    public CriterionScore Weighted(CriterionScore score) =>
        new(score.Value * Percentage.Basis().Apply(1m));
}

/// <summary>
/// Test double for tags interface.
/// </summary>
internal sealed record TestTags(IReadOnlyList<Tag> Items) : ITags
{
    public ITags With(Tag tag) =>
        Items.Any(t => string.Equals(t.Text, tag.Text, StringComparison.OrdinalIgnoreCase))
            ? this
            : new TestTags([.. Items, tag]);

    public void Print(IMedia media, string key)
    {
        var tagTexts = Items.Select(tag => tag.Text);
        media.WriteStringArray(key, tagTexts);
    }
}
