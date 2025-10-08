using CascVel.Modules.Evaluations.Management.Domain.Common;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for contribution values produced by rating selections.
/// </summary>
public interface IRatingContribution
{
    /// <summary>
    /// Aggregates this contribution with another contribution and returns the combined result.
    /// </summary>
    /// <param name="contribution">The contribution to be combined with this contribution.</param>
    /// <returns>A new contribution representing the combined result.</returns>
    IRatingContribution Join(IRatingContribution contribution);

    /// <summary>
    /// Projects the encapsulated amount and participant count into the specified type.
    /// </summary>
    /// <typeparam name="T">The target type of the projection.</typeparam>
    /// <param name="projector">The projection function receiving amount and participant count.</param>
    /// <returns>The result of the projection.</returns>
    T Accept<T>(Func<decimal, ushort, T> projector);

    /// <summary>
    /// Calculates the normalized contribution value that should participate in final scoring.
    /// </summary>
    /// <returns>An optional decimal representing the normalized total when participants exist; otherwise, None.</returns>
    Option<decimal> Total();
}
