using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunContext value object invariants and accessors.
/// </summary>
public sealed class RunContextTests
{
    /// <summary>
    /// Verifies that constructor rejects null items dictionary.
    /// </summary>
    [Fact(DisplayName = "RunContext cannot be created with null items dictionary")]
    public void RunContext_cannot_be_created_with_null_items_dictionary()
    {
        Should.Throw<ArgumentNullException>(() => new RunContext(null!), "RunContext accepted a null items dictionary which is incorrect");
    }

    /// <summary>
    /// Verifies that Items returns the same count as provided.
    /// </summary>
    [Fact(DisplayName = "RunContext returns the same items count")]
    public void RunContext_returns_the_same_items_count()
    {
        var builder = ImmutableDictionary.CreateBuilder<string, string>();
        builder["ключ-✓-" + Guid.NewGuid()] = "значение-✓-" + Guid.NewGuid();
        var vo = new RunContext(builder.ToImmutable());
        vo.Items().Count.ShouldBe(1, "RunContext returned an unexpected items count which is incorrect");
    }
}

