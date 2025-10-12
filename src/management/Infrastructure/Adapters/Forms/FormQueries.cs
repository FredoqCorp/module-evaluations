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
            order_index AS OrderIndex
        FROM form_criteria
        ORDER BY form_id, order_index";

    /// <summary>
    /// Query to load all rating options for criteria.
    /// </summary>
    internal const string LoadRatings = @"
        SELECT
            id AS Id,
            criterion_id AS CriterionId,
            score AS Score,
            label AS Label,
            annotation AS Annotation,
            order_index AS OrderIndex
        FROM rating_options
        ORDER BY criterion_id, order_index";
}
