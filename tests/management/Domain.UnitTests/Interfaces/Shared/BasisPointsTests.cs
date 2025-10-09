using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Shared;

/// <summary>
/// Tests for IBasisPoints behavioral contract.
/// </summary>
public sealed class BasisPointsTests
{
    [Fact]
    public void Converts_to_percent_representation()
    {
        var basis = SharedTypesTestData.RandomBasisPoints();

        var percent = basis.Percent();

        Assert.NotNull(percent);
    }

    [Fact]
    public void Applies_basis_points_to_decimal_value()
    {
        var basis = SharedTypesTestData.RandomBasisPoints();
        var value = (decimal)Random.Shared.Next(100, 1000);

        var result = basis.Apply(value);

        Assert.True(result >= 0);
    }

    [Fact]
    public void Produces_zero_when_applied_to_zero()
    {
        var basis = SharedTypesTestData.RandomBasisPoints();

        var result = basis.Apply(0m);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void Maintains_consistency_between_conversions()
    {
        var basis = SharedTypesTestData.RandomBasisPoints();

        var percent = basis.Percent();
        var roundtrip = percent.Basis();

        Assert.NotNull(roundtrip);
    }
}
