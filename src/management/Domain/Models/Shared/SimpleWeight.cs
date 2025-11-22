using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

/// <summary>
/// JSON-backed weight that exposes percentage and basis points extracted from a payload node.
/// </summary>
public sealed record SimpleWeight : IWeight
{
    private readonly ushort _basisPoints;

    /// <summary>
    /// Creates a JSON-backed weight reader.
    /// </summary>
    /// <param name="node">JSON element that contains the weight value.</param>
    public SimpleWeight(JsonElement node) : this(node.Decimal("weight"))
    {
    }

    /// <summary>
    /// Creates a weight reader from a specific weight value.
    /// </summary>
    /// <param name="weight">Weight value as a decimal percentage.</param>
    public SimpleWeight(decimal weight) : this(weight.BasisPoints())
    {
    }

    /// <summary>
    /// Creates a weight reader from a specific basis points value.
    /// </summary>
    /// <param name="basisPoints">Weight value in basis points.</param>
    public SimpleWeight(ushort basisPoints)
    {
        _basisPoints = basisPoints;
    }

    /// <summary>
    /// Returns the weight expressed in basis points.
    /// </summary>
    public ushort BasisPoints() => _basisPoints;

    /// <summary>
    /// Returns the weight expressed as a percentage.
    /// </summary>
    public decimal Percent() => _basisPoints / 100m;
}

