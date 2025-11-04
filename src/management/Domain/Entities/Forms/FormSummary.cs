using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Immutable entity that represents a summarized view of an evaluation form with essential information.
/// </summary>
public sealed record FormSummary : IFormSummary
{
    private readonly FormId _id;
    private readonly FormMetadata _metadata;
    private readonly CalculationType _calculationType;
    private readonly int _groupsCount;
    private readonly int _criteriaCount;

    /// <summary>
    /// Initializes the form summary with identification, metadata, and structural statistics.
    /// </summary>
    /// <param name="id">Unique identifier of the form.</param>
    /// <param name="metadata">Metadata associated with the form.</param>
    /// <param name="calculationType">Type of calculation rule applied at the root level.</param>
    /// <param name="groupsCount">Total number of groups in the form structure.</param>
    /// <param name="criteriaCount">Total number of criteria across all groups.</param>
    public FormSummary(
        FormId id,
        FormMetadata metadata,
        CalculationType calculationType,
        int groupsCount,
        int criteriaCount)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentOutOfRangeException.ThrowIfNegative(groupsCount);
        ArgumentOutOfRangeException.ThrowIfNegative(criteriaCount);

        _id = id;
        _metadata = metadata;
        _calculationType = calculationType;
        _groupsCount = groupsCount;
        _criteriaCount = criteriaCount;
    }

    /// <summary>
    /// Prints the form summary representation into the provided media.
    /// </summary>
    /// <param name="media">Target media that receives the printed representation.</param>
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        media.With("id", _id.Value);
        _metadata.Print(media);
        media
            .With("groupsCount", _groupsCount)
            .With("criteriaCount", _criteriaCount)
            .With("calculation", _calculationType switch
            {
                CalculationType.Average => "average",
                CalculationType.WeightedAverage => "weighted",
                _ => throw new InvalidOperationException("Unsupported calculation type")
            });
        return media;
    }
}
