using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Entities.Criteria;

/// <summary>
/// Tests for AverageCriteria Add methods.
/// </summary>
public sealed class AverageCriteriaAddTests
{
    [Fact]
    public async Task Add_WithFormId_ReturnsAverageCriterion()
    {
        // Arrange
        var criteria = new AverageCriteria([]);
        var criterionId = new CriterionId(Guid.NewGuid());
        var text = new CriterionText("Evaluate communication skills");
        var title = new CriterionTitle("Communication");
        var ratingOptions = CreateTestRatingOptions();
        var formId = new FormId(Guid.NewGuid());
        var orderIndex = new OrderIndex(0);

        // Act
        var result = await criteria.Add(criterionId, text, title, ratingOptions, formId, orderIndex);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Add_WithGroupId_ReturnsAverageCriterion()
    {
        // Arrange
        var criteria = new AverageCriteria([]);
        var criterionId = new CriterionId(Guid.NewGuid());
        var text = new CriterionText("Evaluate teamwork ability");
        var title = new CriterionTitle("Teamwork");
        var ratingOptions = CreateTestRatingOptions();
        var groupId = new GroupId(Guid.NewGuid());
        var orderIndex = new OrderIndex(1);

        // Act
        var result = await criteria.Add(criterionId, text, title, ratingOptions, groupId, orderIndex);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Add_WithFormId_ThrowsWhenRatingOptionsIsNull()
    {
        // Arrange
        var criteria = new AverageCriteria([]);
        var criterionId = new CriterionId(Guid.NewGuid());
        var text = new CriterionText("Test");
        var title = new CriterionTitle("Test");
        var formId = new FormId(Guid.NewGuid());
        var orderIndex = new OrderIndex(0);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            criteria.Add(criterionId, text, title, null!, formId, orderIndex));
    }

    [Fact]
    public async Task Add_WithGroupId_ThrowsWhenRatingOptionsIsNull()
    {
        // Arrange
        var criteria = new AverageCriteria([]);
        var criterionId = new CriterionId(Guid.NewGuid());
        var text = new CriterionText("Test");
        var title = new CriterionTitle("Test");
        var groupId = new GroupId(Guid.NewGuid());
        var orderIndex = new OrderIndex(0);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            criteria.Add(criterionId, text, title, null!, groupId, orderIndex));
    }

    private static RatingOptions CreateTestRatingOptions()
    {
        var options = new[]
        {
            new RatingOption(
                new RatingScore(5),
                new RatingLabel("Excellent"),
                new RatingAnnotation("Outstanding performance")),
            new RatingOption(
                new RatingScore(3),
                new RatingLabel("Good"),
                new RatingAnnotation("Meets expectations")),
            new RatingOption(
                new RatingScore(1),
                new RatingLabel("Needs Improvement"),
                new RatingAnnotation("Below expectations"))
        };

        return new RatingOptions(options);
    }
}
