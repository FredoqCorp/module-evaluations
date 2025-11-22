using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Represents a simple criterion title.
/// </summary>
public sealed record SimpleCriterionTitle : ICriterionTitle
{
    private readonly string _title;

    /// <summary>
    /// Creates a JSON-backed criterion title.
    /// </summary>
    /// <param name="node">JSON node containing the title field.</param>
    public SimpleCriterionTitle(JsonElement node) : this(node.String("title"))
    {
    }

    /// <summary>
    /// Creates a criterion title from a string.
    /// </summary>
    public SimpleCriterionTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        _title = title;
    }

    /// <inheritdoc />
    public string Text() => _title;
}
