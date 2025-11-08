using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable value object that preserves the textual code of a form.
/// </summary>
public sealed record FormCode : IFormCode
{
    private readonly string _token;

    /// <summary>
    /// Initializes the value object with the provided token after validating basic integrity.
    /// </summary>
    /// <param name="token">Plain code token supplied by the caller.</param>
    public FormCode(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        _token = token;
    }

    /// <summary>
    /// Reads the textual form code.
    /// </summary>
    /// <returns>Form code token.</returns>
    public string Text() => _token;
}
