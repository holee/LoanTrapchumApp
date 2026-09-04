namespace Application.Extensions.Identity.Authorizations.Services
{
    public interface IRoleServiceImpl
    {
        HashSet<string> GetRolesAsync();  
    }
}
