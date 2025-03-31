using Applications.Extensions.Identity;
using Applications.Interfaces.Branches;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace TrapchumApp.Components.Layout
{
    public partial class MainLayout
    {
        [CascadingParameter]
        Task<AuthenticationState> AuthenticateState { get; set; } = default!;

        bool sidebarExpanded = true;
        string? userId = string.Empty;
        int brandId = 0;
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticateState;

            if (auth.User.Identity!.IsAuthenticated)
            {
                userId = auth.User.GetId();

                await getBranchId(userId!);
            }
            await base.OnInitializedAsync();
        }


        async Task getBranchId(string userId)
        {
            var branchResult = await _branchService.GetBranchesByUser(userId,0);

            if (branchResult.IsSuccess)
            {
                var branch = branchResult.Data?.FirstOrDefault();

                if (branch is not null)
                {
                    brandId = branch.BranchId;
                }
            }

        }
    }
}