using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

/// <summary>
/// Behavioral contract for a criteria collection.
/// </summary>
public interface ICriteria
{
    /// <summary>
    /// Prints the criteria contained in the collection into the provided media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Target media that receives the printed representation.</param>
    /// <param name="formId">Identifier of the owning form when the criteria are at root level.</param>
    /// <param name="groupId">Identifier of the owning group when the criteria are nested.</param>
    /// <returns>The media instance that received the printed representation.</returns>
    IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media, Guid formId, Guid? groupId);
}
