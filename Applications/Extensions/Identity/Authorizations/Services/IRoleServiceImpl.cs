namespace Applications.Extensions.Identity.Authorizations.Services
{
    public interface IRoleServiceImpl
    {
        HashSet<string> GetRolesAsync();  
    }
}
