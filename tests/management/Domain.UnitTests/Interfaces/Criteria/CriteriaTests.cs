using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for ICriteria behavioral contract.
/// </summary>
public sealed class CriteriaTests
{
    [Fact]
    public void Validates_internal_consistency()
    {
        var criteria = new TestCriteria(true);

        var exception = Record.Exception(() => criteria.Validate());

        Assert.Null(exception);
    }
}

/// <summary>
/// Test double for criteria interface.
/// </summary>
file sealed record TestCriteria(bool IsValid) : ICriteria
{
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
