namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;

/// <summary>
/// PostgreSQL queries for loading evaluation forms.
/// </summary>
internal static class FormQueries
{
    /// <summary>
    /// Query to load all forms ordered by creation date.
    /// </summary>
    internal const string LoadForms = @"
        SELECT
            id AS Id,
            name AS Name,
            description AS Description,
            code AS Code,
            tags AS Tags,
            root_group_type AS RootGroupType
        FROM forms
        ORDER BY created_at DESC";

    /// <summary>
    /// Query to load all groups with their hierarchy information.
    /// </summary>
    internal const string LoadGroups = @"
        SELECT
            id AS Id,
            form_id AS FormId,
            parent_id AS ParentId,
            title AS Title,
            description AS Description,
            group_type AS GroupType,
            weight_basis_points AS WeightBasisPoints,
            order_index AS OrderIndex
        FROM form_groups
        ORDER BY form_id, order_index";

    /// <summary>
    /// Query to load all criteria with their type and weight information.
    /// </summary>
    internal const string LoadCriteria = @"
        SELECT
            id AS Id,
            form_id AS FormId,
            group_id AS GroupId,
            title AS Title,
            text AS Text,
            criterion_type AS CriterionType,
            weight_basis_points AS WeightBasisPoints,
            order_index AS OrderIndex,
            rating_options AS RatingOptions
        FROM form_criteria
        ORDER BY form_id, order_index";

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
