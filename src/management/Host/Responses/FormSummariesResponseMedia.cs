using System;
using System.Collections.Generic;
using System.Text;
using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;
using Microsoft.AspNetCore.Http;

namespace CascVel.Modules.Evaluations.Management.Host.Responses;

/// <summary>
/// Decorates JsonMediaWriter to emit HTTP results for form summaries responses.
/// </summary>
internal sealed class FormSummariesResponseMedia : IMedia<IResult>
{
    private readonly IMedia<string> _inner;
    private readonly HttpResponse _response;

    /// <summary>
    /// Initializes the media for capturing form summaries responses.
    /// </summary>
    /// <param name="response">HTTP response used to configure headers.</param>
    public FormSummariesResponseMedia(HttpResponse response)
        : this(response, new JsonMediaWriter())
    {
    }

    /// <summary>
    /// Initializes the media with a custom inner media instance.
    /// </summary>
    /// <param name="response">HTTP response used to configure headers.</param>
    /// <param name="media">Inner media that produces textual JSON output.</param>
    public FormSummariesResponseMedia(HttpResponse response, IMedia<string> media)
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
    /// Writes an array of nested objects to the underlying JSON media.
    /// </summary>
    public IMedia WithArray(string key, IEnumerable<Action<IMedia>> items)
    {
        _inner.WithArray(key, items);
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
        var payload = _inner.Output();
        return TypedResults.Content(payload, "application/json", Encoding.UTF8, StatusCodes.Status200OK);
    }

    /// <summary>
    /// Disposes the decorated media resources.
    /// </summary>
    public void Dispose()
    {
        _inner.Dispose();
    }
}
