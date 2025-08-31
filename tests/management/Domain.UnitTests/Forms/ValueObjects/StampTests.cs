using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for Stamp value object null handling.
/// </summary>
public sealed class StampTests
{
    /// <summary>
    /// Verifies that constructor rejects null user identifier.
    /// </summary>
    [Fact(DisplayName = "Stamp cannot be created with null user identifier")]
    public void Stamp_cannot_be_created_with_null_user_identifier()
    {
        Should.Throw<ArgumentNullException>(() => new Stamp(null!, DateTime.UtcNow), "Stamp accepted a null user identifier which is incorrect");
    }
}

