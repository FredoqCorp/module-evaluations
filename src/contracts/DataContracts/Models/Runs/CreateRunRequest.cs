using System.ComponentModel.DataAnnotations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs;

/// <summary>
/// Represents a request to create a new run for a specific form
/// </summary>
/// <param name="FormId">Identifier of the form to run</param>
/// <param name="RunFor">Login of the operator evaluated</param>
/// <param name="Context">Additional run context as key value pairs</param>
public sealed record CreateRunRequest(
    [Required] long FormId,
    [Required] string RunFor,
    [Required] IReadOnlyDictionary<string, string> Context
);
