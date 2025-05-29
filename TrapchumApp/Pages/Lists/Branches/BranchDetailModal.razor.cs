using Applications.Responses.ViewModels.Branches;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Lists.Branches
{
    public partial class BranchDetailModal
    {

        [Parameter] public int Id { get; set; }

        VBranchResponse? branch = new();

        protected override async Task OnInitializedAsync()
        {
            var result = await _branchService.GetBranchById<VBranchResponse>(Id);

            if (result.IsSuccess)
            {
                branch = result.Data;
            }

            await base.OnInitializedAsync();
        }
    }
}