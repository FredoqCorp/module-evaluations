namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;

/// <summary>
/// Database row representing evaluation criterion.
/// </summary>
internal sealed class CriterionRow
{
#pragma warning disable S1144, S3459
    public required Guid Id { get; init; }
    public required Guid FormId { get; init; }
    public Guid? GroupId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required string CriterionType { get; init; }
    public int? WeightBasisPoints { get; init; }
    public required int OrderIndex { get; init; }
#pragma warning restore S1144, S3459
}
