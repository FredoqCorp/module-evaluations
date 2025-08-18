using Infrastructure.IntegrationTests.Fixtures;

namespace Infrastructure.IntegrationTests.Collections;

/// <summary>
/// xUnit collection that shares a single Postgres container and database between tests.
/// </summary>
[CollectionDefinition("postgres-db")]
public sealed class PostgresCollection : ICollectionFixture<PostgresFixture>
{
}
