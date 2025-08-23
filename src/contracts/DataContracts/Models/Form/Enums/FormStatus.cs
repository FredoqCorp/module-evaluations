using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Enums;

/// <summary>
/// Current form status
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FormStatus
{
    /// <summary>
    /// Draft
    /// </summary>
    Draft,

    /// <summary>
    /// Active
    /// </summary>
    Published,

    /// <summary>
    /// Archived
    /// </summary>
    Archived,
}
