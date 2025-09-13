using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Persistence.Rows;

/// <summary>
/// Persistence row for a form group kept in a normalized table.
/// Stores adjacency through ParentId and the group local criteria as jsonb.
/// </summary>
internal sealed class FormGroupRow
{
    /// <summary>
    /// Gets the stable identifier of the group.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the owning form identifier as a shadow foreign key in the database.
    /// </summary>
    public Guid FormId { get; init; }

    /// <summary>
    /// Gets the identifier of a parent group inside the same form or null for root.
    /// </summary>
    public Guid? ParentId { get; init; }

    /// <summary>
    /// Gets the human friendly title of the group.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Gets the display order within siblings.
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// Gets the local criteria list stored as a jsonb column.
    /// </summary>
    public FormCriteriaList Criteria { get; init; } = new([]);
}

