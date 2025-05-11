using Applications.ApplicationPolicies;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace TrapchumApp.Components.Layout
{
    public partial class BranchItems
    {
        [CascadingParameter] Task<AuthenticationState> authenticationState { get; set; } = default!;
        [Parameter] public int? BranchId { get; set; } = 0;
        [Parameter] public string UserId { get; set; } = string.Empty;
        [Parameter] public bool BranchState { get; set; } = false;
        [Parameter] public bool CanOpenBranch { get; set; } = false;

        bool IsAdminOnly = false;

        bool IsAccountOnly = false;
        protected override async Task OnInitializedAsync()
        {
            var user = (await authenticationState).User;

            IsAdminOnly = await _authorizeService.MustHasPermissionAsync(user, RolePolicies.AdminOnly);
           
            IsAccountOnly = await _authorizeService.MustHasPermissionAsync(user, RolePolicies.AccountOnly);


            await base.OnInitializedAsync();
        }


    }
}