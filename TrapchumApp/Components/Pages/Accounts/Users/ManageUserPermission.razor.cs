using Applications.Responses.ViewModels.Auths;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace TrapchumApp.Components.Pages.Accounts.Users
{
    public partial class ManageUserPermission
    {
        [Parameter] public long Id { get; set; }

        protected UserClaimsVm? UserClaimsVM = new UserClaimsVm();

        protected EditForm formEdit = default!;
        protected bool IsLoading = true;
        protected override async Task OnInitializedAsync()
        {
            //var token=await localStorageService.GetItemAsStringAsync("bearerToken");
            //UserClaimsVM=await userService.GetClaimsByUserAsync(Id,token!);
            IsLoading = false;
            await base.OnInitializedAsync();
        }


    }
}
