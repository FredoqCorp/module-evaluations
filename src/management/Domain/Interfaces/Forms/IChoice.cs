namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Contract for a selectable option that carries a discrete score used in evaluation.
/// </summary>
public interface IChoice
{
    /// <summary>
    /// Returns the raw score value without applying any range validation.
    /// </summary>
    ushort Score();

    /// <summary>
    /// Returns the optional human friendly caption or an empty string when absent.
    /// </summary>
    string Caption();

    /// <summary>
    /// Returns the optional annotation or an empty string when absent.
    /// </summary>
    string Annotation();
}
