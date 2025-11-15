using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms.Models;

/// <summary>
/// PostgreSQL-backed calculation descriptor that maps database values to domain types.
/// </summary>
internal sealed class PgFormCalculation : IFormCalculation
{
    private readonly string _value;

    public PgFormCalculation(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        _value = value;
    }

    /// <inheritdoc />
    public CalculationType Type()
    {
        if (string.Equals(_value, "average", StringComparison.OrdinalIgnoreCase))
        {
            return CalculationType.Average;
        }

        if (string.Equals(_value, "weighted", StringComparison.OrdinalIgnoreCase))
        {
            return CalculationType.WeightedAverage;
        }

        throw new InvalidOperationException("Unknown calculation type discriminator");
    }
}
