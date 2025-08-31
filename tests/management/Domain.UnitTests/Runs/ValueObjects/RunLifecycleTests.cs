using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Runs.ValueObjects;

/// <summary>
/// Tests for RunLifecycle value object invariants and accessors.
/// </summary>
public sealed class RunLifecycleTests
{
    /// <summary>
    /// Verifies that constructor rejects null launch stamp.
    /// </summary>
    [Fact(DisplayName = "RunLifecycle cannot be created with null launch stamp")]
    public void RunLifecycle_cannot_be_created_with_null_launch_stamp()
    {
        Should.Throw<ArgumentNullException>(() => new RunLifecycle(null!, new NullStamp(), new NullStamp(), new NullStamp()), "RunLifecycle accepted a null launch stamp which is incorrect");
    }

    /// <summary>
    /// Verifies that Launched returns the same stamp.
    /// </summary>
    [Fact(DisplayName = "RunLifecycle returns the same launch stamp")]
    public void RunLifecycle_returns_the_same_launch_stamp()
    {
        var stamp = new Stamp("usr-✓-" + Guid.NewGuid(), DateTime.UtcNow);
        var vo = new RunLifecycle(stamp, new NullStamp(), new NullStamp(), new NullStamp());
        vo.Launched().ShouldBe(stamp, "RunLifecycle returned an unexpected launch stamp which is incorrect");
    }
}
