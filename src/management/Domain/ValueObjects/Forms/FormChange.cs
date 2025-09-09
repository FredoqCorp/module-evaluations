namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Single field-level change captured for edit audit entries.
/// Path is a dotted path inside the aggregate, e.g. Meta.Name or Groups[1].Criteria[0].Weight.Value.
/// Values are stored as string snapshots to avoid reflection and type introspection.
/// </summary>
public readonly record struct FormChange(string Path, string Before, string After);

