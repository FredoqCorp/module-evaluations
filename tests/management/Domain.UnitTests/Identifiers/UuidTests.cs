using System.Collections.Concurrent;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Identifiers;

/// <summary>
/// Unit tests for the Uuid value object ensuring canonical formatting, version, and concurrency guarantees.
/// </summary>
public sealed class UuidTests
{
    /// <summary>
    /// Ensures default construction produces a UUIDv7 value.
    /// </summary>
    [Fact]
    public void It_creates_a_UUIDv7_on_default_construction()
    {
        var id = new Uuid();
        var text = id.Text();

        text[14].ShouldBe('7', "created identifier is not a UUIDv7");
    }

    /// <summary>
    /// Ensures string conversion returns the canonical text representation.
    /// </summary>
    [Fact]
    public void It_returns_canonical_text_when_converted_to_string()
    {
        var id = new Uuid();

        id.ToString().ShouldBe(id.Text(), "string representation does not match canonical text");
    }

    /// <summary>
    /// Ensures provided Guid value is preserved by round trip through Text.
    /// </summary>
    [Fact]
    public void It_preserves_the_provided_Guid_on_round_trip()
    {
        var g = Guid.NewGuid();
        var id = new Uuid(g);

        Guid.Parse(id.Text()).ShouldBe(g, "text does not parse back to the same Guid");
    }

    /// <summary>
    /// Ensures generated identifier is not equal to the empty Guid.
    /// </summary>
    [Fact]
    public void It_does_not_generate_an_empty_identifier()
    {
        var id = new Uuid();
        var text = id.Text();

        text.ShouldNotBe(Guid.Empty.ToString(), "generated identifier equals the empty Guid");
    }

    /// <summary>
    /// Ensures identifiers generated concurrently are unique and generation completes within a timeout.
    /// </summary>
    [Fact]
    public async Task It_generates_unique_identifiers_in_concurrent_environment()
    {
        var attempts = 0;
        var expected = 256;
        var unique = new ConcurrentDictionary<string, byte>();

        while (attempts < 2 && unique.Count < expected)
        {
            unique.Clear();

            var tasks = Enumerable
                .Range(0, expected)
                .Select(_ => Task.Run(() =>
                {
                    var text = new Uuid().Text();
                    unique.TryAdd(text, 0);
                }));

            await Task.WhenAll(tasks).WaitAsync(TimeSpan.FromSeconds(5));
            attempts++;
        }

        unique.Count.ShouldBe(expected, "concurrent generation produced duplicate identifiers");
    }
}

