namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Provides access to the textual description of a form.
/// </summary>
public interface IFormDescription
{
    /// <summary>
    /// Reads the description text.
    /// </summary>
    /// <returns>Form description text.</returns>
    string Text();
}
