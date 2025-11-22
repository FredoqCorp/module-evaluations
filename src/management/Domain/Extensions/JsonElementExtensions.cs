using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Common;

namespace CascVel.Modules.Evaluations.Management.Domain.Extensions;

static class JsonElementExtensions
{
    extension (JsonElement element)
    {
        public IEnumerable<JsonElement> Array(string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.Array)
            {
                return value.EnumerateArray();
            }
            return [];
        }

        public IEnumerable<T> Array<T>(string propertyName, Func<JsonElement, T> selector)
        {
            foreach (var item in element.Array(propertyName))
            {
                yield return selector(item);
            }
        }

        public int Number(string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.Number)
            {
                return value.GetInt32();
            }
            throw new InvalidOperationException($"Property '{propertyName}' is missing or not a number.");
        }

        public decimal Decimal(string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.Number)
            {
                return value.GetDecimal();
            }
            throw new InvalidOperationException($"Property '{propertyName}' is missing or not a decimal number.");
        }

        public string String(string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var value))
            {
                switch (value.ValueKind)
                {
                    case JsonValueKind.String:
                        return value.GetString() ?? throw new InvalidOperationException($"Property '{propertyName}' is null.");
                    case JsonValueKind.Null:
                        return string.Empty;
                }
            }
            throw new InvalidOperationException($"Property '{propertyName}' is missing or not a string.");
        }

        public string String()
        {
            return element.GetString() ?? throw new InvalidOperationException("Value is null or not a string.");
        }

        public Option<Guid> OptionalGuid(string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String)
            {
                var str = value.GetString();
                if (Guid.TryParse(str, out var guid))
                {
                    return Option.Of(guid);
                }
            }
            return Option.None<Guid>();
        }
    }
}
