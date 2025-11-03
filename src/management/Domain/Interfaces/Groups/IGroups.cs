using System;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Interface for Groups
/// </summary>
public interface IGroups
{
    /// <summary>
    /// Prints the groups contained in the aggregate into the provided media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Target media that receives the printed representation.</param>
    /// <param name="formId">Identifier of the owning form when the groups are at root level.</param>
    /// <param name="parentGroupId">Identifier of the parent group when nested.</param>
    /// <returns>The media instance that received the printed representation.</returns>
    IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media, Guid formId, Guid? parentGroupId);
}
