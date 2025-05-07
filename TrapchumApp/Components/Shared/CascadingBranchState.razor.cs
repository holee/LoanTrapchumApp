using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace TrapchumApp.Components.Shared
{
    public partial class CascadingAppState : ComponentBase, IAppStateService
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        private bool _state;

        public bool BranchState
        {
            get => _state;

            set
            {
                _state = value;

                StateHasChanged();
            }
        }

        public async Task GetBranchState(int? branchId)
        {
            var branchStateResponse = await _branchService.GetBrachStatus(branchId);

            if (branchStateResponse.IsSuccess)
            {
                _state = branchStateResponse.Data!.IsOpen;
            }
            else
            {
                _state = false;
            }

        }

        public IAppStateService CopyState()
        {
            var state = (IAppStateService)this;

            var json = JsonSerializer.Serialize(state);

            var copy = JsonSerializer.Deserialize<AppStateService>(json);

            return copy!;
        }

    }
}
