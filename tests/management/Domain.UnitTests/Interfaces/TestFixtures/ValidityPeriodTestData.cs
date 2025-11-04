using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Validity;
using CascVel.Modules.Evaluations.Management.Domain.Models.Validity;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

/// <summary>
/// Test data factory for validity period scenarios.
/// </summary>
internal static class ValidityPeriodTestData
{
    internal static IValidityPeriod OpenPeriod(DateTime? start = null) =>
        new TestValidityPeriod(start ?? DateTime.UtcNow, null);

    internal static IValidityPeriod ClosedPeriod() =>
        new TestValidityPeriod(
            DateTime.UtcNow.AddDays(-Random.Shared.Next(10, 30)),
            DateTime.UtcNow.AddDays(Random.Shared.Next(10, 30)));

    internal static IValidityPeriod ExpiredPeriod() =>
        new TestValidityPeriod(
            DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 60)),
            DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 10)));
}

/// <summary>
/// Test double for validity period interface.
/// </summary>
internal sealed record TestValidityPeriod(DateTime Start, DateTime? End) : IValidityPeriod
{
    public IValidityPeriod Until(ValidityEnd end) =>
        new TestValidityPeriod(Start, end.Value);

    public bool Active(DateTime moment) =>
        moment >= Start && (End == null || moment <= End);
}
