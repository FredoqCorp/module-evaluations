namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// An option representing a selectable score and optional annotations for a criterion.
/// </summary>
public sealed record Choise
{
    /// <summary>
    /// Score value associated with the option (e.g., 1..5).
    /// </summary>
    public required ushort Score { get; init; }

    /// <summary>
    /// Optional human-friendly caption (e.g., "Yes" / "No").
    /// </summary>
    public string? Caption { get; init; }

    /// <summary>
    /// Optional annotation/notes for the option.
    /// </summary>
    public string? Annotation { get; init; }

    /// <summary>
    /// Optional boundary for automatic selection.
    /// For Goal=Minimize: value ≤ Threshold selects this option.
    /// For Goal=Maximize: value ≥ Threshold selects this option.
    /// The fallback option should have no threshold.
    /// </summary>
    public decimal? Threshold { get; init; }
}
