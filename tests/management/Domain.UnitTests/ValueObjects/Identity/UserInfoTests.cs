using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Identity;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.TestDoubles;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Identity;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.ValueObjects.Identity;

public sealed class UserInfoTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Arrange
        var userId = new UserId("user-123");
        var username = Option.Of("jdoe");
        var name = Option.Of("John Doe");
        var email = Option.Of("john.doe@example.com");

        // Act
        var userInfo = new UserInfo(userId, username, name, email);

        // Assert
        Assert.NotNull(userInfo);
    }

    [Fact]
    public void Constructor_WithAllOptionalFieldsEmpty_CreatesInstance()
    {
        // Arrange
        var userId = new UserId("user-123");
        var username = Option.None<string>();
        var name = Option.None<string>();
        var email = Option.None<string>();

        // Act
        var userInfo = new UserInfo(userId, username, name, email);

        // Assert
        Assert.NotNull(userInfo);
    }

    [Fact]
    public void Print_WithAllFields_WritesAllFieldsToMedia()
    {
        // Arrange
        var userId = new UserId("user-123");
        var username = Option.Of("jdoe");
        var name = Option.Of("John Doe");
        var email = Option.Of("john.doe@example.com");
        var userInfo = new UserInfo(userId, username, name, email);
        var media = new FakeMedia();

        // Act
        userInfo.Print(media);

        // Assert
        Assert.Equal(4, media.Writes.Count);
        Assert.Contains(("userId", "user-123"), media.Writes);
        Assert.Contains(("username", "jdoe"), media.Writes);
        Assert.Contains(("name", "John Doe"), media.Writes);
        Assert.Contains(("email", "john.doe@example.com"), media.Writes);
    }

    [Fact]
    public void Print_WithOnlyUserId_WritesOnlyUserId()
    {
        // Arrange
        var userId = new UserId("user-456");
        var username = Option.None<string>();
        var name = Option.None<string>();
        var email = Option.None<string>();
        var userInfo = new UserInfo(userId, username, name, email);
        var media = new FakeMedia();

        // Act
        userInfo.Print(media);

        // Assert
        Assert.Single(media.Writes);
        Assert.Contains(("userId", "user-456"), media.Writes);
    }

    [Fact]
    public void Print_WithPartialFields_WritesOnlyPresentFields()
    {
        // Arrange
        var userId = new UserId("user-789");
        var username = Option.Of("alice");
        var name = Option.None<string>();
        var email = Option.Of("alice@example.com");
        var userInfo = new UserInfo(userId, username, name, email);
        var media = new FakeMedia();

        // Act
        userInfo.Print(media);

        // Assert
        Assert.Equal(3, media.Writes.Count);
        Assert.Contains(("userId", "user-789"), media.Writes);
        Assert.Contains(("username", "alice"), media.Writes);
        Assert.Contains(("email", "alice@example.com"), media.Writes);
        Assert.DoesNotContain(media.Writes, w => w.Key == "name");
    }

    [Fact]
    public void Print_WithNullMedia_ThrowsArgumentNullException()
    {
        // Arrange
        var userId = new UserId("user-123");
        var userInfo = new UserInfo(
            userId,
            Option.None<string>(),
            Option.None<string>(),
            Option.None<string>());

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => userInfo.Print<object>(null!));
    }

    [Fact]
    public void UserInfo_IsRecord_SupportsValueEquality()
    {
        // Arrange
        var userId = new UserId("user-123");
        var username = Option.Of("jdoe");
        var name = Option.Of("John Doe");
        var email = Option.Of("john.doe@example.com");

        var userInfo1 = new UserInfo(userId, username, name, email);
        var userInfo2 = new UserInfo(userId, username, name, email);

        // Act & Assert
        Assert.Equal(userInfo1, userInfo2);
        Assert.True(userInfo1 == userInfo2);
    }

    [Fact]
    public void UserInfo_WithDifferentData_AreNotEqual()
    {
        // Arrange
        var userInfo1 = new UserInfo(
            new UserId("user-123"),
            Option.Of("jdoe"),
            Option.Of("John Doe"),
            Option.Of("john@example.com"));

        var userInfo2 = new UserInfo(
            new UserId("user-456"),
            Option.Of("jdoe"),
            Option.Of("John Doe"),
            Option.Of("john@example.com"));

        // Act & Assert
        Assert.NotEqual(userInfo1, userInfo2);
        Assert.False(userInfo1 == userInfo2);
    }

    [Fact]
    public void UserInfo_ImplementsIUserInfo_Interface()
    {
        // Arrange
        var userId = new UserId("user-123");
        var userInfo = new UserInfo(
            userId,
            Option.Of("jdoe"),
            Option.Of("John Doe"),
            Option.Of("john@example.com"));

        // Act & Assert
        Assert.IsAssignableFrom<IUserInfo>(userInfo);
    }
}
