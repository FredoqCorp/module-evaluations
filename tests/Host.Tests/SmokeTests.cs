using FluentAssertions;
using Xunit;

namespace Host.Tests;

public class SmokeTests
{
    [Fact]
    public void Math_Works() => (2 + 2).Should().Be(4);
}
