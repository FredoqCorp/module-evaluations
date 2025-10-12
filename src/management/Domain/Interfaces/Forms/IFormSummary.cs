using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Behavioral contract for a form summary that provides essential form information.
/// </summary>
public interface IFormSummary
{
    /// <summary>
    /// Prints the form summary representation into the provided media.
    /// </summary>
    /// <param name="media">Target media that receives the printed representation.</param>
    void Print(IMedia media);
}
