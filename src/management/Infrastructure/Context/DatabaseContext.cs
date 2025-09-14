using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Context;

/// <summary>
/// EF Core database context for Evaluations module.
/// </summary>
public sealed class DatabaseContext : DbContext
{
    /// <summary>
    /// Criteria
    /// </summary>
    public DbSet<Criterion> Criteria { get; init; } = null!;

    /// <summary>
    /// Form evaluation runs
    /// </summary>
    public DbSet<FormRun> FormRuns { get; init; } = null!;

    /// <summary>
    /// Evaluation forms
    /// </summary>
    public DbSet<EvaluationForm> EvaluationForms { get; init; } = null!;

    /// <summary>
    /// Initializes the context with options.
    /// </summary>
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.HasDefaultSchema("evaluations");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        configurationBuilder.Properties<decimal>().HavePrecision(10, 2);
        base.ConfigureConventions(configurationBuilder);
    }
}
