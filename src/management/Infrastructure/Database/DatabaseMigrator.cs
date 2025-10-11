using System.Collections.ObjectModel;
using System.Reflection;
using DbUp;
using DbUp.Engine;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Database;

/// <summary>
/// Handles database schema migrations using DbUp.
/// </summary>
public static class DatabaseMigrator
{
    /// <summary>
    /// Executes all pending database migrations.
    /// </summary>
    /// <param name="connectionString">PostgreSQL connection string.</param>
    /// <returns>Result of the migration operation.</returns>
    public static DatabaseUpgradeResult MigrateDatabase(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                Assembly.GetExecutingAssembly(),
                script => script.Contains(".Database.Scripts.", StringComparison.Ordinal))
            .WithTransactionPerScript()
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        return result;
    }

    /// <summary>
    /// Checks if database needs migration.
    /// </summary>
    /// <param name="connectionString">PostgreSQL connection string.</param>
    /// <returns>True if there are pending migrations.</returns>
    public static bool IsUpgradeRequired(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                Assembly.GetExecutingAssembly(),
                script => script.Contains(".Database.Scripts.", StringComparison.Ordinal))
            .Build();

        return upgrader.IsUpgradeRequired();
    }

    /// <summary>
    /// Gets list of scripts that will be executed.
    /// </summary>
    /// <param name="connectionString">PostgreSQL connection string.</param>
    /// <returns>Read-only collection of pending script names.</returns>
    public static ReadOnlyCollection<string> GetScriptsToExecute(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                Assembly.GetExecutingAssembly(),
                script => script.Contains(".Database.Scripts.", StringComparison.Ordinal))
            .Build();

        var scripts = upgrader.GetScriptsToExecute()
            .Select(s => s.Name)
            .ToList();

        return scripts.AsReadOnly();
    }
}
