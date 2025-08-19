namespace CascVel.Module.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

using System.Security.Cryptography;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Shouldly;

/// <summary>
/// Tests for Weight value object conversion methods.
/// </summary>
public sealed class WeightTests
{
    /// <summary>
    /// Verifies that Percent and Bps expose consistent values.
    /// </summary>
    [Fact(DisplayName = "Weight exposes basis points and percent consistently")]
    public void Weight_exposes_basis_points_and_percent_consistently()
    {
        ushort bps = checked((ushort)RandomNumberGenerator.GetInt32(0, 10_001));
        var vo = new Weight(bps);
        decimal pct = vo.Percent();
        vo.Bps().ShouldBe(checked((ushort)decimal.Round(pct * 100m, 0, MidpointRounding.ToZero)), "Weight returned inconsistent values which is incorrect");
    }
}
