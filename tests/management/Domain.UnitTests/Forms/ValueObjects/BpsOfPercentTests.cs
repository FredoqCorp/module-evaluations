using System.Security.Cryptography;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for BpsOfPercent value method ensuring rounding and bounds behaviors.
/// </summary>
public sealed class BpsOfPercentTests
{
    private static decimal RandPercent()
    {
        int whole = RandomNumberGenerator.GetInt32(0, 100);
        int frac = RandomNumberGenerator.GetInt32(0, 10000);
        return whole + (frac / 10000m);
    }

    /// <summary>
    /// Verifies that percent converts to basis points using the provided rounding policy.
    /// </summary>
    [Fact(DisplayName = "BpsOfPercent returns basis points rounded by the provided policy")]
    public void BpsOfPercent_returns_basis_points_rounded_by_the_provided_policy()
    {
        decimal p = RandPercent();
        var vo = new BpsOfPercent(p, MidpointRounding.ToZero);
        ushort got = vo.Value();
        got.ShouldBe(checked((ushort)decimal.Round(p * 100m, 0, MidpointRounding.ToZero)), "BpsOfPercent returned unexpected bps value which is incorrect");
    }

    /// <summary>
    /// Verifies that out-of-range values cause a fast failure.
    /// </summary>
    [Fact(DisplayName = "BpsOfPercent cannot return a value for percent outside zero to one hundred")]
    public void BpsOfPercent_cannot_return_a_value_for_percent_outside_zero_to_one_hundred()
    {
        decimal p = 100m + (RandomNumberGenerator.GetInt32(1, 100) / 10m);
        var vo = new BpsOfPercent(p, MidpointRounding.AwayFromZero);
        Should.Throw<InvalidDataException>(() => vo.Value(), "BpsOfPercent accepted an out of range percent which is incorrect");
    }
}
