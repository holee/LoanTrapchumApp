using Domains.Models.Identity;
namespace Applications.Interfaces.Identity;

public interface IIdentityService //: IService
{
    Task<string?> GetUserNameAsync(string userId, CancellationToken cancellation = default);
    Task<bool> IsInRoleAsync(string userId, string role, CancellationToken cancellation = default);
    Task<bool> AuthorizeAsync(string userId, string policyName, CancellationToken cancellation = default);
    Task<IDictionary<string, string?>> FetchUsers(string roleName, CancellationToken cancellation = default);
    Task<ApplicationUser?> GetApplicationUserDto(string userName, CancellationToken cancellation = default);
    string GetUserName(string userId);
    Task<List<ApplicationUser>?> GetUsers(string? tenantId, CancellationToken cancellation = default);
    void RemoveApplicationUserCache(string userName);
}
