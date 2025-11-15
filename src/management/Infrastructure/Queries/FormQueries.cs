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

    /// <summary>
    /// Query that loads a single form metadata record by identifier.
    /// </summary>
    internal const string LoadFormById = @"
        SELECT
            f.id AS Id,
            f.name AS Name,
            f.description AS Description,
            f.code AS Code,
            f.tags::text AS Tags,
            f.root_group_type AS RootGroupType
        FROM forms f
        WHERE f.id = @Id";

    /// <summary>
    /// Query that loads all groups for a specific form preserving hierarchical order.
    /// </summary>
    internal const string LoadFormGroups = @"
        SELECT
            fg.id AS Id,
            fg.parent_id AS ParentId,
            fg.title AS Title,
            fg.description AS Description,
            fg.weight_basis_points AS WeightBasisPoints,
            fg.order_index AS OrderIndex
        FROM form_groups fg
        WHERE fg.form_id = @Id
        ORDER BY fg.parent_id NULLS FIRST, fg.order_index ASC";

    /// <summary>
    /// Query that loads all root-level criteria for a specific form.
    /// </summary>
    internal const string LoadRootCriteria = @"
        SELECT
            fc.id AS Id,
            fc.title AS Title,
            fc.text AS Text,
            fc.weight_basis_points AS WeightBasisPoints,
            fc.order_index AS OrderIndex,
            fc.rating_options::text AS RatingOptions
        FROM form_criteria fc
        WHERE fc.form_id = @Id AND fc.group_id IS NULL
        ORDER BY fc.order_index ASC";

    /// <summary>
    /// Query that loads all grouped criteria for a form.
    /// </summary>
    internal const string LoadFormCriteriaTree = @"
        SELECT
            fc.id AS Id,
            fc.group_id AS GroupId,
            fc.title AS Title,
            fc.text AS Text,
            fc.weight_basis_points AS WeightBasisPoints,
            fc.order_index AS OrderIndex,
            fc.rating_options::text AS RatingOptions
        FROM form_criteria fc
        WHERE fc.form_id = @Id AND fc.group_id IS NOT NULL
        ORDER BY fc.group_id NULLS FIRST, fc.order_index ASC";
}
