using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Persistence.Rows;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Mappers;

/// <summary>
/// Utilities to assemble a domain FormGroupList tree from normalized rows.
/// </summary>
internal static class FormGroupsTreeMapper
{
    /// <summary>
    /// Builds a FormGroupList tree from a flat list of normalized rows.
    /// </summary>
    public static FormGroupList Build(IReadOnlyList<FormGroupRow> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);

        var byId = rows.ToDictionary(r => r.Id);
        var byParent = rows.ToLookup(r => r.ParentId);

        FormGroup Map(Guid id)
        {
            var r = byId[id];
            var children = byParent[id].Select(ch => Map(ch.Id)).ToList();
            return new FormGroup(new FormGroupId(r.Id), r.Title, new OrderIndex(r.Order), r.Criteria, new FormGroupList(children));
        }

        var roots = byParent[null].Select(r => Map(r.Id)).ToList();
        return new FormGroupList(roots);
    }
}

