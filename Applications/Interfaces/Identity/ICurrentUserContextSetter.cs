using System.Security.Claims;

namespace Applications.Interfaces.Identity;

public interface ICurrentUserContextSetter
{
    /// <summary>
    /// Sets the current user context with the provided session information.
    /// </summary>
    /// <param name="user">The session information of the current user.</param>
    void SetCurrentUser(ClaimsPrincipal user);

    /// <summary>
    /// Clears the current user context.
    /// </summary>
    void Clear();
}
