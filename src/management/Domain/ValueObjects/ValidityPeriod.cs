using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable validity period that enforces chronological order and answers availability queries.
/// </summary>
public sealed record ValidityPeriod : IValidityPeriod
{
    private readonly ValidityStart _start;

    private readonly Option<ValidityEnd> _end = Option.None<ValidityEnd>();

    /// <summary>
    /// Initializes the validity period with required boundaries.
    /// </summary>
    /// <param name="start">Inclusive start moment.</param>
    /// <param name="end">Inclusive finish.</param>
    public ValidityPeriod(ValidityStart start, ValidityEnd end): this(start, Option.Of(end))
    {
    }

    /// <summary>
    /// Initializes the validity period with open end.
    /// </summary>
    /// <param name="start">Inclusive start moment.</param>
    public ValidityPeriod(ValidityStart start): this(start, Option.None<ValidityEnd>())
    {
    }

    private ValidityPeriod(ValidityStart start, Option<ValidityEnd> end)
    {
        _start = start;
        _end = end;
    }

    /// <summary>
    /// Returns a new validity period that ends at the provided moment.
    /// </summary>
    /// <param name="end">Moment that closes the validity window.</param>
    /// <returns>Validity period with updated boundary.</returns>
    public IValidityPeriod Until(ValidityEnd end)
    {
        return new ValidityPeriod(_start, end);
    }

    /// <summary>
    /// Determines whether the provided moment falls inside the validity window.
    /// </summary>
    /// <param name="moment">Moment to evaluate.</param>
    /// <returns>True when the moment is inside the window.</returns>
    public bool Active(DateTime moment)
    {
        if (moment < _start.Value)
        {
            return false;
        }

        return _end.Map(end => moment <= end.Value).Reduce(() => true);
    }
}
