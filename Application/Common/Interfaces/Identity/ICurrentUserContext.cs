namespace Application.Interfaces.Identity;

public interface ICurrentUserContext
{
    /// <summary>
    /// Gets or sets the session information of the current user.
    /// </summary>
    SessionInfo? SessionInfo { get; set; }
}
