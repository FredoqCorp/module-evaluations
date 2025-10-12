namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;

/// <summary>
/// Database row representing form metadata.
/// </summary>
internal sealed class FormRow
{
#pragma warning disable S1144, S3459
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string Code { get; init; }
    public required string Tags { get; init; }
    public required string RootGroupType { get; init; }
#pragma warning restore S1144, S3459
}
