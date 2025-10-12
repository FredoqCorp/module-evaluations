namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;

/// <summary>
/// Database row representing rating option for criterion.
/// </summary>
internal sealed class RatingOptionRow
{
#pragma warning disable S1144, S3459
    public required Guid Id { get; init; }
    public required Guid CriterionId { get; init; }
    public required decimal Score { get; init; }
    public required string Label { get; init; }
    public string? Annotation { get; init; }
    public required int OrderIndex { get; init; }
#pragma warning restore S1144, S3459
}
