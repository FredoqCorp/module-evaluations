namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Unique identifier for an evaluation form.
/// </summary>
public readonly record struct FormId
{
    /// <summary>
    /// Creates form identifier ensuring it is not empty.
    /// </summary>
    /// <param name="value">Non-empty GUID representing the form.</param>
    /// <exception cref="ArgumentException">Thrown when value is empty.</exception>
    public FormId(Guid value)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(value, Guid.Empty);

        Value = value;
    }
    
    /// <summary>
    /// Creates a new form identifier with a newly generated GUID.
    /// </summary>
    public FormId() : this(Guid.CreateVersion7())
    {
    }

    /// <summary>
    /// Underlying GUID value representing the form identifier.
    /// </summary>
    public Guid Value { get; init; }
}
