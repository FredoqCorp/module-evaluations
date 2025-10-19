using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.TestDoubles;

/// <summary>
/// Fake implementation of IMedia for testing printer pattern.
/// </summary>
internal sealed class FakeMedia : IMedia<object>
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
        Writes.Add((key, values.ToList()));
        return this;
    }

    public IMedia StartObject()
    {
        Writes.Add((Guid.NewGuid().ToString(), "[START_OBJECT]"));
        return this;
    }

    public IMedia StartObject(string key)
    {
        Writes.Add((key, "[START_OBJECT]"));
        return this;
    }

    public IMedia EndObject()
    {
        Writes.Add(("[END_OBJECT]", "[END_OBJECT]"));
        return this;
    }

    public object Output()
    {
        return Writes;
    }
}
