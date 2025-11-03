using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Identity;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Identity;

/// <summary>
/// Immutable value object that encapsulates user identification and metadata.
/// </summary>
public sealed record UserInfo : IUserInfo
{
    private readonly UserId _id;
    private readonly Option<string> _username;
    private readonly Option<string> _name;
    private readonly Option<string> _email;

    /// <summary>
    /// Initializes user information with identifier and optional metadata.
    /// </summary>
    /// <param name="id">Unique user identifier from authentication token.</param>
    /// <param name="username">Optional username/login identifier.</param>
    /// <param name="name">Optional user display name.</param>
    /// <param name="email">Optional user email address.</param>
    public UserInfo(UserId id, Option<string> username, Option<string> name, Option<string> email)
    {
        _id = id;
        _username = username;
        _name = name;
        _email = email;
    }

    /// <summary>
    /// Prints the user information into the provided media.
    /// </summary>
    /// <param name="media">Target media that receives the printed representation.</param>
    public void Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        media.With("userId", _id.Value);
        media.With("username", _username);
        media.With("name", _name);
        media.With("email", _email);
    }
}
