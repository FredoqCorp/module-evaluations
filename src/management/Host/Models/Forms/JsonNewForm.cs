using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Forms;

/// <summary>
/// Form aggregate backed by raw JSON payload that coordinates metadata, groups and criteria components.
/// </summary>
internal sealed record JsonNewForm : IForm
{
    private readonly FormId _identity;
    private readonly JsonDocument _document;

    /// <summary>
    /// Creates a printable form aggregate from a JSON document.
    /// </summary>
    /// <param name="document">JSON document that describes the form structure.</param>
    public JsonNewForm(JsonDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        _document = document;
        _identity = new FormId();
    }

    /// <inheritdoc />
    public async Task<IMedia<TOutput>> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        var calculation = new JsonFormCalculation(_document.RootElement);
        var metadata = new JsonFormMetadata(_document.RootElement.GetProperty("metadata"));
        var groups = new SimpleGroups(_document.RootElement, (_, element) =>
        {
            if (calculation.Type() == CalculationType.WeightedAverage)
            {
                return new SimpleWeightedGroup(element);
            }
            else
            {
                return new SimpleAverageGroup(element);
            }
        }, calculation.Type());
        var criteria = new SimpleCriteria(_document.RootElement, (_, element) =>
        {
            if (calculation.Type() == CalculationType.WeightedAverage)
            {
                return new SimpleWeightedCriterion(element, new SimpleAverageCriterion(element));
            }
            else
            {
                return new SimpleAverageCriterion(element);
            }
        }, calculation.Type());
        media.With("formId", _identity.Value);
        metadata.Print(media);
        media.With("calculation", calculation.Type() switch
        {
            CalculationType.Average => "average",
            CalculationType.WeightedAverage => "weighted",
            _ => throw new InvalidOperationException("Unsupported calculation type")
        });
        await groups.Print(media);
        await criteria.Print(media);
        return media;
    }
}
