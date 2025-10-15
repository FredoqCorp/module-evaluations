using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.TestDoubles;

internal sealed class FakeMedia : IMedia
{
    public List<(string Key, object Value)> Writes { get; } = [];

    public IMedia WriteString(string key, string value)
    {
        Writes.Add((key, value));
        return this;
    }

    public IMedia WriteOptionalString(string key, Option<string> value)
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

    public IMedia WriteGuid(string key, Guid value)
    {
        Writes.Add((key, value));
        return this;
    }

    public IMedia WriteInt32(string key, int value)
    {
        Writes.Add((key, value));
        return this;
    }

    public IMedia WriteStringArray(string key, IEnumerable<string> values)
    {
        Writes.Add((key, values.ToArray()));
        return this;
    }

    public T? GetValue<T>(string key)
    {
        var write = Writes.FirstOrDefault(w => w.Key == key);
        return write.Key is not null ? (T?)write.Value : default;
    }
}
