using System.Security.Claims;
using Applications.Responses.ViewModels.Loans.Requests;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructures.Authorizations;

public sealed class SameUserRequestAuthorizationHandler :
    AuthorizationHandler<SameUserRequestRequirement, VLoanRequests>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        SameUserRequestRequirement requirement,
        VLoanRequests resource)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId) && resource.UserRequestId == userId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
