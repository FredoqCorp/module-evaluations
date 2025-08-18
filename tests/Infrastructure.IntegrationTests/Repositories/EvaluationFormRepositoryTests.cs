using CascVel.Module.Evaluations.Management.Application.Interfaces;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.Calculation;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;
using CascVel.Module.Evaluations.Management.Infrastructure.Repositories;
using Infrastructure.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CascVel.Module.Evaluations.Management.Infrastructure.IntegrationTests.Repositories;

/// <summary>
/// Integration tests for EvaluationFormRepository using real PostgreSQL via Testcontainers and EF Core migrations.
/// </summary>
[Collection("postgres-db")]
public sealed class EvaluationFormRepositoryTests
{
    private readonly PostgresFixture _fx;

    /// <summary>
    /// Initializes a new instance of the <see cref="EvaluationFormRepositoryTests"/> class with the provided PostgresFixture.
    /// </summary>
    /// <param name="fx">The PostgresFixture used to set up the test database context.</param>
    public EvaluationFormRepositoryTests(PostgresFixture fx)
    {
        _fx = fx;
    }

    /// <summary>
    /// Verifies that creating and retrieving an evaluation form returns the same aggregate instance.
    /// Usage: This test creates a form, saves it, and asserts that the loaded form has the same code.
    /// </summary>
    [Fact(DisplayName = "Create and get evaluation form returns the same aggregate" )]
    public async Task Create_and_get_evaluation_form_returns_the_same_aggregate()
    {
        await _fx.ResetAsync();
        await using var scope = _fx.Services.CreateAsyncScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<EvaluationFormRepository>>();
        IEvaluationFormRepository repo = new EvaluationFormRepository(factory, logger);
        var now = DateTime.UtcNow;
        var form = new EvaluationForm
        {
            Meta = new FormMeta
            {
                Name = new FormName { Value = "Тестовое имя Ω" },
                Description = "Описание ❤",
                Tags = new List<string> { "A", "b" },
                Code = new FormCode { Value = Guid.NewGuid().ToString("N") },
            },
            Lifecycle = new FormLifecycle
            {
                Status = FormStatus.Draft,
                Validity = new Period { Start = now, End = null },
                Audit = new AuditTrail
                {
                    Created = new Stamp { UserId = "u-ξ", At = now },
                },
            },
            Calculation = FormCalculationKind.WeightedMean,
            Groups = [],
            Criteria = [],
        };
        long id = await repo.CreateAsync(form);
        var loaded = await repo.GetAsync(id, isFullInclude: true);
        Assert.True(string.Equals(loaded.Meta.Code.Value, form.Meta.Code.Value, StringComparison.Ordinal), "Loaded form code is different which is a failure");
    }

    /// <summary>
    /// Verifies that groups and criteria added to a form are persisted and returned when loading the full graph
    /// </summary>
    [Fact(DisplayName = "Create form with groups and criteria then get returns full graph" )]
    public async Task Create_form_with_groups_and_criteria_then_get_returns_full_graph()
    {
        await _fx.ResetAsync();
        await using var scope = _fx.Services.CreateAsyncScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<EvaluationFormRepository>>();
        var repo = new EvaluationFormRepository(factory, logger);
        DateTime now = DateTime.UtcNow;
        string titleTop = $"Критерий ∑ {Guid.NewGuid():N}";
        string titleGrouped = $"Критерий ◎ {Guid.NewGuid():N}";
        string titleGroup = $"Группа Ω {Guid.NewGuid():N}";
        Criterion topCriterion = MakeCriterion(titleTop, "описание ✿");
        Criterion groupedCriterion = MakeCriterion(titleGrouped, "описание ⊕");
        EvaluationForm form = MakeForm(now, titleGroup, topCriterion, groupedCriterion);
        long id = await repo.CreateAsync(form);
        var loaded = await repo.GetAsync(id, isFullInclude: true);
        bool hasTop = loaded.Criteria.Any(c => string.Equals(c.Criterion.Text.Title.Value, titleTop, StringComparison.Ordinal));
        bool hasGroup = loaded.Groups.Any(g => string.Equals(g.Title, titleGroup, StringComparison.Ordinal) && g.Criteria.Any(fc => string.Equals(fc.Criterion.Text.Title.Value, titleGrouped, StringComparison.Ordinal)));
        Assert.True(hasTop && hasGroup, "Form graph does not contain expected group and criteria which is a failure");
    }

    /// <summary>
    /// Builds a domain Criterion with minimal required data for persistence.
    /// </summary>
    private static Criterion MakeCriterion(string title, string description)
    {
        return new Criterion
        {
            Text = new CriterionText
            {
                Title = new CriterionTitle { Value = title },
                Description = new CriterionDescription { Value = description },
            },
            Options = Array.Empty<Option>(),
            Automation = null,
        };
    }

    /// <summary>
    /// Builds an EvaluationForm with one top-level criterion and one group with a criterion.
    /// </summary>
    private static EvaluationForm MakeForm(DateTime now, string groupTitle, Criterion top, Criterion inside)
    {
        return new EvaluationForm
        {
            Meta = new FormMeta
            {
                Name = new FormName { Value = "Форма с группами Ψ" },
                Description = "Проверка структуры ⚑",
                Tags = ["grp", "crit"],
                Code = new FormCode { Value = Guid.NewGuid().ToString("N") },
            },
            Lifecycle = new FormLifecycle
            {
                Status = FormStatus.Draft,
                Validity = new Period { Start = now, End = null },
                Audit = new AuditTrail { Created = new Stamp { UserId = "u-ζ", At = now } },
            },
            Calculation = FormCalculationKind.WeightedMean,
            Criteria =
            [
                new FormCriterion { Id = 0, Criterion = top, Order = new OrderIndex { Value = 0 }, Weight = null },
            ],
            Groups =
            [
                new FormGroup
                {
                    Title = groupTitle,
                    Order = new OrderIndex { Value = 0 },
                    Weight = null,
                    Criteria =
                    [
                        new FormCriterion { Id = 0, Criterion = inside, Order = new OrderIndex { Value = 0 }, Weight = null },
                    ],
                    Groups = [],
                },
            ],
        };
    }
}
