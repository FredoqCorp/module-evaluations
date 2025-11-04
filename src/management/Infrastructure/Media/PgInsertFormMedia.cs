using System.Globalization;
using System.Text;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Media;

/// <summary>
/// Builds PostgreSQL insert statements from the printable form aggregate.
/// </summary>
internal sealed class PgInsertFormMedia : IMedia<string>
{
    private readonly Dictionary<string, object> _form = [];
    private readonly Dictionary<string, List<Dictionary<string, object>>> _collections = new(StringComparer.OrdinalIgnoreCase)
    {
        ["group"] = [],
        ["criterion"] = []
    };
    private readonly Stack<Scope> _scopes = new();
    private readonly DateTimeOffset _stamp;

    /// <summary>
    /// Creates an instance bound to the specified timestamp.
    /// </summary>
    /// <param name="stamp">Timestamp applied to created rows.</param>
    public PgInsertFormMedia(DateTimeOffset stamp)
    {
        _stamp = stamp;
        _scopes.Push(new Scope("form", _form));
    }

    /// <inheritdoc />
    public IMedia With(string key, string value)
    {
        Context()[key] = value;
        return this;
    }

    /// <inheritdoc />
    public IMedia With(string key, Option<string> value)
    {
        if (value.IsSome)
        {
            value.Map(text =>
            {
                Context()[key] = text;
                return text;
            });
        }

        return this;
    }

    /// <inheritdoc />
    public IMedia With(string key, Guid value)
    {
        Context()[key] = value;
        return this;
    }

    /// <inheritdoc />
    public IMedia With(string key, int value)
    {
        Context()[key] = value;
        return this;
    }

    /// <inheritdoc />
    public IMedia With(string key, IEnumerable<string> values)
    {
        Context()[key] = values.ToArray();
        return this;
    }

    /// <inheritdoc />
    public IMedia WithArray(string key, IEnumerable<Action<IMedia>> items)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(items);

        if (string.Equals(key, "groups", StringComparison.OrdinalIgnoreCase))
        {
            var snapshots = CaptureArray("group", items, StoreGroup);
            Context()[key] = JsonSerializer.Serialize(snapshots);
            return this;
        }

        if (string.Equals(key, "criteria", StringComparison.OrdinalIgnoreCase))
        {
            var snapshots = CaptureArray("criterion", items, StoreCriterion);
            Context()[key] = JsonSerializer.Serialize(snapshots);
            return this;
        }

        if (string.Equals(key, "ratingOptions", StringComparison.OrdinalIgnoreCase))
        {
            var snapshots = CaptureArray("ratingOption", items, null);
            var mapped = new Dictionary<string, Dictionary<string, object>>();
            for (var index = 0; index < snapshots.Count; index++)
            {
                var name = index.ToString(CultureInfo.InvariantCulture);
                mapped[name] = snapshots[index];
            }

            Context()[key] = JsonSerializer.Serialize(mapped);
            return this;
        }

        var genericSnapshots = CaptureArray("object", items, null);
        Context()[key] = JsonSerializer.Serialize(genericSnapshots);
        return this;
    }

    /// <inheritdoc />
    public IMedia WithObject(string key, Action<IMedia> configure)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(configure);

        if (string.Equals(key, "group", StringComparison.OrdinalIgnoreCase))
        {
            CaptureObject("group", configure, StoreGroup);
            return this;
        }

        if (string.Equals(key, "criterion", StringComparison.OrdinalIgnoreCase))
        {
            CaptureObject("criterion", configure, StoreCriterion);
            return this;
        }

        throw new InvalidOperationException($"Unsupported printable object '{key}'");
    }

    /// <inheritdoc />
    public string Output()
    {
        var builder = new StringBuilder();
        builder.AppendLine(FormStatement());
        foreach (var snapshot in Collection("group"))
        {
            builder.AppendLine(GroupStatement(snapshot));
        }

        foreach (var snapshot in Collection("criterion"))
        {
            builder.AppendLine(CriterionStatement(snapshot));
        }

        return builder.ToString().TrimEnd();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _form.Clear();
        foreach (var collection in _collections.Values)
        {
            collection.Clear();
        }

        _scopes.Clear();
    }

    /// <summary>
    /// Creates the SQL statement for the forms table.
    /// </summary>
    /// <returns>SQL insert statement.</returns>
    private string FormStatement()
    {
        var id = (Guid)_form["formId"];
        var name = (string)_form["name"];
        var description = (string)_form["description"];
        var code = (string)_form["code"];
        var calculation = (string)_form["calculation"];
        var tags = _form.TryGetValue("tags", out var tagsValue) ? (string[])tagsValue : Array.Empty<string>();
        var tagsJson = JsonSerializer.Serialize(tags);
        var stamp = TimestampLiteral();

        return $"INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES ({GuidLiteral(id)}, {TextLiteral(name)}, {TextLiteral(description)}, {TextLiteral(code)}, {JsonLiteral(tagsJson)}, {TextLiteral(calculation)}, {stamp}, {stamp});";
    }

    /// <summary>
    /// Creates the SQL statement for the form_groups table.
    /// </summary>
    /// <param name="snapshot">Recorded group values.</param>
    /// <returns>SQL insert statement.</returns>
    private string GroupStatement(Dictionary<string, object> snapshot)
    {
        var id = (Guid)snapshot["id"];
        var formId = (Guid)snapshot["formId"];
        snapshot.TryGetValue("parentId", out var parentValue);
        var parentId = parentValue is Guid parent ? parent : (Guid?)null;
        var title = (string)snapshot["title"];
        var description = (string)snapshot["description"];
        var orderIndex = (int)snapshot["orderIndex"];
        snapshot.TryGetValue("weightBasisPoints", out var weightValue);
        var weight = weightValue is int basis ? basis : (int?)null;
        var stamp = TimestampLiteral();

        return $"INSERT INTO form_groups (id, form_id, parent_id, title, description, weight_basis_points, order_index, created_at) VALUES ({GuidLiteral(id)}, {GuidLiteral(formId)}, {NullableGuidLiteral(parentId)}, {TextLiteral(title)}, {TextLiteral(description)}, {NullableIntLiteral(weight)}, {orderIndex.ToString(CultureInfo.InvariantCulture)}, {stamp});";
    }

    /// <summary>
    /// Creates the SQL statement for the form_criteria table.
    /// </summary>
    /// <param name="snapshot">Recorded criterion values.</param>
    /// <returns>SQL insert statement.</returns>
    private string CriterionStatement(Dictionary<string, object> snapshot)
    {
        var id = (Guid)snapshot["id"];
        snapshot.TryGetValue("formId", out var formValue);
        var formId = formValue is Guid form ? form : (Guid?)null;
        snapshot.TryGetValue("groupId", out var groupValue);
        var groupId = groupValue is Guid group ? group : (Guid?)null;
        var title = (string)snapshot["title"];
        var text = (string)snapshot["text"];
        var orderIndex = (int)snapshot["orderIndex"];
        var ratingOptions = (string)snapshot["ratingOptions"];
        snapshot.TryGetValue("weightBasisPoints", out var weightValue);
        var weight = weightValue is int basis ? basis : (int?)null;
        var stamp = TimestampLiteral();

        return $"INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES ({GuidLiteral(id)}, {NullableGuidLiteral(formId)}, {NullableGuidLiteral(groupId)}, {TextLiteral(title)}, {TextLiteral(text)}, {NullableIntLiteral(weight)}, {JsonLiteral(ratingOptions)}, {orderIndex.ToString(CultureInfo.InvariantCulture)}, {stamp});";
    }

    /// <summary>
    /// Provides access to the active context dictionary used for writing values.
    /// </summary>
    /// <returns>Dictionary representing the current media scope.</returns>
    private Dictionary<string, object> Context()
    {
        return _scopes.TryPeek(out var scope) ? scope.Values : _form;
    }

    /// <summary>
    /// Resolves the form identifier required for relational records.
    /// </summary>
    /// <returns>Form identifier.</returns>
    private Guid FormId()
    {
        if (_form.TryGetValue("formId", out var value) && value is Guid identifier)
        {
            return identifier;
        }

        throw new InvalidOperationException("Form identifier is missing from the printable payload");
    }

    /// <summary>
    /// Captures array items into snapshots and optionally stores relational representations.
    /// </summary>
    /// <param name="kind">Logical kind of items being captured.</param>
    /// <param name="items">Enumeration of item printers.</param>
    /// <param name="store">Optional storage callback that enriches and stores relational data.</param>
    /// <returns>Snapshots captured from the printers.</returns>
    private List<Dictionary<string, object>> CaptureArray(string kind, IEnumerable<Action<IMedia>> items, Action<Dictionary<string, object>, Scope>? store)
    {
        var parent = _scopes.Peek();
        var snapshots = new List<Dictionary<string, object>>();
        foreach (var item in items)
        {
            ArgumentNullException.ThrowIfNull(item);
            var values = new Dictionary<string, object>();
            _scopes.Push(new Scope(kind, values));
            try
            {
                item(this);
            }
            finally
            {
                _scopes.Pop();
            }

            snapshots.Add(new Dictionary<string, object>(values));

            if (store is not null)
            {
                var record = new Dictionary<string, object>(values);
                store(record, parent);
            }
        }

        return snapshots;
    }

    /// <summary>
    /// Captures a nested object and persists it using the provided storage callback.
    /// </summary>
    /// <param name="kind">Logical kind of the nested object.</param>
    /// <param name="configure">Printer that populates the object.</param>
    /// <param name="store">Storage callback that enriches and stores relational data.</param>
    private void CaptureObject(string kind, Action<IMedia> configure, Action<Dictionary<string, object>, Scope> store)
    {
        var parent = _scopes.Peek();
        var values = new Dictionary<string, object>();
        _scopes.Push(new Scope(kind, values));
        try
        {
            configure(this);
        }
        finally
        {
            _scopes.Pop();
        }

        var record = new Dictionary<string, object>(values);
        store(record, parent);
    }

    /// <summary>
    /// Stores group data enriched with relational identifiers.
    /// </summary>
    /// <param name="record">Group values to persist.</param>
    /// <param name="parent">Parent scope describing the caller context.</param>
    private void StoreGroup(Dictionary<string, object> record, Scope parent)
    {
        record["formId"] = FormId();
        if (string.Equals(parent.Kind, "group", StringComparison.OrdinalIgnoreCase) &&
            parent.Values.TryGetValue("id", out var parentValue) &&
            parentValue is Guid parentId)
        {
            record["parentId"] = parentId;
        }

        Collection("group").Add(record);
    }

    /// <summary>
    /// Stores criterion data enriched with relational identifiers.
    /// </summary>
    /// <param name="record">Criterion values to persist.</param>
    /// <param name="parent">Parent scope describing the caller context.</param>
    private void StoreCriterion(Dictionary<string, object> record, Scope parent)
    {
        record["formId"] = FormId();
        if (string.Equals(parent.Kind, "group", StringComparison.OrdinalIgnoreCase) &&
            parent.Values.TryGetValue("id", out var groupValue) &&
            groupValue is Guid groupId)
        {
            record["groupId"] = groupId;
        }

        Collection("criterion").Add(record);
    }

    /// <summary>
    /// Returns the mutable collection that aggregates relational snapshots.
    /// </summary>
    /// <param name="kind">Logical kind of collection requested.</param>
    /// <returns>Collection associated with the requested kind.</returns>
    private List<Dictionary<string, object>> Collection(string kind)
    {
        if (_collections.TryGetValue(kind, out var entries))
        {
            return entries;
        }

        var created = new List<Dictionary<string, object>>();
        _collections[kind] = created;
        return created;
    }

    /// <summary>
    /// Describes the active writing scope in the media pipeline.
    /// </summary>
    private sealed record Scope(string Kind, Dictionary<string, object> Values);

    /// <summary>
    /// Produces SQL literal for GUID values.
    /// </summary>
    /// <param name="value">GUID value.</param>
    /// <returns>SQL literal.</returns>
    private static string GuidLiteral(Guid value)
    {
        return $"'{value:D}'::uuid";
    }

    /// <summary>
    /// Produces SQL literal for nullable GUID values.
    /// </summary>
    /// <param name="value">Nullable GUID value.</param>
    /// <returns>SQL literal or NULL.</returns>
    private static string NullableGuidLiteral(Guid? value)
    {
        return value.HasValue ? GuidLiteral(value.Value) : "NULL";
    }

    /// <summary>
    /// Produces SQL literal for text values.
    /// </summary>
    /// <param name="value">Text value.</param>
    /// <returns>SQL literal.</returns>
    private static string TextLiteral(string value)
    {
        return $"'{value.Replace("'", "''", StringComparison.Ordinal)}'";
    }

    /// <summary>
    /// Produces SQL literal for nullable integers.
    /// </summary>
    /// <param name="value">Nullable integer value.</param>
    /// <returns>SQL literal or NULL.</returns>
    private static string NullableIntLiteral(int? value)
    {
        return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : "NULL";
    }

    /// <summary>
    /// Produces SQL literal for JSON payloads.
    /// </summary>
    /// <param name="value">JSON payload.</param>
    /// <returns>SQL literal for JSONB.</returns>
    private static string JsonLiteral(string value)
    {
        return $"{TextLiteral(value)}::jsonb";
    }

    /// <summary>
    /// Produces SQL literal for timestamps with time zones.
    /// </summary>
    /// <returns>SQL literal.</returns>
    private string TimestampLiteral()
    {
        var text = _stamp.ToString("O", CultureInfo.InvariantCulture);
        return $"{TextLiteral(text)}::timestamptz";
    }

    /// <summary>
    /// Nested media used for capturing object values.
    /// </summary>
    private sealed class ObjectMedia : IMedia
    {
        private readonly Dictionary<string, object> _values = new();

        /// <summary>
        /// Returns the captured values.
        /// </summary>
        /// <returns>Snapshot of values.</returns>
        public Dictionary<string, object> Snapshot()
        {
            return _values;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public IMedia With(string key, string value)
        {
            _values[key] = value;
            return this;
        }

        /// <inheritdoc />
        public IMedia With(string key, Option<string> value)
        {
            if (value.IsSome)
            {
                value.Map(text =>
                {
                    _values[key] = text;
                    return text;
                });
            }

            return this;
        }

        /// <inheritdoc />
        public IMedia With(string key, Guid value)
        {
            _values[key] = value;
            return this;
        }

        /// <inheritdoc />
        public IMedia With(string key, int value)
        {
            _values[key] = value;
            return this;
        }

        /// <inheritdoc />
        public IMedia With(string key, IEnumerable<string> values)
        {
            _values[key] = values.ToArray();
            return this;
        }

        /// <inheritdoc />
        public IMedia WithArray(string key, IEnumerable<Action<IMedia>> items)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);
            ArgumentNullException.ThrowIfNull(items);

            var snapshots = new List<Dictionary<string, object>>();
            foreach (var item in items)
            {
                ArgumentNullException.ThrowIfNull(item);
                using var recorder = new ObjectMedia();
                item(recorder);
                snapshots.Add(recorder.Snapshot());
            }

            _values[key] = JsonSerializer.Serialize(snapshots);
            return this;
        }

        /// <inheritdoc />
        public IMedia WithObject(string key, Action<IMedia> configure)
        {
            throw new InvalidOperationException("Nested objects are not supported");
        }
    }
}
