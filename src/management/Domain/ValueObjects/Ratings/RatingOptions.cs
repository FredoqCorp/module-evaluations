using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable collection of rating options.
/// </summary>
public sealed record RatingOptions : IRatingOptions
{
    private readonly IImmutableList<IRatingOption> _options;

    /// <summary>
    /// Creates a collection of rating options from the provided enumerable without accepting null.
    /// </summary>
    /// <param name="options">Enumerable of rating options.</param>
    public RatingOptions(IEnumerable<IRatingOption> options)
    {
        _options = [.. options];
    }

    /// <summary>
    /// Calculates the total contribution produced by the selected option, if any.
    /// </summary>
    /// <returns>The contribution that should participate in downstream scoring.</returns>
    public IRatingContribution Contribution()
    {
        IRatingContribution total = new RatingContribution(decimal.Zero, 0);

        foreach (var option in _options)
        {
            total = total.Join(option.Contribution());
        }

        return total;
    }

}
