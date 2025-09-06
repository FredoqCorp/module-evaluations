using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunLifecycle value object invariants and accessors.
/// </summary>
public sealed class RunLifecycleTests
{
    /// <summary>
    /// Verifies that Launched returns the same stamp.
    /// </summary>
    [Fact(DisplayName = "RunLifecycle returns the same launch stamp")]
    public void RunLifecycle_returns_the_same_launch_stamp()
    {
        var stamp = new Stamp("usr-✓-" + Guid.NewGuid(), DateTime.UtcNow);
        var vo = new RunLifecycle(stamp, new Stamp("st-✓-" + Guid.NewGuid(), DateTime.UtcNow), new Stamp("c-✓-" + Guid.NewGuid(), DateTime.UtcNow), new Stamp("p-✓-" + Guid.NewGuid(), DateTime.UtcNow));
        vo.Launched().ShouldBe(stamp, "RunLifecycle returned an unexpected launch stamp which is incorrect");
    }
}
