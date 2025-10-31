using System;

namespace CascVel.Modules.Evaluations.Management.Host.Contracts;

/// <summary>
/// Response contract that exposes the identifier of the newly created form.
/// </summary>
public sealed record FormCreatedResponse
{
    /// <summary>
    /// Initializes the response with the created form identifier.
    /// </summary>
    /// <param name="id">Identifier of the created form.</param>
    public FormCreatedResponse(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Identifier assigned to the created form.
    /// </summary>
    public Guid Id { get; init; }
}
