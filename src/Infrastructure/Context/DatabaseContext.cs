using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Context;

/// <summary>
/// EF Core database context for Evaluations module.
/// </summary>
public sealed class DatabaseContext : DbContext
{
    /// <summary>
    /// Criteria
    /// </summary>
    internal DbSet<Criterion> Criteria { get; set; }

    /// <summary>
    /// Form evaluation runs
    /// </summary>
    internal DbSet<FormRun> FormRuns { get; set; }

    /// <summary>
    /// Evaluation forms
    /// </summary>
    internal DbSet<EvaluationForm> EvaluationForms { get; set; }

    /// <summary>
    /// Evaluation form groups
    /// </summary>
    internal DbSet<FormGroup> FormGroups { get; set; }

    /// <summary>
    /// Initializes the context with options.
    /// </summary>
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Use dedicated schema for this module
        modelBuilder.HasDefaultSchema("evaluations");

        // Apply entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(10, 2);

        base.ConfigureConventions(configurationBuilder);
    }
}
