using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Context;

/// <summary>
/// EF Core database context for Evaluations module.
/// </summary>
public class DatabaseContext : DbContext
{
    /// <summary>
    /// Criteria
    /// </summary>
    public DbSet<Criterion> Criteria { get; set; }

    /// <summary>
    /// Form evaluation runs
    /// </summary>
    public DbSet<FormRun> FormRuns { get; set; }

    /// <summary>
    /// Evaluation forms
    /// </summary>
    public DbSet<EvaluationForm> EvaluationForms { get; set; }

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
        // Keep defaults for other types; JSON handled via Npgsql
        base.ConfigureConventions(configurationBuilder);
    }
}
