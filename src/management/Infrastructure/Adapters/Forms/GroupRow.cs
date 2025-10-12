namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;

/// <summary>
/// Database row representing form group hierarchy.
/// </summary>
internal sealed class GroupRow
{
#pragma warning disable S1144, S3459
    public required Guid Id { get; init; }
    public required Guid FormId { get; init; }
    public Guid? ParentId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string GroupType { get; init; }
    public int? WeightBasisPoints { get; init; }
    public required int OrderIndex { get; init; }
#pragma warning restore S1144, S3459
}
