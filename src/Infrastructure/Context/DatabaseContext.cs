using CascVel.Module.Evaluations.Management.Domain.Entities.AutomaticParameters;
using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Form;
using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Context;

/// <summary>
/// EF Core database context for Evaluations module.
/// </summary>
public class DatabaseContext : DbContext
{
    /// <summary>
    /// Evaluation forms
    /// </summary>
    public DbSet<EvaluationForm> Forms { get; set; }
    /// <summary>
    /// Base criteria set (TPH)
    /// </summary>
    public DbSet<BaseCriterion> BaseCriterion { get; set; }
    /// <summary>
    /// Automatic criteria set (TPH subset)
    /// </summary>
    public DbSet<AutomaticCriterion> AutomaticCriterion { get; set; }
    /// <summary>
    /// Default criteria set (TPH subset)
    /// </summary>
    public DbSet<DefaultCriterion> DefaultCriterion { get; set; }
    /// <summary>
    /// Group criteria set (TPH subset)
    /// </summary>
    public DbSet<GroupCriterion> GroupCriterion { get; set; }
    /// <summary>
    /// Automatic parameters
    /// </summary>
    public DbSet<AutomaticParameter> AutomaticParameters { get; set; }
    // Runs
    /// <summary>
    /// Completed runs
    /// </summary>
    public DbSet<Run> Runs { get; set; }
    /// <summary>
    /// Per-criterion results
    /// </summary>
    public DbSet<RunCriterionResult> Results { get; set; }

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
         // Default precision for decimals across the model
         configurationBuilder.Properties<decimal>().HavePrecision(10, 2);
         base.ConfigureConventions(configurationBuilder);
     }
}
