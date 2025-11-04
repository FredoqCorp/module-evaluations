using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Shared;

/// <summary>
/// JSON-backed weight that exposes percentage and basis points extracted from a payload node.
/// </summary>
internal sealed record JsonWeight : IWeight
{
    private readonly JsonElement _node;

    /// <summary>
    /// Creates a JSON-backed weight reader.
    /// </summary>
    /// <param name="node">JSON element that contains the weight value.</param>
    public JsonWeight(JsonElement node)
    {
        _node = node;
    }

    /// <summary>
    /// Returns the weight expressed in basis points.
    /// </summary>
    public ushort BasisPoints()
    {
        var bp = _node.GetProperty("weight").GetDecimal() * 100;
        if (bp < 1 || bp > 10000)
        {
            throw new InvalidDataException("Weight must be between 1.00 and 100.00");
        }
        return (ushort)bp;
    }

    /// <summary>
    /// Returns the weight expressed as a percentage.
    /// </summary>
    public decimal Percent() => BasisPoints() / 100m;
}
