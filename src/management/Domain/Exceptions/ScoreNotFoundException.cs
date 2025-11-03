using CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Exceptions;

/// <summary>
/// Exception thrown when a rating score is not found in the available options.
/// </summary>
public sealed class ScoreNotFoundException : Exception
{
    /// <summary>
    /// Creates an exception with a default message.
    /// </summary>
    public ScoreNotFoundException() : base("Rating score was not found in the available options.")
    {
    }

    /// <summary>
    /// Creates an exception for a score that was not found in the rating options.
    /// </summary>
    /// <param name="score">The score that was not found.</param>
    public ScoreNotFoundException(RatingScore score)
        : base($"Rating score {score.Value} was not found in the available options.")
    {
        Score = score;
    }

    /// <summary>
    /// Creates an exception with a custom message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ScoreNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates an exception with a custom message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ScoreNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// The score that was not found.
    /// </summary>
    public RatingScore? Score { get; }
}
