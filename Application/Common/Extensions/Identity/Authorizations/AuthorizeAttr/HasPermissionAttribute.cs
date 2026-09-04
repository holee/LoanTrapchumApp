using Application.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.AuthorizeAttr;

public class HasPermissionAttribute : AuthorizeAttribute
{

    public HasPermissionAttribute(string module, string feature) : base(policy: AppPermission.RuleFor(module, feature))
    {

    }
}
