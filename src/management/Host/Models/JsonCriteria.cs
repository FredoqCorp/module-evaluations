using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Represents a criteria collection backed by JSON.
/// </summary>
internal sealed class JsonCriteria : ICriteria
{
    private readonly JsonElement _container;
    private readonly CalculationType _calculation;

    /// <summary>
    /// Creates a JSON-backed criteria collection.
    /// </summary>
    /// <param name="container">JSON element containing the criteria array.</param>
    /// <param name="calculation">Calculation strategy used by the parent form.</param>
    public JsonCriteria(JsonElement container, CalculationType calculation)
    {
        _container = container;
        _calculation = calculation;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media, Guid formId, Guid? groupId)
    {
        ArgumentNullException.ThrowIfNull(media);

        foreach (var node in JsonFormNodes.Collection(_container, "criteria"))
        {
            var criterion = new JsonCriterion(node, _calculation);
            criterion.Print(media, formId, groupId);
        }
        return media;
    }
}
