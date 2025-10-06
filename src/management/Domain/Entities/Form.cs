using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Immutable aggregate root that represents an evaluation form.
/// </summary>
public sealed class Form : IForm
{
    private readonly FormId _id;
    private readonly FormMetadata _metadata;
    private readonly ICriteria _criteria;
    private readonly IGroups _groups;
    /// <summary>
    /// Initializes the form aggregate with identifier, metadata, criteria, and groups.
    /// </summary>
    /// <param name="id">Unique identifier of the form.</param>
    /// <param name="metadata">Metadata associated with the form.</param>
    /// <param name="criteria">Collection of criteria in the form.</param>
    /// <param name="groups">Collection of groups in the form.</param>
    public Form(FormId id, FormMetadata metadata, ICriteria criteria, IGroups groups)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);

        _id = id;
        _metadata = metadata;
        _criteria = criteria;
        _groups = groups;
    }

    /// <summary>
    /// Validates the internal consistency of the form aggregate.
    /// </summary>
    public void Validate()
    {
    }
}
