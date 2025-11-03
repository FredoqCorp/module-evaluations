using CascVel.Modules.Evaluations.Management.Domain.Models.Validity;

namespace CascVel.Modules.Evaluations.Management.Domain.Exceptions;

/// <summary>
/// Exception thrown when validity period boundaries break chronological order.
/// </summary>
public sealed class ValidityPeriodRangeException : Exception
{
    /// <summary>
    /// Creates an exception with a default message.
    /// </summary>
    public ValidityPeriodRangeException() : base("Validity end cannot precede start")
    {
    }

    /// <summary>
    /// Creates an exception carrying the offending boundaries.
    /// </summary>
    /// <param name="start">Validity period start that was provided.</param>
    /// <param name="end">Validity period end that precedes the start.</param>
    public ValidityPeriodRangeException(ValidityStart start, ValidityEnd end)
        : base($"Validity end {end.Value:O} precedes start {start.Value:O}")
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Creates an exception with a custom message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ValidityPeriodRangeException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates an exception with a custom message and an inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ValidityPeriodRangeException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Validity period start that triggered the exception.
    /// </summary>
    public ValidityStart? Start { get; }

    /// <summary>
    /// Validity period end that triggered the exception.
    /// </summary>
    public ValidityEnd? End { get; }
}
