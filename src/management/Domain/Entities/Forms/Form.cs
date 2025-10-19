using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Immutable aggregate root that represents an evaluation form.
/// </summary>
public sealed class Form : IForm
{
    private readonly FormId _id;
    private readonly FormMetadata _metadata;
    private readonly IFormRootGroup _root;

    /// <summary>
    /// Initializes the form aggregate with identifier, metadata, and root group.
    /// </summary>
    /// <param name="id">Unique identifier of the form.</param>
    /// <param name="metadata">Metadata associated with the form.</param>
    /// <param name="root">Root group representing the form structure.</param>
    public Form(FormId id, FormMetadata metadata, IFormRootGroup root)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(root);

        _id = id;
        _metadata = metadata;
        _root = root;
    }

    /// <summary>
    /// Validates the internal consistency of the form aggregate.
    /// </summary>
    public void Validate()
    {
        _root.Validate();
    }
}
