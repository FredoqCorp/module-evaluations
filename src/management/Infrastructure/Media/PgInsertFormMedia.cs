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
    private readonly List<Dictionary<string, object>> _groups = [];
    private readonly List<Dictionary<string, object>> _criteria = [];
    private readonly DateTimeOffset _stamp;

    /// <summary>
    /// Creates an instance bound to the specified timestamp.
    /// </summary>
    /// <param name="stamp">Timestamp applied to created rows.</param>
    public PgInsertFormMedia(DateTimeOffset stamp)
    {
        _stamp = stamp;
    }

    /// <inheritdoc />
    public IMedia With(string key, string value)
    {
        _form[key] = value;
        return this;
    }

    /// <inheritdoc />
    public IMedia With(string key, Option<string> value)
    {
        if (value.IsSome)
        {
            value.Map(text =>
            {
                _form[key] = text;
                return text;
            });
        }

        return this;
    }

    /// <inheritdoc />
    public IMedia With(string key, Guid value)
    {
        _form[key] = value;
        return this;
    }

    /// <inheritdoc />
    public IMedia With(string key, int value)
    {
        _form[key] = value;
        return this;
    }

    /// <inheritdoc />
    public IMedia With(string key, IEnumerable<string> values)
    {
        _form[key] = values.ToArray();
        return this;
    }

    /// <inheritdoc />
    public IMedia WithObject(string key, Action<IMedia> configure)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(configure);

        using var recorder = new ObjectMedia();
        configure(recorder);

        var snapshot = recorder.Snapshot();
        if (string.Equals(key, "group", StringComparison.OrdinalIgnoreCase))
        {
            _groups.Add(snapshot);
            return this;
        }

        if (string.Equals(key, "criterion", StringComparison.OrdinalIgnoreCase))
        {
            _criteria.Add(snapshot);
            return this;
        }

        throw new InvalidOperationException($"Unsupported printable object '{key}'");
    }

    /// <inheritdoc />
    public string Output()
    {
        var builder = new StringBuilder();
        builder.AppendLine(FormStatement());
        foreach (var snapshot in _groups)
        {
            builder.AppendLine(GroupStatement(snapshot));
        }

        foreach (var snapshot in _criteria)
        {
            builder.AppendLine(CriterionStatement(snapshot));
        }

        return builder.ToString().TrimEnd();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _form.Clear();
        _groups.Clear();
        _criteria.Clear();
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
        var groupType = (string)snapshot["groupType"];
        var orderIndex = (int)snapshot["orderIndex"];
        snapshot.TryGetValue("weightBasisPoints", out var weightValue);
        var weight = weightValue is int basis ? basis : (int?)null;
        var stamp = TimestampLiteral();

        return $"INSERT INTO form_groups (id, form_id, parent_id, title, description, group_type, weight_basis_points, order_index, created_at) VALUES ({GuidLiteral(id)}, {GuidLiteral(formId)}, {NullableGuidLiteral(parentId)}, {TextLiteral(title)}, {TextLiteral(description)}, {TextLiteral(groupType)}, {NullableIntLiteral(weight)}, {orderIndex.ToString(CultureInfo.InvariantCulture)}, {stamp});";
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
        var criterionType = (string)snapshot["criterionType"];
        var orderIndex = (int)snapshot["orderIndex"];
        var ratingOptions = (string)snapshot["ratingOptions"];
        snapshot.TryGetValue("weightBasisPoints", out var weightValue);
        var weight = weightValue is int basis ? basis : (int?)null;
        var stamp = TimestampLiteral();

        return $"INSERT INTO form_criteria (id, form_id, group_id, title, text, criterion_type, weight_basis_points, rating_options, order_index, created_at) VALUES ({GuidLiteral(id)}, {NullableGuidLiteral(formId)}, {NullableGuidLiteral(groupId)}, {TextLiteral(title)}, {TextLiteral(text)}, {TextLiteral(criterionType)}, {NullableIntLiteral(weight)}, {JsonLiteral(ratingOptions)}, {orderIndex.ToString(CultureInfo.InvariantCulture)}, {stamp});";
    }

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
            _values.Clear();
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
        public IMedia WithObject(string key, Action<IMedia> configure)
        {
            throw new InvalidOperationException("Nested objects are not supported");
        }
    }
}
