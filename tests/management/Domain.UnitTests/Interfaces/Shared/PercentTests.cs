using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Shared;

/// <summary>
/// Tests for IPercent behavioral contract.
/// </summary>
public sealed class PercentTests
{
    [Fact]
    public void Converts_to_basis_points_representation()
    {
        var percent = SharedTypesTestData.RandomPercent();

        var basis = percent.Basis();

        Assert.NotNull(basis);
    }

    [Fact]
    public void Maintains_bidirectional_conversion_with_basis_points()
    {
        var percent = SharedTypesTestData.RandomPercent();

        var basis = percent.Basis();
        var roundtrip = basis.Percent();

        Assert.NotNull(roundtrip);
    }

    [Fact]
    public void Supports_repeated_basis_conversion()
    {
        var percent = SharedTypesTestData.RandomPercent();

        var first = percent.Basis();
        var second = percent.Basis();

        Assert.Equal(first, second);
    }
}
