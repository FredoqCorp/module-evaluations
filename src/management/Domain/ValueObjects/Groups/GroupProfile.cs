namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;

/// <summary>
/// Immutable value object that captures the identity and descriptive attributes of a group.
/// </summary>
public readonly record struct GroupProfile
{
    /// <summary>
    /// Initializes the group profile with identifier, title, and description.
    /// </summary>
    /// <param name="id">Unique identifier of the group.</param>
    /// <param name="title">Title of the group.</param>
    /// <param name="description">Description of the group.</param>
    public GroupProfile(GroupId id, GroupTitle title, GroupDescription description)
    {
        Id = id;
        Title = title;
        Description = description;
    }

    /// <summary>
    /// Unique identifier of the group.
    /// </summary>
    public GroupId Id { get; init; }

    /// <summary>
    /// Title of the group.
    /// </summary>
    public GroupTitle Title { get; init; }

    /// <summary>
    /// Description of the group.
    /// </summary>
    public GroupDescription Description { get; init; }
}
