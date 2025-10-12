using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Weighted;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Average;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Weighted;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using System.Text.Json;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;

/// <summary>
/// Assembles domain form objects from database rows.
/// </summary>
internal sealed class FormAssembler
{
    private readonly Dictionary<Guid, List<RatingOptionRow>> _ratingsByCriterionId;

    public FormAssembler(Dictionary<Guid, List<RatingOptionRow>> ratingsByCriterionId)
    {
        _ratingsByCriterionId = ratingsByCriterionId ?? throw new ArgumentNullException(nameof(ratingsByCriterionId));
    }

    /// <summary>
    /// Assembles complete form with all nested structures.
    /// </summary>
    public Form Assemble(
        FormRow formRow,
        List<GroupRow> groupRows,
        List<CriterionRow> criteriaRows)
    {
        var id = new FormId(formRow.Id);
        var name = new FormName(formRow.Name);
        var description = new FormDescription(formRow.Description ?? string.Empty);
        var code = new FormCode(formRow.Code);

        var tagStrings = JsonSerializer.Deserialize<List<string>>(formRow.Tags) ?? [];
        var tagObjects = tagStrings.Select(t => new Tag(t));
        var tags = new Tags(tagObjects);

        var metadata = new FormMetadata(name, description, code, tags);

        IFormRootGroup rootGroup = formRow.RootGroupType.ToUpperInvariant() switch
        {
            "AVERAGE" => BuildAverageRoot(groupRows, criteriaRows),
            "WEIGHTED" => BuildWeightedRoot(groupRows, criteriaRows),
            _ => throw new InvalidOperationException($"Unknown root group type: {formRow.RootGroupType}")
        };

        return new Form(id, metadata, rootGroup);
    }

    private AverageRootGroup BuildAverageRoot(
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

    private WeightedRootGroup BuildWeightedRoot(
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

    private Criterion BuildAverageCriterion(CriterionRow row)
    {
        var id = new CriterionId(row.Id);
        var text = new CriterionText(row.Text);
        var title = new CriterionTitle(row.Title);

        var ratingRows = _ratingsByCriterionId.GetValueOrDefault(row.Id, []);
        var ratingOptions = BuildRatings(ratingRows);

        return new Criterion(id, text, title, ratingOptions);
    }

    private WeightedCriterion BuildWeightedCriterion(CriterionRow row)
    {
        var criterion = BuildAverageCriterion(row);
        var weight = new Weight(row.WeightBasisPoints ?? 0);

        return new WeightedCriterion(criterion, weight);
    }

    private AverageCriterionGroup BuildAverageGroup(
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

    private WeightedCriterionGroup BuildWeightedGroup(
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

    private static RatingOptions BuildRatings(List<RatingOptionRow> rows)
    {
        var options = rows
            .OrderBy(r => r.OrderIndex)
            .Select(r => new RatingOption(
                new RatingScore((ushort)Math.Round(r.Score)),
                new RatingLabel(r.Label),
                new RatingAnnotation(r.Annotation ?? string.Empty)
            ))
            .ToList();

        return new RatingOptions(options);
    }
}
