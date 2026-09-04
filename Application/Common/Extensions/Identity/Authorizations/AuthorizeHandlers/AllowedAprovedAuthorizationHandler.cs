using System.Security.Claims;
using Application.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Application.Responses.ViewModels.Loans.Requests;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.AuthorizeHandlers
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
