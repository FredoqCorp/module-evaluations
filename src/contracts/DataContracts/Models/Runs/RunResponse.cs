using CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs.Enums;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs.Evaluations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs;

/// <summary>
/// Represents a response model for a created or retrieved run including a snapshot of the form and audit information
/// </summary>
/// <param name="Id">Run identifier</param>
/// <param name="FormSnapshot">Form snapshot with groups and criteria captured at the time of the run</param>
/// <param name="Context">Run specific context</param>
/// <param name="Evaluations">Per-criterion evaluations for this run</param>
/// <param name="CreatedAt">Date of run creation</param>
/// <param name="CreatedBy">User who created the run</param>
/// <param name="FirstSavedAt">Date of first save</param>
/// <param name="FirstSavedBy">User who saved first</param>
/// <param name="LastSavedAt">Date of last save</param>
/// <param name="LastSavedBy">User who saved last</param>
/// <param name="PublishedAt">Date of publication</param>
/// <param name="PublishedBy">User who published</param>
/// <param name="ScoreResult">Calculated score</param>
/// <param name="ViewedAt">When the operator viewed the evaluation</param>
/// <param name="AgreementStatus">Agreement status</param>
/// <param name="AgreementAt">Date of agreement status change</param>
public sealed record RunResponse(
    long Id,
    FormSnapshotResponse FormSnapshot,
    IReadOnlyDictionary<string, string> Context,
    IReadOnlyList<CriterionEvaluationResponse> Evaluations,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? FirstSavedAt,
    string? FirstSavedBy,
    DateTime? LastSavedAt,
    string? LastSavedBy,
    DateTime? PublishedAt,
    string? PublishedBy,
    decimal? ScoreResult,
    DateTime? ViewedAt,
    AgreementStatus? AgreementStatus,
    DateTime? AgreementAt
);
