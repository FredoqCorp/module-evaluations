namespace CascVel.Modules.Evaluations.Management.Infrastructure.Queries;

/// <summary>
/// PostgreSQL queries for loading evaluation forms.
/// </summary>
internal static class FormQueries
{
    /// <summary>
    /// Optimized query to load form summaries with counts.
    /// </summary>
    internal const string LoadFormSummaries = @"
        SELECT
            f.id AS Id,
            f.name AS Name,
            f.description AS Description,
            f.code AS Code,
            f.tags AS Tags,
            f.root_group_type AS RootGroupType,
            COUNT(DISTINCT fg.id) AS GroupsCount,
            COUNT(DISTINCT fc.id) AS CriteriaCount
        FROM forms f
        LEFT JOIN form_groups fg ON f.id = fg.form_id
        LEFT JOIN form_criteria fc ON f.id = fc.form_id
        GROUP BY f.id, f.name, f.description, f.code, f.tags, f.root_group_type, f.created_at
        ORDER BY f.created_at DESC";
}
