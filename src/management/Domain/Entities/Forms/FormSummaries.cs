using System;
using System.Collections.Generic;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Common;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Immutable entity that encapsulates a printable collection of form summaries.
/// </summary>
public sealed record FormSummaries : IFormSummaries
{
    private readonly IReadOnlyCollection<IFormSummary> _items;

    /// <summary>
    /// Initializes the collection with the provided summaries.
    /// </summary>
    /// <param name="items">Collection of form summaries.</param>
    public FormSummaries(IReadOnlyCollection<IFormSummary> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = items;
    }

    /// <summary>
    /// Prints the collection to the provided media.
    /// </summary>
    /// <param name="media">Target media that receives the printed representation.</param>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <returns>The media instance that received the printed representation.</returns>
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        media.WithArray("forms", Items(_items, media));
        return media;
    }

    /// <summary>
    /// Returns the read-only collection of summaries.
    /// </summary>
    /// <returns>Collection of summaries.</returns>
    public IReadOnlyCollection<IFormSummary> Values()
    {
        return _items;
    }

    private static IEnumerable<Action<IMedia>> Items<TOutput>(IEnumerable<IFormSummary> options, IMedia<TOutput> media)
    {
        foreach (var option in options)
        {
            yield return _ => option.Print(media);
        }
    }

    private sealed class NestedMedia<TOutput> : IMedia<TOutput>
    {
        private readonly IMedia _inner;

        public NestedMedia(IMedia inner)
        {
            ArgumentNullException.ThrowIfNull(inner);

            _inner = inner;
        }

        public IMedia With(string key, string value)
        {
            _inner.With(key, value);
            return this;
        }

        public IMedia With(string key, Option<string> value)
        {
            _inner.With(key, value);
            return this;
        }

        public IMedia With(string key, Guid value)
        {
            _inner.With(key, value);
            return this;
        }

        public IMedia With(string key, int value)
        {
            _inner.With(key, value);
            return this;
        }

        public IMedia With(string key, IEnumerable<string> values)
        {
            _inner.With(key, values);
            return this;
        }

        public IMedia WithArray(string key, IEnumerable<Action<IMedia>> items)
        {
            ArgumentNullException.ThrowIfNull(items);

            IEnumerable<Action<IMedia>> Wrap()
            {
                foreach (var item in items)
                {
                    yield return inner =>
                    {
                        ArgumentNullException.ThrowIfNull(item);
                        var adapter = new NestedMedia<TOutput>(inner);
                        item(adapter);
                    };
                }
            }

            _inner.WithArray(key, Wrap());
            return this;
        }

        public IMedia WithObject(string key, Action<IMedia> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);

            _inner.WithObject(
                key,
                inner =>
                {
                    var adapter = new NestedMedia<TOutput>(inner);
                    configure(adapter);
                });
            return this;
        }

        public TOutput Output()
        {
            throw new InvalidOperationException("Output is not available for nested media");
        }

        public void Dispose()
        {
        }
    }
}
