using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Represents an immutable collection of form criteria for evaluation forms.
/// Provides serialization capabilities and ensures type safety for criterion management.
/// </summary>
public sealed record FormCriteriaList
{
    private readonly ImmutableList<FormCriterion> _criteria;

    /// <summary>
    /// Initializes a new instance of the FormCriteriaList with the specified criteria collection.
    /// </summary>
    /// <param name="criteria">The list of form criteria to encapsulate.</param>
    /// <exception cref="ArgumentNullException">Thrown when criteria is null.</exception>
    public FormCriteriaList(IList<FormCriterion> criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);

        _criteria = [.. criteria];
    }

    /// <summary>
    /// Extracts the identifiers from all form criteria in the collection.
    /// </summary>
    /// <returns>An immutable list containing the identifiers of all form criteria.</returns>
    public IImmutableList<FormCriterionId> Ids()
    {
        return _criteria.Select(c => c.Id).ToImmutableList();
    }

    internal IReadOnlyList<FormCriterion> Items()
    {
        return _criteria;
    }

    /// <summary>
    /// Returns the number of criteria in the collection.
    /// </summary>
    /// <returns>The count of form criteria.</returns>
    public int Count()
    {
        return _criteria.Count;
    }
}
