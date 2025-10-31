using System.Security.Claims;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Infrastructure.Identity;
using CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.TestDoubles;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Identity;

public sealed class HttpContextModuleUserTests
{
    [Fact]
    public void Constructor_WithNullHttpContextAccessor_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new HttpContextModuleUser(null!));
    }

    [Fact]
    public void IsInRole_WithMatchingRole_ReturnsTrue()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-123"),
            new Claim("module_role", "FormDesigner")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var result = moduleUser.IsInRole(ModuleRole.FormDesigner);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsInRole_WithNonMatchingRole_ReturnsFalse()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-123"),
            new Claim("module_role", "FormDesigner")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var result = moduleUser.IsInRole(ModuleRole.Supervisor);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsInRole_WithUnauthenticatedUser_ReturnsFalse()
    {
        // Arrange
        var identity = new ClaimsIdentity(); // No authentication type
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var result = moduleUser.IsInRole(ModuleRole.FormDesigner);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsInRole_WithNullHttpContext_ReturnsFalse()
    {
        // Arrange
        var accessor = FakeHttpContextAccessor.Create(null);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var result = moduleUser.IsInRole(ModuleRole.FormDesigner);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsInRole_IsCaseInsensitive()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-123"),
            new Claim("module_role", "formdesigner") // lowercase
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var result = moduleUser.IsInRole(ModuleRole.FormDesigner);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UserInfo_WithAllClaims_ReturnsCompleteUserInfo()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-123"),
            new Claim("preferred_username", "jdoe"),
            new Claim("name", "John Doe"),
            new Claim("email", "john.doe@example.com"),
            new Claim("module_role", "FormDesigner")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var userInfo = moduleUser.UserInfo();
        using var media = new FakeMedia();
        userInfo.Print(media);

        // Assert
        Assert.Equal("user-123", media.GetValue<string>("userId"));
        Assert.Equal("jdoe", media.GetValue<string>("username"));
        Assert.Equal("John Doe", media.GetValue<string>("name"));
        Assert.Equal("john.doe@example.com", media.GetValue<string>("email"));
    }

    [Fact]
    public void UserInfo_WithOnlyRequiredClaims_ReturnsMinimalUserInfo()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-456"),
            new Claim("module_role", "Supervisor")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var userInfo = moduleUser.UserInfo();
        using var media = new FakeMedia();
        userInfo.Print(media);

        // Assert
        Assert.Equal("user-456", media.GetValue<string>("userId"));
        Assert.Null(media.GetValue<string>("username"));
        Assert.Null(media.GetValue<string>("name"));
        Assert.Null(media.GetValue<string>("email"));
    }

    [Fact]
    public void UserInfo_WithGivenNameAndFamilyName_ComposesFullName()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-789"),
            new Claim("given_name", "Jane"),
            new Claim("family_name", "Smith"),
            new Claim("module_role", "Operator")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var userInfo = moduleUser.UserInfo();
        using var media = new FakeMedia();
        userInfo.Print(media);

        // Assert
        Assert.Equal("Jane Smith", media.GetValue<string>("name"));
    }

    [Fact]
    public void UserInfo_PrefersPreferredUsernameOverUsername()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-999"),
            new Claim("preferred_username", "preferred_user"),
            new Claim("username", "fallback_user"),
            new Claim("module_role", "FormDesigner")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var userInfo = moduleUser.UserInfo();
        using var media = new FakeMedia();
        userInfo.Print(media);

        // Assert
        Assert.Equal("preferred_user", media.GetValue<string>("username"));
    }

    [Fact]
    public void UserInfo_WithUnauthenticatedUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var identity = new ClaimsIdentity(); // No authentication type
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => moduleUser.UserInfo());
    }

    [Fact]
    public void UserInfo_WithMissingSubClaim_ThrowsInvalidOperationException()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("module_role", "FormDesigner")
            // Missing 'sub' claim
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => moduleUser.UserInfo());
        Assert.Contains("sub", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Print_WithValidUser_DelegatesToUserInfo()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-111"),
            new Claim("name", "Test User"),
            new Claim("module_role", "FormDesigner")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);
        using var media = new FakeMedia();

        // Act
        moduleUser.Print(media);

        // Assert
        Assert.Equal("user-111", media.GetValue<string>("userId"));
        Assert.Equal("Test User", media.GetValue<string>("name"));
    }

    [Fact]
    public void Print_WithNullMedia_ThrowsArgumentNullException()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-123"),
            new Claim("module_role", "FormDesigner")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => moduleUser.Print<object>(null!));
    }

    [Fact]
    public void UserInfo_IsLazilyEvaluated_OnlyOnce()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "user-222"),
            new Claim("module_role", "Supervisor")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var accessor = FakeHttpContextAccessor.Create(principal);
        var moduleUser = new HttpContextModuleUser(accessor);

        // Act
        var userInfo1 = moduleUser.UserInfo();
        var userInfo2 = moduleUser.UserInfo();

        // Assert - Should return the same instance (Lazy<T> guarantees single evaluation)
        Assert.Same(userInfo1, userInfo2);
    }
}
