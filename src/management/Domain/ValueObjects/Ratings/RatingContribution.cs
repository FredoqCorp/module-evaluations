using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

/// <summary>
/// Immutable contribution value that captures the aggregated score amount alongside participant count.
/// </summary>
public sealed record RatingContribution : IRatingContribution
{
    private readonly decimal _amount;
    private readonly ushort _participants;

    /// <summary>
    /// Creates a contribution value while enforcing a non-negative amount.
    /// </summary>
    /// <param name="amount">Aggregated score amount.</param>
    /// <param name="participants">Number of participants contributing to the score.</param>
    public RatingContribution(decimal amount, ushort participants)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        _amount = amount;
        _participants = participants;
    }

    /// <summary>
    /// Aggregates this contribution with another contribution and returns the combined result.
    /// </summary>
    /// <param name="contribution">The contribution to be combined with this contribution.</param>
    /// <returns>A new contribution representing the combined result.</returns>
    public IRatingContribution Join(IRatingContribution contribution)
    {
        ArgumentNullException.ThrowIfNull(contribution);
        return contribution.Accept((amount, participants) =>
            new RatingContribution(_amount + amount, (ushort)(_participants + participants)));
    }

    /// <summary>
    /// Calculates the normalized contribution value that should participate in final scoring.
    /// </summary>
    /// <returns>An optional decimal representing the normalized total when participants exist; otherwise, None.</returns>
    public Option<decimal> Total()
    {
        if (_participants == 0)
        {
            return Option.None<decimal>();
        }

        return Option.Of(_amount / _participants);
    }

    /// <summary>
    /// Projects the encapsulated amount and participant count into the specified type.
    /// </summary>
    /// <typeparam name="T">The target type of the projection.</typeparam>
    /// <param name="projector">The projection function receiving amount and participant count.</param>
    /// <returns>The result of the projection.</returns>
    public T Accept<T>(Func<decimal, ushort, T> projector)
    {
        ArgumentNullException.ThrowIfNull(projector);
        return projector(_amount, _participants);
    }
}
