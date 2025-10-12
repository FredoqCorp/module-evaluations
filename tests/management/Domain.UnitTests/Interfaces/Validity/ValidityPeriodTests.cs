using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Validity;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Validity;

/// <summary>
/// Tests for IValidityPeriod behavioral contract.
/// </summary>
public sealed class ValidityPeriodTests
{
    [Fact]
    public void Returns_new_period_with_updated_boundary()
    {
        var period = ValidityPeriodTestData.OpenPeriod();
        var end = new ValidityEnd(DateTime.UtcNow.AddDays(Random.Shared.Next(10, 30)));

        var updated = period.Until(end);

        Assert.NotNull(updated);
    }

    [Fact]
    public void Confirms_moment_inside_validity_window()
    {
        var period = ValidityPeriodTestData.ClosedPeriod();
        var momentInside = DateTime.UtcNow;

        var isActive = period.Active(momentInside);

        Assert.True(isActive);
    }

    [Fact]
    public void Denies_moment_outside_validity_window()
    {
        var period = ValidityPeriodTestData.ExpiredPeriod();
        var momentOutside = DateTime.UtcNow;

        var isActive = period.Active(momentOutside);

        Assert.False(isActive);
    }

    [Fact]
    public void Preserves_immutability_when_setting_end()
    {
        var original = ValidityPeriodTestData.OpenPeriod();
        var end = new ValidityEnd(DateTime.UtcNow.AddDays(Random.Shared.Next(10, 30)));

        var updated = original.Until(end);

        Assert.NotEqual(original, updated);
    }

    [Fact]
    public void Supports_open_ended_validity()
    {
        var period = ValidityPeriodTestData.OpenPeriod();
        var futureMoment = DateTime.UtcNow.AddYears(Random.Shared.Next(1, 10));

        var isActive = period.Active(futureMoment);

        Assert.True(isActive);
    }

    [Fact]
    public void Handles_moment_at_exact_boundary()
    {
        var now = DateTime.UtcNow;
        var period = ValidityPeriodTestData.OpenPeriod(start: now).Until(new ValidityEnd(now));

        var isActive = period.Active(now);

        Assert.True(isActive);
    }
}
