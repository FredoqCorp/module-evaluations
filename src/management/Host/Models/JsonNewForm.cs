using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Form aggregate backed by raw JSON payload that coordinates metadata, groups and criteria components.
/// </summary>
internal sealed record JsonNewForm : IForm
{
    private readonly FormId _identity;
    private readonly JsonFormMetadata _metadata;
    private readonly JsonFormCalculation _calculation;
    private readonly JsonFormStructure _structure;

    /// <summary>
    /// Creates a printable form aggregate from a JSON document.
    /// </summary>
    /// <param name="document">JSON document that describes the form structure.</param>
    public JsonNewForm(JsonDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        using var snapshot = JsonDocument.Parse(document.RootElement.GetRawText());
        var root = snapshot.RootElement;
        _calculation = new JsonFormCalculation(document);
        _metadata = new JsonFormMetadata(document);
        _identity = new FormId();

        var layout = JsonFormNodes.Section(root, "root").Clone();
        var groups = new JsonGroups(layout, _calculation);
        var criteria = new JsonCriteria(layout, _calculation);
        _structure = new JsonFormStructure(groups, criteria);
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        var token = JsonFormNodes.CalculationToken(_calculation);
        media.With("formId", _identity.Value);
        _metadata.Print(media);
        media.With("calculation", token);

        var form = _identity.Value;
        _structure.Criteria.Print(media, form, null);
        _structure.Groups.Print(media, form, null);
        return media;
    }
}
