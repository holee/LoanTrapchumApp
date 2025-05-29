namespace Applications.Interfaces.Identity;

public interface ICurrentUserAccessor
{
    /// <summary>
    /// Gets the current session information of the user.
    /// </summary>
    SessionInfo? SessionInfo { get; }
}
