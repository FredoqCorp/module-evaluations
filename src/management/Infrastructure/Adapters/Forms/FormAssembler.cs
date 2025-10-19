using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Weighted;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Average;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Weighted;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using System.Globalization;
using System.Text.Json;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;

/// <summary>
/// Assembles domain form objects from database rows.
/// </summary>
internal static class FormAssembler
{
    /// <summary>
    /// Assembles form summary from optimized row with counts.
    /// </summary>
    public static FormSummary AssembleSummaryFromRow(FormSummaryRow row)
    {
        var id = new FormId(row.Id);
        var name = new FormName(row.Name);
        var description = new FormDescription(row.Description ?? string.Empty);
        var code = new FormCode(row.Code);

        var tagStrings = JsonSerializer.Deserialize<List<string>>(row.Tags) ?? [];
        var tagObjects = tagStrings.Select(t => new Tag(t));
        var tags = new Tags(tagObjects);

        var metadata = new FormMetadata(name, description, code, tags);

        var calculationType = row.RootGroupType.ToUpperInvariant() switch
        {
            "AVERAGE" => CalculationType.Average,
            "WEIGHTED" => CalculationType.WeightedAverage,
            _ => throw new InvalidOperationException($"Unknown root group type: {row.RootGroupType}")
        };

        return new FormSummary(id, metadata, calculationType, row.GroupsCount, row.CriteriaCount);
    }

    /// <summary>
    /// Assembles form summary with metadata and structural statistics.
    /// </summary>
    public static FormSummary AssembleSummary(
        FormRow formRow,
        List<GroupRow> groupRows,
        List<CriterionRow> criteriaRows)
    {
        var id = new FormId(formRow.Id);
        var metadata = BuildMetadata(formRow);

        var calculationType = formRow.RootGroupType.ToUpperInvariant() switch
        {
            "AVERAGE" => CalculationType.Average,
            "WEIGHTED" => CalculationType.WeightedAverage,
            _ => throw new InvalidOperationException($"Unknown root group type: {formRow.RootGroupType}")
        };

        var groupsCount = groupRows.Count;
        var criteriaCount = criteriaRows.Count;

        return new FormSummary(id, metadata, calculationType, groupsCount, criteriaCount);
    }

    /// <summary>
    /// Assembles complete form with all nested structures.
    /// </summary>
    public static Form Assemble(
        FormRow formRow,
        List<GroupRow> groupRows,
        List<CriterionRow> criteriaRows)
    {
        var id = new FormId(formRow.Id);
        var metadata = BuildMetadata(formRow);

        IFormRootGroup rootGroup = formRow.RootGroupType.ToUpperInvariant() switch
        {
            "AVERAGE" => BuildAverageRoot(groupRows, criteriaRows),
            "WEIGHTED" => BuildWeightedRoot(groupRows, criteriaRows),
            _ => throw new InvalidOperationException($"Unknown root group type: {formRow.RootGroupType}")
        };

        return new Form(id, metadata, rootGroup);
    }

    private static FormMetadata BuildMetadata(FormRow formRow)
    {
        var name = new FormName(formRow.Name);
        var description = new FormDescription(formRow.Description ?? string.Empty);
        var code = new FormCode(formRow.Code);

        var tagStrings = JsonSerializer.Deserialize<List<string>>(formRow.Tags) ?? [];
        var tagObjects = tagStrings.Select(t => new Tag(t));
        var tags = new Tags(tagObjects);

        return new FormMetadata(name, description, code, tags);
    }

    private static AverageRootGroup BuildAverageRoot(
        List<GroupRow> groupRows,
        List<CriterionRow> criteriaRows)
    {
        var rootCriteria = criteriaRows
            .Where(c => c.GroupId == null && c.CriterionType.Equals("average", StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.OrderIndex)
            .Select(BuildAverageCriterion)
            .ToList();

        var rootGroups = groupRows
            .Where(g => g.ParentId == null && g.GroupType.Equals("average", StringComparison.OrdinalIgnoreCase))
            .OrderBy(g => g.OrderIndex)
            .Select(g => BuildAverageGroup(g, groupRows, criteriaRows))
            .ToList();

        return new AverageRootGroup(
            new AverageCriteria(rootCriteria),
            new AverageGroups(rootGroups)
        );
    }

    private static WeightedRootGroup BuildWeightedRoot(
        List<GroupRow> groupRows,
        List<CriterionRow> criteriaRows)
    {
        var rootCriteria = criteriaRows
            .Where(c => c.GroupId == null && c.CriterionType.Equals("weighted", StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.OrderIndex)
            .Select(BuildWeightedCriterion)
            .ToList();

        var rootGroups = groupRows
            .Where(g => g.ParentId == null && g.GroupType.Equals("weighted", StringComparison.OrdinalIgnoreCase))
            .OrderBy(g => g.OrderIndex)
            .Select(g => BuildWeightedGroup(g, groupRows, criteriaRows))
            .ToList();

        return new WeightedRootGroup(
            new WeightedCriteria(rootCriteria),
            new WeightedGroups(rootGroups)
        );
    }

    private static Criterion BuildAverageCriterion(CriterionRow row)
    {
        var id = new CriterionId(row.Id);
        var text = new CriterionText(row.Text);
        var title = new CriterionTitle(row.Title);

        var ratingOptions = BuildRatings(row.RatingOptions);

        return new Criterion(id, text, title, ratingOptions);
    }

    private static WeightedCriterion BuildWeightedCriterion(CriterionRow row)
    {
        var criterion = BuildAverageCriterion(row);
        var weight = new Weight(row.WeightBasisPoints ?? 0);

        return new WeightedCriterion(criterion, weight);
    }

    private static AverageCriterionGroup BuildAverageGroup(
        GroupRow groupRow,
        List<GroupRow> allGroups,
        List<CriterionRow> allCriteria)
    {
        var profile = BuildProfile(groupRow);

        var childCriteria = allCriteria
            .Where(c => c.GroupId == groupRow.Id && c.CriterionType.Equals("average", StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.OrderIndex)
            .Select(BuildAverageCriterion)
            .ToList();

        var childGroups = allGroups
            .Where(g => g.ParentId == groupRow.Id && g.GroupType.Equals("average", StringComparison.OrdinalIgnoreCase))
            .OrderBy(g => g.OrderIndex)
            .Select(g => BuildAverageGroup(g, allGroups, allCriteria))
            .ToList();

        return new AverageCriterionGroup(
            profile,
            new AverageCriteria(childCriteria),
            new AverageGroups(childGroups)
        );
    }

    private static WeightedCriterionGroup BuildWeightedGroup(
        GroupRow groupRow,
        List<GroupRow> allGroups,
        List<CriterionRow> allCriteria)
    {
        var profile = BuildProfile(groupRow);
        var weight = new Weight(groupRow.WeightBasisPoints ?? 0);

        var childCriteria = allCriteria
            .Where(c => c.GroupId == groupRow.Id && c.CriterionType.Equals("weighted", StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.OrderIndex)
            .Select(BuildWeightedCriterion)
            .ToList();

        var childGroups = allGroups
            .Where(g => g.ParentId == groupRow.Id && g.GroupType.Equals("weighted", StringComparison.OrdinalIgnoreCase))
            .OrderBy(g => g.OrderIndex)
            .Select(g => BuildWeightedGroup(g, allGroups, allCriteria))
            .ToList();

        return new WeightedCriterionGroup(
            profile,
            new WeightedCriteria(childCriteria),
            new WeightedGroups(childGroups),
            weight
        );
    }

    private static GroupProfile BuildProfile(GroupRow row)
    {
        var id = new GroupId(row.Id);
        var title = new GroupTitle(row.Title);
        var description = new GroupDescription(row.Description ?? string.Empty);

        return new GroupProfile(id, title, description);
    }

    private static RatingOptions BuildRatings(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidOperationException("Criterion rating options payload is missing");
        }

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        if (root.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Criterion rating options payload must be a JSON object keyed by order index");
        }

        var indexedOptions = new List<(int Index, RatingOption Option)>();
        foreach (var property in root.EnumerateObject())
        {
            var index = ParseIndex(property.Name);
            var option = BuildRatingOption(property.Value);
            indexedOptions.Add((index, option));
        }

        var orderedOptions = indexedOptions
            .OrderBy(tuple => tuple.Index)
            .Select(tuple => tuple.Option)
            .ToList();

        return new RatingOptions(orderedOptions);
    }

    private static int ParseIndex(string key)
    {
        if (!int.TryParse(key, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index))
        {
            throw new InvalidOperationException($"Criterion rating option index {key} must be an integer");
        }

        return index;
    }

    private static RatingOption BuildRatingOption(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Criterion rating option must be described by a JSON object");
        }

        if (!element.TryGetProperty("score", out var scoreElement))
        {
            throw new InvalidOperationException("Criterion rating option must contain score");
        }

        var scoreValue = ParseScore(scoreElement);

        if (!element.TryGetProperty("label", out var labelElement))
        {
            throw new InvalidOperationException("Criterion rating option must contain label");
        }

        var labelValue = labelElement.GetString();
        if (string.IsNullOrWhiteSpace(labelValue))
        {
            throw new InvalidOperationException("Criterion rating option label cannot be empty");
        }

        var annotationValue = string.Empty;
        if (element.TryGetProperty("annotation", out var annotationElement) && annotationElement.ValueKind == JsonValueKind.String)
        {
            annotationValue = annotationElement.GetString() ?? string.Empty;
        }

        return new RatingOption(
            new RatingScore(scoreValue),
            new RatingLabel(labelValue),
            new RatingAnnotation(annotationValue));
    }

    private static ushort ParseScore(JsonElement scoreElement)
    {
        if (scoreElement.ValueKind == JsonValueKind.Number)
        {
            if (scoreElement.TryGetUInt16(out var direct))
            {
                return direct;
            }

            var decimalScore = scoreElement.GetDecimal();
            return Convert.ToUInt16(decimalScore);
        }

        if (scoreElement.ValueKind == JsonValueKind.String)
        {
            var text = scoreElement.GetString();
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException("Criterion rating option score cannot be empty");
            }

            if (!ushort.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                throw new InvalidOperationException($"Criterion rating option score {text} is not a valid positive integer");
            }

            return parsed;
        }

        throw new InvalidOperationException("Criterion rating option score must be numeric");
    }
}
