using Applications.Extensions.Identity.Authorizations.Extensions;
using System.Security.Claims;
using TrapchumApp.Shared;
using Applications.Extensions.Identity.Policies;
using Domains.Models.Identity;

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

            IsAdminOnly = await _authorizeService.MustHasPermissionAsync(_user, Policies.AdminOnly);

            IsAccountOnly = await _authorizeService.MustHasPermissionAsync(_user, Policies.AccountOnly);

            IsCreditAgentOnly = await _authorizeService.MustHasPermissionAsync(_user, Policies.CreditOfficerOnly);

            IsManagerOnly = await _authorizeService.MustHasPermissionAsync(_user, Policies.ManagerOnly);


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