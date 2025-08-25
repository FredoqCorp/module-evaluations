using System.Collections.Concurrent;
using System.Security.Cryptography;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.ValueObjects;

/// <summary>
/// Tests for the ServiceCode value object behavior
/// The tests use irregular Unicode inputs, one assertion per test, and avoid checking constructor behavior directly
/// </summary>
public sealed class ServiceCodeTests
{
    private static string Rnd()
    {
        int len = RandomNumberGenerator.GetInt32(3, 12);
        char[] arr = new char[len];
        for (int i = 0; i < len; i++)
        {
            int range = RandomNumberGenerator.GetInt32(0, 3);
            int code;
            if (range == 0)
            {
                code = RandomNumberGenerator.GetInt32(0x0370, 0x03FF);
            }
            else if (range == 1)
            {
                code = RandomNumberGenerator.GetInt32(0x0400, 0x04FF);
            }
            else
            {
                code = '0' + RandomNumberGenerator.GetInt32(0, 10);
            }
            arr[i] = (char)code;
        }
        return new string(arr);
    }
    /// <summary>
    /// Verifies that value is trimmed and exposed via ToString using irregular Unicode
    /// </summary>
    [Fact(DisplayName = "ServiceCode stores exactly the trimmed value as its string representation")]
    public void ServiceCode_stores_exactly_the_trimmed_value_as_its_string_representation()
    {
        string inner = Rnd();
        string value = "  " + inner + "  ";
        var code = new ServiceCode(value);
        code.ToString().ShouldBe(inner, "ServiceCode did not trim the input which is incorrect");
    }

    /// <summary>
    /// Verifies that two instances with the same semantic value are equal by value
    /// </summary>
    [Fact(DisplayName = "ServiceCode compares equal for identical semantic values")]
    public void ServiceCode_compares_equal_for_identical_semantic_values()
    {
        string s = Rnd();
        var one = new ServiceCode(s);
        var two = new ServiceCode(s);
        one.Equals(two).ShouldBeTrue("ServiceCode did not use value equality which is incorrect");
    }

    /// <summary>
    /// Verifies immutable consistent behavior under concurrent reads
    /// </summary>
    [Fact(DisplayName = "ServiceCode keeps its value consistent under concurrent reads")]
    public void ServiceCode_keeps_its_value_consistent_under_concurrent_reads()
    {
        string inner = Rnd();
        string value = "  " + inner + "  ";
        var code = new ServiceCode(value);
        var bag = new ConcurrentBag<string>();
        Parallel.For(0, 128, _ => bag.Add(code.ToString()));
        bag.All(s => string.Equals(s, inner, StringComparison.Ordinal)).ShouldBeTrue("ServiceCode returned inconsistent value under concurrency which is incorrect");
    }
}
