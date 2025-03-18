using System.Security.Claims;
using Applications.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Authorizations.AuthorizeHandlers
{
    //public class AllowedAprovedAuthorizationHandler : AuthorizationHandler<AllowedDisburseAuthorizationRquirement, VLoanRequests>
    //{
    //    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowedDisburseAuthorizationRquirement requirement, VLoanRequests resource)
    //    {
    //        if (context.User.HasClaim(ClaimTypes.NameIdentifier, resource.AprovedUserId)) {
    //            context.Succeed(requirement);
    //        }
    //        return Task.CompletedTask;
    //    }
    //}

}
