using System.Collections.Immutable;
using System.Globalization;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

/// <summary>
/// Immutable collection of rating options.
/// </summary>
public sealed record RatingOptions : IRatingOptions
{
    private readonly IImmutableList<IRatingOption> _options;

    /// <summary>
    /// Creates a collection of rating options from the provided enumerable without accepting null.
    /// </summary>
    /// <param name="options">Enumerable of rating options.</param>
    public RatingOptions(IEnumerable<IRatingOption> options)
    {
        _options = [.. options];
    }

    /// <inheritdoc />
    public void Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        for (int i = 0; i < _options.Count; i++)
        {
            var option = _options[i];
            var scoreKey = i.ToString(CultureInfo.InvariantCulture);

            media.WithObject(scoreKey, c =>
            {
                option.Print(media);
            });
        }
    }
}
