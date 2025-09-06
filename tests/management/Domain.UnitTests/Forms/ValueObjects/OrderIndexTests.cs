using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for OrderIndex value object.
/// </summary>
public sealed class OrderIndexTests
{
    /// <summary>
    /// Verifies that Value returns the same non-negative number.
    /// </summary>
    [Fact(DisplayName = "OrderIndex returns the same non negative number")]
    public void OrderIndex_returns_the_same_non_negative_number()
    {
        var v = System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, 10_000);
        var vo = new OrderIndex(v);
        vo.Value().ShouldBe(v, "OrderIndex returned an unexpected number which is incorrect");
    }

    /// <summary>
    /// Verifies that negative numbers fail fast on access.
    /// </summary>
    [Fact(DisplayName = "OrderIndex cannot return value when negative")]
    public void OrderIndex_cannot_return_value_when_negative()
    {
        var v = -System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, 1000);
        var vo = new OrderIndex(v);
        Should.Throw<InvalidDataException>(() => vo.Value(), "OrderIndex accepted a negative number which is incorrect");
    }
}

