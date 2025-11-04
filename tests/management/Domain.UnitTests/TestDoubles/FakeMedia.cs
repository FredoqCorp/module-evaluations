using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.TestDoubles;

/// <summary>
/// Fake implementation of IMedia for testing printer pattern.
/// </summary>
internal sealed class FakeMedia : IMedia<object>
{
    public List<(string Key, object Value)> Writes { get; } = [];

    /// <summary>
    /// Releases resources held by the fake media.
    /// </summary>
    public void Dispose()
    {
        Writes.Clear();
    }

    public IMedia With(string key, string value)
    {
        Writes.Add((key, value));
        return this;
    }

    public IMedia With(string key, Option<string> value)
    {
        if (value.IsSome)
        {
            value.Map(v =>
            {
                Writes.Add((key, v));
                return v;
            });
        }
        return this;
    }

    public IMedia With(string key, Guid value)
    {
        Writes.Add((key, value));
        return this;
    }

    public IMedia With(string key, int value)
    {
        Writes.Add((key, value));
        return this;
    }

    public IMedia With(string key, IEnumerable<string> values)
    {
        Writes.Add((key, values.ToList()));
        return this;
    }

    public IMedia WithObject(string key, Action<IMedia> configure)
    {
        using var nestedMedia = new FakeMedia();
        configure(nestedMedia);
        Writes.Add((key, nestedMedia.Output()));
        return this;
    }

    public IMedia WithArray(string key, IEnumerable<Action<IMedia>> items)
    {
        var arrayItems = new List<object>();
        foreach (var configure in items)
        {
            using var itemMedia = new FakeMedia();
            configure(itemMedia);
            arrayItems.Add(itemMedia.Output());
        }
        Writes.Add((key, arrayItems));
        return this;
    }

    public object Output()
    {
        return Writes;
    }
}
