using Applications.Interfaces.Branches;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace TrapchumApp.Components.Shared
{
    public partial class CascadingAppState : ComponentBase, IAppStateService
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        private bool _branchState;

        public bool BranchState
        {
            get => _branchState;

            set
            {
                _branchState = value;
                StateHasChanged();
            }
        }

        public async Task GetBranchState(int branchId)
        {
            var branchStateResponse = await _branchService.GetBrachStatus(branchId);

            if (branchStateResponse.IsSuccess)
            {
                _branchState = branchStateResponse.Data!.IsOpen;
            }
            else
            {
                _branchState = false;
            }

        }

        public IAppStateService CopyState()
        {
            var state = (IAuthStateService)this;
            var json = JsonSerializer.Serialize(state);
            var copy = JsonSerializer.Deserialize<AppStateService>(json);
            return copy!;
        }

    }
}
