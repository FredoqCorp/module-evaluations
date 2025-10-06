namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a form aggregate that manages lifecycle and metadata.
/// </summary>
public interface IForm
{
    /// <summary>
    /// Validates the internal consistency of the form aggregate.
    /// </summary>
    void Validate();
}
