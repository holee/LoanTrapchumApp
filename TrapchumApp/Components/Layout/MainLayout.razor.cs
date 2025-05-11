using Applications.ApplicationPolicies;
using Applications.Extensions.Identity;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TrapchumApp.Components.Shared;

namespace TrapchumApp.Components.Layout
{
    public partial class MainLayout
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticateState { get; set; } = default!;
        [CascadingParameter] CascadingBranchState bState { get; set; } = default!;

        bool sidebarExpanded = true;

        string? userId = string.Empty;

        int branchId;

        bool IsAdminOnly = false;

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticateState;

            IsAdminOnly = await _authorizeService.MustHasPermissionAsync(auth.User, RolePolicies.AdminOnly);

            if (auth.User.Identity!.IsAuthenticated)
            {
                userId = auth.User.GetId();

                await GetBranchId(userId!);
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