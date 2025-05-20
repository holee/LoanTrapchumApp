using System.Security.Claims;
using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructures.Authorizations;

public sealed class SameUserApprovedAuthorizationHandler :
    AuthorizationHandler<SameUserApprovedRequirement, VLoanRequests>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        SameUserApprovedRequirement requirement,
        VLoanRequests resource)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId) && resource.AprovedUserId == userId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}