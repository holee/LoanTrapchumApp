using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace TrapchumApp.Components.Shared
{
    public partial class CascadingBranchState : ComponentBase, IAppStateService
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        private bool _state;
        private bool _isAdmin;
        private bool _isAccount;
        private bool _isCreditAgent;
        private bool _canDisbursed;
        private bool _canApproved;
        private bool _canRequest;

        public bool State
        {
            get => _state;

            set
            {
                _state = value;

                StateHasChanged();
            }
        }

        public bool IsAdmin { get => _isAdmin; set => _isAdmin = value; }
        public bool IsAccount { get => _isAccount; set => _isAccount = value; }
        public bool IsCreditAgent { get => _isCreditAgent; set => _isCreditAgent = value; }
        public bool CanDisbursed { get => _canDisbursed; set => _canDisbursed = value; }
        public bool CanApproved { get => _canApproved; set => _canApproved = value; }
        public bool CanRequest { get => _canRequest; set => _canRequest = value; }

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
