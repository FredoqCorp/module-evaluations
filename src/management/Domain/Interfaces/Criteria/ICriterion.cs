using System;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

/// <summary>
/// Behavioral contract for a single criterion.
/// </summary>
public interface ICriterion
{
    /// <summary>
    /// Prints the criterion into the provided media within its parent context.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Target media that receives the printed representation.</param>
    /// <param name="formId">Identifier of the owning form when the criterion is at root level.</param>
    /// <param name="groupId">Identifier of the owning group when the criterion is nested.</param>
    /// <returns>The media instance that received the printed representation.</returns>
    IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media, Guid formId, Guid? groupId);
}
