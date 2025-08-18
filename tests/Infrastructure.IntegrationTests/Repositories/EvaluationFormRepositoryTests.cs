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
    /// Verifies full integrity of meta, lifecycle and calculation after persistence and retrieval
    /// </summary>
    [Fact(DisplayName = "Create form then get preserves meta lifecycle and calculation integrity")]
    public async Task Create_form_then_get_preserves_meta_lifecycle_and_calculation_integrity()
    {
        await _fx.ResetAsync();
        await using var scope = _fx.Services.CreateAsyncScope();
        IDbContextFactory<DatabaseContext> factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        ILogger<EvaluationFormRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<EvaluationFormRepository>>();
        var repo = new EvaluationFormRepository(factory, logger);
        DateTime start = DateTime.UtcNow.AddMinutes(-5);
        DateTime? end = start.AddDays(3);
        string name = $"Имя Δ {Guid.NewGuid():N}";
        const string desc = "Описание µ ✪";
        string code = Guid.NewGuid().ToString("N");
        var tags = new List<string> { "Ålpha", "βeta", "Гамма" };
        var form = new EvaluationForm
        {
            Meta = new FormMeta
            {
                Name = new FormName { Value = name },
                Description = desc,
                Tags = tags,
                Code = new FormCode { Value = code },
            },
            Lifecycle = new FormLifecycle
            {
                Status = FormStatus.Draft,
                Validity = new Period { Start = start, End = end },
                Audit = new AuditTrail { Created = new Stamp { UserId = "usr-λ", At = start } },
            },
            Calculation = FormCalculationKind.ArithmeticMean,
        };
        long id = await repo.CreateAsync(form);
        var loaded = await repo.GetAsync(id, isFullInclude: true);
        var validity = loaded.Lifecycle.Validity!;
        bool ok = string.Equals(loaded.Meta.Name.Value, name, StringComparison.Ordinal)
                      && string.Equals(loaded.Meta.Description, desc, StringComparison.Ordinal)
                      && string.Equals(loaded.Meta.Code.Value, code, StringComparison.Ordinal)
                      && loaded.Meta.Tags.Count == tags.Count && loaded.Meta.Tags.Zip(tags, (a, b) => string.Equals(a, b, StringComparison.Ordinal)).All(x => x)
                      && loaded.Lifecycle.Status == FormStatus.Draft
                      && loaded.Lifecycle.Validity != null
              && Math.Abs((validity.Start - start).TotalSeconds) < 1
              && validity.End.HasValue && end.HasValue && Math.Abs((validity.End.Value - end.Value).TotalSeconds) < 1
                      && string.Equals(loaded.Lifecycle.Audit.Created.UserId, "usr-λ", StringComparison.Ordinal)
                      && loaded.Lifecycle.Audit.Updated == null
                      && loaded.Lifecycle.Audit.StateChanged == null
                      && loaded.Calculation == FormCalculationKind.ArithmeticMean;
        Assert.True(ok, "Form meta lifecycle and calculation integrity is broken which is a failure");
    }

    /// <summary>
    /// Verifies that weights, orders and generated identifiers for groups and criteria are preserved
    /// </summary>
    [Fact(DisplayName = "Create weighted form then get preserves weights orders and identifiers")]
    public async Task Create_weighted_form_then_get_preserves_weights_orders_and_identifiers()
    {
        await _fx.ResetAsync();
        await using var scope = _fx.Services.CreateAsyncScope();
        IDbContextFactory<DatabaseContext> factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        ILogger<EvaluationFormRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<EvaluationFormRepository>>();
        var repo = new EvaluationFormRepository(factory, logger);
        DateTime now = DateTime.UtcNow;
        string gTitle = $"Группа ♜ {Guid.NewGuid():N}";
        string ct1 = $"Критерий ☯ {Guid.NewGuid():N}";
        string ct2 = $"Критерий ✸ {Guid.NewGuid():N}";
        var c1 = MakeCriterion(ct1, "описание ♠");
        var c2 = MakeCriterion(ct2, "описание ♣");
        EvaluationForm form = BuildWeightedForm(now, gTitle, c1, c2);
        long id = await repo.CreateAsync(form);
        var loaded = await repo.GetAsync(id, isFullInclude: true);
        var grp = loaded.Groups.Single(x => string.Equals(x.Title, gTitle, StringComparison.Ordinal));
        var gcrit = grp.Criteria.Single(x => string.Equals(x.Criterion.Text.Title.Value, ct2, StringComparison.Ordinal));
        var tcrit = loaded.Criteria.Single(x => string.Equals(x.Criterion.Text.Title.Value, ct1, StringComparison.Ordinal));
        bool ok = grp.Id > 0
                  && gcrit.Id > 0
                  && tcrit.Id > 0
                  && grp.Order.Value == 1
                  && gcrit.Order.Value == 3
                  && tcrit.Order.Value == 2
                  && grp.Weight != null && grp.Weight!.Bps() == 2500
                  && gcrit.Weight != null && gcrit.Weight!.Bps() == 7500
                  && tcrit.Weight != null && tcrit.Weight!.Bps() == 1200
                  && loaded.Calculation == FormCalculationKind.WeightedMean;
        Assert.True(ok, "Weights orders or identifiers integrity is broken which is a failure");
    }

    /// <summary>
    /// Verifies that a form with nested groups is persisted and the full hierarchy is returned when loading the graph
    /// </summary>
    [Fact(DisplayName = "Create form with nested groups then get returns full nested graph" )]
    public async Task Create_form_with_nested_groups_then_get_returns_full_nested_graph()
    {
        await _fx.ResetAsync();
        await using var scope = _fx.Services.CreateAsyncScope();
        IDbContextFactory<DatabaseContext> factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        ILogger<EvaluationFormRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<EvaluationFormRepository>>();
        var repo = new EvaluationFormRepository(factory, logger);
        DateTime now = DateTime.UtcNow;
        string root = $"Группа α {Guid.NewGuid():N}";
        string child = $"Группа β {Guid.NewGuid():N}";
        string ct = $"Критерий γ {Guid.NewGuid():N}";
        Criterion crit = MakeCriterion(ct, "описание ζ");
        EvaluationForm form = BuildNestedForm(now, root, child, crit);
        long id = await repo.CreateAsync(form);
        var loaded = await repo.GetAsync(id, isFullInclude: true);
        var r = loaded.Groups.Single(g => string.Equals(g.Title, root, StringComparison.Ordinal));
        var ch = r.Groups.Single(g => string.Equals(g.Title, child, StringComparison.Ordinal));
        bool has = ch.Criteria.Any(fc => string.Equals(fc.Criterion.Text.Title.Value, ct, StringComparison.Ordinal));
        bool ok = r.Id > 0 && ch.Id > 0 && has;
        Assert.True(ok, "Nested groups hierarchy integrity is broken which is a failure");
    }


    /// <summary>
    /// Builds a weighted form with one top-level criterion and one group with a criterion for integrity checks
    /// </summary>
    private static EvaluationForm BuildWeightedForm(DateTime now, string groupTitle, Criterion top, Criterion inside)
    {
        var form = new EvaluationForm
        {
            Meta = new FormMeta
            {
                Name = new FormName { Value = "Взвешенная форма ♕" },
                Description = "Проверка весов ❖",
                Tags = ["w1", "W2"],
                Code = new FormCode { Value = Guid.NewGuid().ToString("N") },
            },
            Lifecycle = new FormLifecycle
            {
                Status = FormStatus.Draft,
                Validity = new Period { Start = now, End = null },
                Audit = new AuditTrail { Created = new Stamp { UserId = "u-κ", At = now } },
            },
            Calculation = FormCalculationKind.WeightedMean,
        };

        var group = new FormGroup
        {
            Title = groupTitle,
            Order = new OrderIndex { Value = 1 },
            Weight = new Weight(2500),
        };
        group.AddCriteria([ new FormCriterion { Id = 0, Criterion = inside, Order = new OrderIndex { Value = 3 }, Weight = new Weight(7500) } ]);

        form.AddGroups([ group ]);

        form.AddCriteria([new FormCriterion { Id = 0, Criterion = top, Order = new OrderIndex { Value = 2 }, Weight = new Weight(1200) } ]);

        return form;
    }

    /// <summary>
    /// Builds a form with a root group and a nested child group that contains a criterion
    /// </summary>
    private static EvaluationForm BuildNestedForm(DateTime now, string rootTitle, string childTitle, Criterion inside)
    {
        var form =  new EvaluationForm
        {
            Meta = new FormMeta
            {
                Name = new FormName { Value = "Иерархическая форма ψ" },
                Description = "Проверка вложенности ☑",
                Tags = ["nest"],
                Code = new FormCode { Value = Guid.NewGuid().ToString("N") },
            },
            Lifecycle = new FormLifecycle
            {
                Status = FormStatus.Draft,
                Validity = new Period { Start = now, End = null },
                Audit = new AuditTrail { Created = new Stamp { UserId = "u-ν", At = now } },
            },
            Calculation = FormCalculationKind.ArithmeticMean,
        };

        var rootGroup = new FormGroup
        {
            Title = rootTitle,
            Order = new OrderIndex { Value = 0 },
            Weight = null,
        };

        var childGroup = new FormGroup
        {
            Title = childTitle,
            Order = new OrderIndex { Value = 0 },
            Weight = null,
        };

        childGroup.AddCriteria([ new FormCriterion { Id = 0, Criterion = inside, Order = new OrderIndex { Value = 0 }, Weight = null } ]);

        rootGroup.AddChilds([ childGroup ]);

        form.AddGroups([ rootGroup ]);

        return form;
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
            Options = [],
            Automation = null,
        };
    }

    /// <summary>
    /// Builds an EvaluationForm with one top-level criterion and one group with a criterion.
    /// </summary>
    private static EvaluationForm MakeForm(DateTime now, string groupTitle, Criterion top, Criterion inside)
    {
        var form = new EvaluationForm
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
        };

        var criterion = new FormCriterion { Id = 0, Criterion = top, Order = new OrderIndex { Value = 0 }, Weight = null };
        form.AddCriteria([ criterion ]);

        var group = new FormGroup
        {
            Title = groupTitle,
            Order = new OrderIndex { Value = 0 },
            Weight = null,
        };

        var groupCriterion = new FormCriterion { Id = 0, Criterion = inside, Order = new OrderIndex { Value = 0 }, Weight = null };
        group.AddCriteria([ groupCriterion ]);

        form.AddGroups([ group ]);

        return form;
    }
}
