using Applications.ApplicationPolicies;
using Applications.Extensions.Identity;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TrapchumApp.Components.Shared;

namespace TrapchumApp.Components.Layout
{
    public partial class MainLayout
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticateState { get; set; } = default!;
        [CascadingParameter] CascadingAppState bState { get; set; } = default!;

        bool sidebarExpanded = true;

        string? userId = string.Empty;

        int branchId;

        bool IsAdminOnly = false;

        bool IsAccountOnly = false;

        bool IsCreditAgentOnly = false;

        bool IsManagerOnly = false;

        ClaimsPrincipal? _user;
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticateState;

            _user = auth.User;

            IsAdminOnly = await _authorizeService.MustHasPermissionAsync(_user, RolePolicies.AdminOnly);

            IsAccountOnly = await _authorizeService.MustHasPermissionAsync(_user, RolePolicies.AccountOnly);

            IsCreditAgentOnly = await _authorizeService.MustHasPermissionAsync(_user, RolePolicies.CreditOfficerOnly);

            IsManagerOnly = await _authorizeService.MustHasPermissionAsync(_user, RolePolicies.ManagerOnly);


            if (_user.Identity!.IsAuthenticated)
            {
                userId = _user.GetId();

                await GetBranchId(userId!);

                await bState.GetAuthenticatedUser(_user);
            }
            await base.OnInitializedAsync();
        }


        async Task GetBranchId(string userId)
        {
            var rsp = await _branchService.GetBranchesByUser(userId);

            if (rsp.IsSuccess)
            {
                branchId = rsp.Data!.BranchId;
            }

            if (branchId > 0)
            {
                await bState.GetBranchState(branchId);
            }
        }
    }
}