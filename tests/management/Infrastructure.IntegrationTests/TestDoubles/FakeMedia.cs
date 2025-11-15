using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.TestDoubles;

/// <summary>
/// In-memory media double that captures writes for verification.
/// </summary>
internal sealed class FakeMedia : IMedia<object>
{
    public List<(string Key, object Value)> Writes { get; } = [];

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
        Writes.Add((key, values.ToArray()));
        return this;
    }

    public IMedia WithObject(string key, Action<IMedia> configure)
    {
        using var nestedMedia = new FakeMedia();
        configure(nestedMedia);
        Writes.Add((key, nestedMedia.Output()));
        return this;
    }

    public T? GetValue<T>(string key)
    {
        var write = Writes.FirstOrDefault(w => w.Key == key);
        return write.Key is not null ? (T?)write.Value : default;
    }

    public IMedia WithArray(string key, IEnumerable<Action<IMedia>> items)
    {
        var arrayItems = new List<object>();
        foreach (var configure in items)
        {
            using var itemMedia = new FakeMedia();
            configure(itemMedia);
            var snapshot = (List<(string Key, object Value)>)itemMedia.Output();
            arrayItems.Add(snapshot.ToList());
        }
        Writes.Add((key, arrayItems));
        return this;
    }

    /// <summary>
    /// Writes an array of nested objects asynchronously using the fake media.
    /// </summary>
    public async Task<IMedia> WithArrayAsync(string key, IEnumerable<Func<IMedia, Task>> items)
    {
        var arrayItems = new List<object>();
        foreach (var configure in items)
        {
            using var itemMedia = new FakeMedia();
            await configure(itemMedia);
            var snapshot = (List<(string Key, object Value)>)itemMedia.Output();
            arrayItems.Add(snapshot.ToList());
        }

        Writes.Add((key, arrayItems));
        return this;
    }

    public object Output()
    {
        return Writes;
    }
}
