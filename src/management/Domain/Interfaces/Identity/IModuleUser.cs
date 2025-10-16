using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Identity;

/// <summary>
/// Behavioral contract for accessing authenticated module user information and authorization context.
/// </summary>
public interface IModuleUser
{
    /// <summary>
    /// Determines whether the current user is assigned to the specified module role.
    /// </summary>
    /// <param name="role">Module role to check.</param>
    /// <returns>True if the user has the specified role; otherwise, false.</returns>
    bool IsInRole(ModuleRole role);

    /// <summary>
    /// Gets the current user information.
    /// </summary>
    /// <returns>User information including identifier and metadata.</returns>
    IUserInfo UserInfo();

    /// <summary>
    /// Prints the current user information into the provided media.
    /// </summary>
    /// <param name="media">Target media that receives the printed user representation.</param>
    void Print(IMedia media);
}
