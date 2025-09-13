using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Serialization;

/// <summary>
/// Centralized JSON (de)serialization helpers for FormCriteriaList to ensure consistent jsonb storage.
/// </summary>
internal static class FormCriteriaJson
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
        WriteIndented = false,
    };

    /// <summary>
    /// Serializes the criteria list to a compact JSON string.
    /// </summary>
    public static string Serialize(FormCriteriaList value)
    {
        if (value == null)
        {
            return "[]";
        }
        var list = value.Items();
        return JsonSerializer.Serialize(list, Options);
    }

    /// <summary>
    /// Deserializes the JSON string to a criteria list.
    /// </summary>
    public static FormCriteriaList Deserialize(string json)
    {
        var list = JsonSerializer.Deserialize<List<FormCriterion>>(json, Options) ?? new List<FormCriterion>(0);
        return new FormCriteriaList(list);
    }
}

