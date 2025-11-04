namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Provides access to the code token of a form.
/// </summary>
public interface IFormCode
{
    /// <summary>
    /// Reads the code token as text.
    /// </summary>
    /// <returns>Form code token.</returns>
    string Text();
}
