using System.Globalization;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;
using Shouldly;
using Xunit;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Media;

/// <summary>
/// Unit-style tests for PgInsertFormMedia script generation.
/// </summary>
public sealed class PgInsertFormMediaTests
{
    [Fact]
    public async Task Given_weighted_snapshot_When_output_runs_Then_contains_weight_fragments()
    {
        var stamp = DateTimeOffset.UtcNow.AddMilliseconds(RandomNumberGenerator.GetInt32(1, 500));
        var formId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var criterionId = Guid.NewGuid();
        var name = $"Имя-{Guid.NewGuid():N}";
        var description = $"описание-{Guid.NewGuid():N}";
        var code = $"код-{Guid.NewGuid():N}";
        var groupTitle = $"Grupo-{Guid.NewGuid():N}";
        var groupDescription = $"Detalhe-{Guid.NewGuid():N}";
        var criterionTitle = $"Critério-{Guid.NewGuid():N}";
        var criterionText = $"Texto-{Guid.NewGuid():N}";
        var ratingOptions = "{\"0\":{\"score\":4.5,\"label\":\"高\",\"annotation\":\"♛\"}}";
        const int expectedGroupWeight = 3200;
        const int expectedCriterionWeight = 6800;

        var media = new PgInsertFormMedia(stamp);
        media.With("formId", formId);
        media.With("name", name);
        media.With("description", description);
        media.With("code", code);
        media.With("calculation", "weighted");
        media.With("tags", [$"тег-{Guid.NewGuid():N}", $"标签-{Guid.NewGuid():N}"]);
        media.WithObject("group", inner =>
        {
            inner.With("id", groupId);
            inner.With("formId", formId);
            inner.With("title", groupTitle);
            inner.With("description", groupDescription);
            inner.With("groupType", "weighted");
            inner.With("orderIndex", 0);
            inner.With("weightBasisPoints", expectedGroupWeight);
        });
        media.WithObject("criterion", inner =>
        {
            inner.With("id", criterionId);
            inner.With("groupId", groupId);
            inner.With("title", criterionTitle);
            inner.With("text", criterionText);
            inner.With("criterionType", "weighted");
            inner.With("orderIndex", 0);
            inner.With("ratingOptions", ratingOptions);
            inner.With("weightBasisPoints", expectedCriterionWeight);
        });

        var scriptTask = Task.Run(media.Output);
        var script = await scriptTask;
        var success = script.Contains("INSERT INTO forms", StringComparison.Ordinal) &&
                      script.Contains(formId.ToString("D", CultureInfo.InvariantCulture), StringComparison.Ordinal) &&
                      script.Contains(expectedGroupWeight.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) &&
                      script.Contains(expectedCriterionWeight.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) &&
                      script.Contains("INSERT INTO form_criteria", StringComparison.Ordinal);
        success.ShouldBeTrue("Generated script skipped expected weighted fragments");
    }

    [Fact]
    public async Task Given_average_snapshot_When_output_runs_Then_omits_weight_fragments()
    {
        var stamp = DateTimeOffset.UtcNow.AddMilliseconds(RandomNumberGenerator.GetInt32(1, 500));
        var formId = Guid.NewGuid();
        var criterionId = Guid.NewGuid();
        var name = $"Имя-{Guid.NewGuid():N}";
        var description = $"описание-{Guid.NewGuid():N}";
        var code = $"код-{Guid.NewGuid():N}";
        var criterionTitle = $"Critère-{Guid.NewGuid():N}";
        var criterionText = $"Texto-{Guid.NewGuid():N}";
        var ratingOptions = "{\"0\":{\"score\":3,\"label\":\"средне\",\"annotation\":\"ø\"}}";

        var media = new PgInsertFormMedia(stamp);
        media.With("formId", formId);
        media.With("name", name);
        media.With("description", description);
        media.With("code", code);
        media.With("calculation", "average");
        media.With("tags", new[] { $"тег-{Guid.NewGuid():N}" });
        media.WithObject("criterion", inner =>
        {
            inner.With("id", criterionId);
            inner.With("formId", formId);
            inner.With("title", criterionTitle);
            inner.With("text", criterionText);
            inner.With("criterionType", "average");
            inner.With("orderIndex", 0);
            inner.With("ratingOptions", ratingOptions);
        });

        var scriptTask = Task.Run(media.Output);
        var script = await scriptTask;
        var success = script.Contains("INSERT INTO forms", StringComparison.Ordinal) &&
                      script.Contains("average", StringComparison.Ordinal) &&
                      script.Contains("INSERT INTO form_criteria", StringComparison.Ordinal) &&
                      !script.Contains("weight_basis_points, 0", StringComparison.Ordinal);
        success.ShouldBeTrue("Average script unexpectedly referenced weights");
    }
}
