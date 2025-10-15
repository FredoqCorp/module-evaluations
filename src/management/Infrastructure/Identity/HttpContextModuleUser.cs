using System.Security.Claims;
using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Identity;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Identity;
using Microsoft.AspNetCore.Http;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Identity;

/// <summary>
/// Extracts module user information from HTTP context claims principal.
/// </summary>
internal sealed class HttpContextModuleUser : IModuleUser
{
    private const string ModuleRoleClaimType = "module_role";
    private readonly Lazy<UserInfo> _userInfo;
    private readonly ClaimsPrincipal? _principal;

    /// <summary>
    /// Initializes the user accessor with HTTP context accessor.
    /// </summary>
    /// <param name="httpContextAccessor">Accessor for current HTTP context.</param>
    public HttpContextModuleUser(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);

        _principal = httpContextAccessor.HttpContext?.User;
        _userInfo = new Lazy<UserInfo>(ExtractUserInfo);
    }

    /// <summary>
    /// Determines whether the current user is assigned to the specified module role.
    /// </summary>
    /// <param name="role">Module role to check.</param>
    /// <returns>True if the user has the specified role; otherwise, false.</returns>
    public bool IsInRole(ModuleRole role)
    {
        if (_principal is null || !_principal.Identity?.IsAuthenticated == true)
        {
            return false;
        }

        var roleClaim = _principal.FindFirst(ModuleRoleClaimType)?.Value;
        return roleClaim is not null &&
               Enum.TryParse<ModuleRole>(roleClaim, ignoreCase: true, out var userRole) &&
               userRole == role;
    }

    /// <summary>
    /// Gets the current user information.
    /// </summary>
    /// <returns>User information including identifier and metadata.</returns>
    public IUserInfo GetUserInfo()
    {
        return _userInfo.Value;
    }

    /// <summary>
    /// Prints the current user information into the provided media.
    /// </summary>
    /// <param name="media">Target media that receives the printed user representation.</param>
    public void Print(IMedia media)
    {
        ArgumentNullException.ThrowIfNull(media);

        _userInfo.Value.Print(media);
    }

    private UserInfo ExtractUserInfo()
    {
        if (_principal is null || !_principal.Identity?.IsAuthenticated == true)
        {
            throw new InvalidOperationException("User is not authenticated");
        }

        // Extract user ID from 'sub' claim (standard JWT subject claim)
        var subClaim = (_principal.FindFirst(ClaimTypes.NameIdentifier)
                       ?? _principal.FindFirst("sub")) ?? throw new InvalidOperationException("User identifier claim ('sub' or NameIdentifier) not found in token");
        var userId = new UserId(subClaim.Value);

        // Extract optional username/login
        var usernameClaim = _principal.FindFirst("preferred_username")
                           ?? _principal.FindFirst("username");
        var username = usernameClaim is not null
            ? Option.Of(usernameClaim.Value)
            : Option.None<string>();

        // Extract optional name from claims (try multiple claim types)
        var name = ExtractName();

        // Extract optional email
        var emailClaim = _principal.FindFirst(ClaimTypes.Email)
                        ?? _principal.FindFirst("email");
        var email = emailClaim is not null
            ? Option.Of(emailClaim.Value)
            : Option.None<string>();

        return new UserInfo(userId, username, name, email);
    }

    private Option<string> ExtractName()
    {
        // Try to construct full name from given_name and family_name
        var givenName = _principal?.FindFirst("given_name")?.Value;
        var familyName = _principal?.FindFirst("family_name")?.Value;

        if (!string.IsNullOrWhiteSpace(givenName) && !string.IsNullOrWhiteSpace(familyName))
        {
            return Option.Of($"{givenName} {familyName}");
        }

        // Fall back to 'name' claim
        var nameClaim = _principal?.FindFirst(ClaimTypes.Name)
                       ?? _principal?.FindFirst("name");

        return nameClaim is not null
            ? Option.Of(nameClaim.Value)
            : Option.None<string>();
    }
}
