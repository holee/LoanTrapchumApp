namespace Application.Extensions.Identity.Authorizations.Services
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermission(string roleId);
    }
}
