using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Represents an immutable collection of FormGroup value objects for form criteria grouping.
/// </summary>
public sealed record class FormGroupList
{
    private readonly ImmutableList<FormGroup> _groups;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormGroupList"/> record with the specified groups.
    /// </summary>
    /// <param name="groups">The collection of <see cref="FormGroup"/> value objects to group.</param>
    public FormGroupList(IList<FormGroup> groups)
    {
        ArgumentNullException.ThrowIfNull(groups);
        _groups = [.. groups];
    }

    /// <summary>
    /// Gets an immutable list of <see cref="FormGroupId"/> representing the IDs of the form groups.
    /// </summary>
    public IImmutableList<FormGroupId> Ids() => _groups.Select(g => g.Id).ToImmutableList();

    /// <summary>
    /// Returns the <see cref="FormGroup"/> with the specified <see cref="Guid"/> value.
    /// Throws <see cref="KeyNotFoundException"/> if no group with the given ID exists.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> value of the group ID to find.</param>
    /// <returns>The <see cref="FormGroup"/> matching the specified ID.</returns>
    public FormGroup Group(Guid value)
    {
        return _groups.FirstOrDefault(g => g.Id.Value == value)
               ?? throw new KeyNotFoundException($"Group with ID {value} not found.");
    }

    /// <summary>
    /// Gets the number of form groups in the collection.
    /// </summary>
    public int Count() => _groups.Count;
}
