using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

internal sealed record JsonWeight : IWeight
{
    private readonly JsonElement _node;

    public JsonWeight(JsonElement node)
    {
        _node = node;
    }

    public ushort BasisPoints()
    {
        var bp = _node.GetProperty("weight").GetDecimal() * 100;
        if (bp < 1 || bp > 10000)
        {
            throw new InvalidDataException("Weight must be between 1.00 and 100.00");
        }
        return (ushort)bp;
    }

    public decimal Percent() => BasisPoints() / 100m;
}
