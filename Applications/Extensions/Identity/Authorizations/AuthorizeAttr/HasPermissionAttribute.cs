using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Authorizations.AuthorizeAttr;

public class HasPermissionAttribute : AuthorizeAttribute
{

    public HasPermissionAttribute(string module, string feature) : base(policy: AppPermission.RuleFor(module, feature))
    {

    }
}
