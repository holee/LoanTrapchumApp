using Application.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.AuthorizeHandlers
{
    public class SameAproveLevelAuthorizationHandler : AuthorizationHandler<SameAproveLevelRequirement, LoanRequest>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameAproveLevelRequirement requirement, LoanRequest resource)
        {
            if (context.User.Identity!.IsAuthenticated)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }



    }
}