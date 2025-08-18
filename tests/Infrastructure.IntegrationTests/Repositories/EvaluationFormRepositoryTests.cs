using CascVel.Module.Evaluations.Management.Application.Interfaces;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.Calculation;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
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
}
