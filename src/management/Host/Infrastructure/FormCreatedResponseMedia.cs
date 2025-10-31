using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;
using Microsoft.AspNetCore.Http;

namespace CascVel.Modules.Evaluations.Management.Host.Infrastructure;

/// <summary>
/// Decorates JsonMediaWriter to emit HTTP results for created form responses.
/// </summary>
internal sealed class FormCreatedResponseMedia : IMedia<IResult>, IDisposable
{
    private readonly JsonMediaWriter _inner;
    private Guid? _id;

    /// <summary>
    /// Initializes the media for capturing created form responses.
    /// </summary>
    public FormCreatedResponseMedia()
    {
        _inner = new JsonMediaWriter();
    }

    /// <summary>
    /// Writes a string value to the underlying JSON media.
    /// </summary>
    public IMedia With(string key, string value)
    {
        _inner.With(key, value);
        return this;
    }

    /// <summary>
    /// Writes an optional string value to the underlying JSON media.
    /// </summary>
    public IMedia With(string key, Option<string> value)
    {
        _inner.With(key, value);
        return this;
    }

    /// <summary>
    /// Writes a GUID value to the underlying JSON media.
    /// </summary>
    public IMedia With(string key, Guid value)
    {
        if (string.Equals(key, "formId", StringComparison.OrdinalIgnoreCase))
        {
            _id = value;
        }

        _inner.With(key, value);
        return this;
    }

    /// <summary>
    /// Writes an integer value to the underlying JSON media.
    /// </summary>
    public IMedia With(string key, int value)
    {
        _inner.With(key, value);
        return this;
    }

    /// <summary>
    /// Writes a collection of strings to the underlying JSON media.
    /// </summary>
    public IMedia With(string key, IEnumerable<string> values)
    {
        _inner.With(key, values);
        return this;
    }

    /// <summary>
    /// Writes a nested object to the underlying JSON media.
    /// </summary>
    public IMedia WithObject(string key, Action<IMedia> configure)
    {
        _inner.WithObject(key, configure);
        return this;
    }

    /// <summary>
    /// Completes the JSON document and returns the HTTP result.
    /// </summary>
    public IResult Output()
    {
        if (!_id.HasValue)
        {
            throw new InvalidOperationException("Form identifier is required in the response payload");
        }

        var payload = _inner.Output();
        return new FormCreatedContentResult(_id.Value, payload);
    }

    /// <summary>
    /// Disposes the decorated media resources.
    /// </summary>
    public void Dispose()
    {
        _inner.Dispose();
    }

    /// <summary>
    /// HTTP result that writes the created form payload.
    /// </summary>
    private sealed class FormCreatedContentResult : IResult
    {
        private readonly Guid _identifier;
        private readonly string _payload;

        /// <summary>
        /// Initializes the result with identifier and serialized payload.
        /// </summary>
        /// <param name="identifier">Identifier of the created form.</param>
        /// <param name="payload">Serialized JSON payload.</param>
        public FormCreatedContentResult(Guid identifier, string payload)
        {
            _identifier = identifier;
            _payload = payload;
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            httpContext.Response.StatusCode = StatusCodes.Status201Created;
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.Headers.Location = $"/forms/{_identifier}";
            await httpContext.Response.WriteAsync(_payload);
        }
    }
}
