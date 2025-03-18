using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Authorizations.Attributes
{
    public class MustHasPermissionAttribute : AuthorizeAttribute
    {
        public MustHasPermissionAttribute(Permission permission) => Policy = permission.ToString();
        public MustHasPermissionAttribute(string permission) => Policy = permission;
        public MustHasPermissionAttribute(string module, string feature) => Policy = AppPermission.RuleFor(module, feature);
    
    }
}
