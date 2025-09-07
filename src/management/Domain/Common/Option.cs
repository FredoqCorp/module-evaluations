namespace CascVel.Modules.Evaluations.Management.Domain.Common;

/// <summary>
/// Optional value object that models presence or absence without using null.
/// </summary>
#pragma warning disable CA1716 // Identifiers should not match keywords
public readonly record struct Option<T>
#pragma warning restore CA1716 // Identifiers should not match keywords
{
    private readonly T? _content;
    internal Option(T content) => _content = content;

    /// <summary>
    /// Returns an Option containing the result of applying the provided mapping function to the contained value, or None if absent.
    /// </summary>
    /// <typeparam name="TR">The type of the mapped value.</typeparam>
    /// <param name="map">The mapping function to apply to the contained value.</param>
    /// <returns>An Option containing the mapped value or None.</returns>
    public Option<TR> Map<TR>(Func<T, TR> map)
    {
        ArgumentNullException.ThrowIfNull(map);
        return _content is not null ? Option.Of(map(_content)) : Option.None<TR>();
    }

    /// <summary>
    /// Returns an Option containing the result of applying the provided binding function to the contained value, or None if absent.
    /// </summary>
    /// <typeparam name="TR">The type of the bound value.</typeparam>
    /// <param name="bind">The binding function to apply to the contained value.</param>
    /// <returns>An Option containing the bound value or None.</returns>
    public Option<TR> Bind<TR>(Func<T, Option<TR>> bind)
    {
        ArgumentNullException.ThrowIfNull(bind);
        return _content is not null ? bind(_content) : Option.None<TR>();
    }

    /// <summary>
    /// Returns the contained value if present; otherwise returns the provided alternative value.
    /// </summary>
    /// <param name="orElse">The value to return if no content is present.</param>
    /// <returns>The contained value or the alternative value.</returns>
    public T Reduce(T orElse) => _content ?? orElse;

    /// <summary>
    /// Returns the contained value if present; otherwise returns the value produced by the provided function.
    /// </summary>
    /// <param name="orElse">A function that produces an alternative value if no content is present.</param>
    /// <returns>The contained value or the value produced by <paramref name="orElse"/>.</returns>
    public T Reduce(Func<T> orElse)
    {
        ArgumentNullException.ThrowIfNull(orElse);
        return _content ?? orElse();
    }

    /// <inheritdoc/>
    public override string ToString() =>
        _content is not null ? _content.ToString() ?? string.Empty : string.Empty;
}

#pragma warning disable CA1716 // Identifiers should not match keywords

/// <summary>
/// Factory helpers for creating Option values without using null
/// </summary>
public static class Option
{
    /// <summary>
    /// Creates an Option that contains the provided value.
    /// </summary>
    /// <param name="content">The value to wrap.</param>
    /// <returns>An Option containing the provided value.</returns>
    public static Option<T> Of<T>(T content) => new(content ?? throw new ArgumentException("Option cannot accept null value", nameof(content)));

    /// <summary>
    /// Creates an empty Option with no value.
    /// </summary>
    /// <returns>An Option with no value.</returns>
    public static Option<T> None<T>() => new();
}
