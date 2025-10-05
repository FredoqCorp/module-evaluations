namespace CascVel.Modules.Evaluations.Management.Domain.Common;

/// <summary>
/// Extension methods for working with enumerables and options.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Returns the first element of a sequence as an Option, or None if the sequence is empty.
    /// </summary>
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> sequence) =>
            sequence.Select(x => Option.Of(x))
                .DefaultIfEmpty(Option.None<T>())
                .First();
    /// <summary>
    /// Returns the first element of a sequence that satisfies a condition as an Option, or None if no such element is found.
    /// </summary>
    public static Option<T> FirstOrNone<T>(
        this IEnumerable<T> sequence, Func<T, bool> predicate) =>
        sequence.Where(predicate).FirstOrNone();
}
