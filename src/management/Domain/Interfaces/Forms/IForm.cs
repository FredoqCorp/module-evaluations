using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Behavioral contract for a form aggregate that manages lifecycle and metadata.
/// </summary>
public interface IForm
{
    /// <summary>
    /// Validates the internal consistency of the form aggregate.
    /// </summary>
    void Validate();

    /// <summary>
    /// Prints the form aggregate into the provided media for serialization or persistence.
    /// </summary>
    /// <param name="media">Media that receives the structured representation.</param>
    void Print(IMedia media);
}
