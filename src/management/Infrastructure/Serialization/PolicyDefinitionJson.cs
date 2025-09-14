using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Serialization;

/// <summary>
/// JSON envelope serializer for ICalculationPolicyDefinition persisted in jsonb.
/// Stores policy code and optional weights payload for weighted policies.
/// </summary>
internal static class PolicyDefinitionJson
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        WriteIndented = false,
    };

    private sealed class DefinitionDto
    {
        public string Code { get; set; } = string.Empty;
        public Dictionary<Guid, ushort>? Weights { get; set; }
    }

    /// <summary>
    /// Serializes a definition into a policy envelope JSON string.
    /// </summary>
    public static string Serialize(ICalculationPolicyDefinition def)
    {
        ArgumentNullException.ThrowIfNull(def);
        var code = def.Code();
        if (string.Equals(code, "weighted-mean", StringComparison.Ordinal))
        {
            var w = (WeightedMeanPolicyDefinition)def;
            var map = w.Weights();
            var payload = map.ToDictionary(kv => kv.Key, kv => kv.Value.Bps());
            return JsonSerializer.Serialize(new DefinitionDto { Code = code, Weights = payload }, Options);
        }
        return JsonSerializer.Serialize(new DefinitionDto { Code = code }, Options);
    }

    /// <summary>
    /// Deserializes a policy envelope JSON string into a definition instance.
    /// </summary>
    public static ICalculationPolicyDefinition Deserialize(string json)
    {
        var dto = JsonSerializer.Deserialize<DefinitionDto>(json, Options);
        if (dto == null || string.IsNullOrWhiteSpace(dto.Code))
        {
            return new ArithmeticMeanPolicyDefinition();
        }
        if (string.Equals(dto.Code, "weighted-mean", StringComparison.Ordinal))
        {
            var map = dto.Weights != null
                ? dto.Weights.ToImmutableDictionary(kv => kv.Key, kv => new Weight(kv.Value))
                : ImmutableDictionary<Guid, Weight>.Empty;
            return new WeightedMeanPolicyDefinition(map);
        }
        return new ArithmeticMeanPolicyDefinition();
    }
}

