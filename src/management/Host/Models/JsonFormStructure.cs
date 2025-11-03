namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Represents immutable form structure including groups and criteria collections.
/// </summary>
internal sealed record JsonFormStructure(JsonGroups Groups, JsonCriteria Criteria);
