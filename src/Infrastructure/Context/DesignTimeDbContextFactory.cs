using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Context;

/// <summary>
/// Design-time factory for EF Core migrations/tools.
/// Keeps Infrastructure migrations independent from Host startup.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    /// <summary>
    /// Creates DatabaseContext for design-time operations (migrations).
    /// </summary>
    /// <param name="args">CLI arguments.</param>
    /// <returns>Configured <see cref="DatabaseContext"/>.</returns>
    public DatabaseContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Database")
            ?? Environment.GetEnvironmentVariable("EVALUATIONS_DB_CONNECTION")
            ?? "Host=localhost;Port=5432;Database=module_evaluations;Username=postgres;Password=postgres";

        DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>()
            .UseNpgsql(connectionString);

        return new DatabaseContext(optionsBuilder.Options);
    }
}
