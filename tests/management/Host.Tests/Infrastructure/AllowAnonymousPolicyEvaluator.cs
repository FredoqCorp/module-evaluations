using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace CascVel.Modules.Evaluations.Management.Host.Tests.Infrastructure;

/// <summary>
/// Policy evaluator that allows all requests without authentication for testing.
/// </summary>
public sealed class AllowAnonymousPolicyEvaluator : IPolicyEvaluator
{
    /// <summary>
    /// Authenticates the request by creating a fake authenticated user.
    /// </summary>
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(policy);
        ArgumentNullException.ThrowIfNull(context);

        // Create a fake authenticated user with FormDesigner role for tests
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-123"),
            new Claim("sub", "test-user-123"),
            new Claim("module_role", "FormDesigner"),
            new Claim("name", "Test User"),
            new Claim("email", "test@example.com")
        };

        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    /// <summary>
    /// Authorizes the request by always returning success.
    /// </summary>
    public Task<PolicyAuthorizationResult> AuthorizeAsync(
        AuthorizationPolicy policy,
        AuthenticateResult authenticationResult,
        HttpContext context,
        object? resource)
    {
        ArgumentNullException.ThrowIfNull(policy);
        ArgumentNullException.ThrowIfNull(context);

        return Task.FromResult(PolicyAuthorizationResult.Success());
    }
}
