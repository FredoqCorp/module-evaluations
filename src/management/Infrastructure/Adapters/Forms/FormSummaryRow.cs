namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;

/// <summary>
/// Database row representing form summary with aggregated counts.
/// </summary>
internal sealed class FormSummaryRow
{
#pragma warning disable S1144, S3459
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string Code { get; init; }
    public required string Tags { get; init; }
    public required string RootGroupType { get; init; }
    public required int GroupsCount { get; init; }
    public required int CriteriaCount { get; init; }
#pragma warning restore S1144, S3459
}
