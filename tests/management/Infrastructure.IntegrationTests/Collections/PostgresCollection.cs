using CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Fixtures;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Collections;

/// <summary>
/// xUnit collection that shares a single Postgres container and database between tests.
/// </summary>
[CollectionDefinition("postgres-db")]
#pragma warning disable CA1515 // Consider making public types internal
public sealed class PostgresDbScope : ICollectionFixture<PostgresFixture>
#pragma warning restore CA1515 // Consider making public types internal
{
}
