using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.TestDoubles;

internal sealed class FakeHttpContextAccessor : IHttpContextAccessor
{
    public HttpContext? HttpContext { get; set; }

    public static FakeHttpContextAccessor Create(ClaimsPrincipal? principal = null)
    {
        var accessor = new FakeHttpContextAccessor();

        if (principal is not null)
        {
            accessor.HttpContext = new FakeHttpContext
            {
                User = principal
            };
        }

        return accessor;
    }
}

internal sealed class FakeHttpContext : HttpContext
{
    public override IFeatureCollection Features => throw new NotImplementedException();
    public override HttpRequest Request => throw new NotImplementedException();
    public override HttpResponse Response => throw new NotImplementedException();
    public override ConnectionInfo Connection => throw new NotImplementedException();
    public override WebSocketManager WebSockets => throw new NotImplementedException();
    #pragma warning disable S1133 // Deprecated code should be removed
    [Obsolete("Required to implement obsolete HttpContext.Authentication property")]
    public override AuthenticationManager Authentication => throw new NotImplementedException();
    #pragma warning restore S1133
    public override ClaimsPrincipal User { get; set; } = new();
    public override IDictionary<object, object?> Items { get; set; } = new Dictionary<object, object?>();
    public override IServiceProvider RequestServices { get; set; } = null!;
    public override CancellationToken RequestAborted { get; set; }
    public override string TraceIdentifier { get; set; } = string.Empty;
    public override ISession Session { get; set; } = null!;

    public override void Abort() => throw new NotImplementedException();
}
