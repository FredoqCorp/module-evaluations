using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Forms.ValueObjects;

/// <summary>
/// Tests for Period invariants.
/// </summary>
public sealed class PeriodInvariantsTests
{
    /// <summary>
    /// Verifies that Finish throws when it is less than Start.
    /// </summary>
    [Fact(DisplayName = "Period cannot finish earlier than it starts")]
    public void Period_cannot_finish_earlier_than_it_starts()
    {
        var start = DateTime.UtcNow;
        var end = start.AddDays(-1);
        var p = new Period(start, end);
        Should.Throw<InvalidDataException>(() => p.Finish(), "Period accepted finish earlier than start which is incorrect");
    }
}

