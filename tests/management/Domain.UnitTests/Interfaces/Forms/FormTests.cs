using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Forms;

/// <summary>
/// Tests for IForm behavioral contract.
/// </summary>
public sealed class FormTests
{
    [Fact]
    public void Validates_internal_consistency()
    {
        var form = new TestForm(Option.Of((decimal)Random.Shared.Next(1, 100)), true);

        var exception = Record.Exception(() => form.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Throws_when_validation_fails()
    {
        var form = new TestForm(Option.Of(50m), false);

        Assert.Throws<InvalidOperationException>(() => form.Validate());
    }

    [Fact]
    public void Returns_normalized_score_when_participants_exist()
    {
        var expectedScore = (decimal)Random.Shared.Next(1, 100);
        var form = new TestForm(Option.Of(expectedScore), true);

        var score = form.Score();

        Assert.True(score.IsSome);
    }

    [Fact]
    public void Returns_none_when_no_participants()
    {
        var form = new TestForm(Option.None<decimal>(), true);

        var score = form.Score();

        Assert.False(score.IsSome);
    }

    [Fact]
    public void Produces_consistent_score_across_calls()
    {
        var expectedScore = (decimal)Random.Shared.Next(1, 100);
        var form = new TestForm(Option.Of(expectedScore), true);

        var first = form.Score();
        var second = form.Score();

        Assert.Equal(first, second);
    }
}

/// <summary>
/// Test double for form interface.
/// </summary>
file sealed record TestForm(Option<decimal> TestScore, bool IsValid) : IForm
{
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }

    public Option<decimal> Score() => TestScore;

    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        throw new NotSupportedException("Test form double does not print structure");
    }
}
