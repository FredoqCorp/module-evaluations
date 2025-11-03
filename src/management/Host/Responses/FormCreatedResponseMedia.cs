using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;

namespace CascVel.Modules.Evaluations.Management.Host.Responses;

/// <summary>
/// Decorates JsonMediaWriter to emit HTTP results for created form responses.
/// </summary>
internal sealed class FormCreatedResponseMedia : IMedia<IResult>
{
    private readonly IMedia<string> _inner;
    private readonly HttpResponse _response;
    private Guid? _id;

    /// <summary>
    /// Initializes the media for capturing created form responses.
    /// </summary>
    /// <param name="response">HTTP response used to set headers.</param>
    public FormCreatedResponseMedia(HttpResponse response): this(response, new JsonMediaWriter())
    {
    }

    public FormCreatedResponseMedia(HttpResponse response, IMedia<string> media)
    {
        ArgumentNullException.ThrowIfNull(response);
        ArgumentNullException.ThrowIfNull(media);

        _response = response;
        _inner = media;
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
        _response.Headers.Location = $"/forms/{_id.Value}";
        return TypedResults.Content(payload, "application/json", statusCode: StatusCodes.Status201Created);
    }

    /// <summary>
    /// Disposes the decorated media resources.
    /// </summary>
    public void Dispose()
    {
        _inner.Dispose();
    }

}
