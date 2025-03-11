namespace Applications.Extensions.Identity.Authorizations.Services
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermission(string roleId);
    }
}
