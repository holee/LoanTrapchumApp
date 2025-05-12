using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Applications.ApplicationPolicies;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Microsoft.AspNetCore.Authorization;

namespace TrapchumApp.Components.Shared
{
    public partial class CascadingAppState : ComponentBase, IAppStateService
    {
        [Parameter] public RenderFragment ChildContent { get; set; } = default!;

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

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                StateHasChanged();
            } 
        }

        public bool IsAccount
        {
            get => _isAccount;
            
            set
            {
                _isAccount = value;
                
                StateHasChanged();
            } 
        }

        public bool IsCreditAgent
        {
            get => _isCreditAgent;
            set
            {
                 _isCreditAgent = value;
                 
                 StateHasChanged();
            }
        }

        public bool CanDisbursed
        {
            get => _canDisbursed;
            set
            {
                _canDisbursed = value;
                StateHasChanged();
            }
        }

        public bool CanApproved
        {
            get => _canApproved;
            set
            {
                _canApproved = value;
                StateHasChanged();
            }
        }

        public bool CanRequest
        {
            get => _canRequest;
            set
            {
                _canRequest = value;
                
                StateHasChanged();
            }
        }


        public async Task CheckedIsAdmin(ClaimsPrincipal user)
        {
            _isAdmin = await _authorizeService.MustHasPermissionAsync(user, RolePolicies.AdminOnly);
        }
        
        public async Task CheckedIsAccount(ClaimsPrincipal user)
        {
            _isAccount = await _authorizeService.MustHasPermissionAsync(user, RolePolicies.AccountOnly);
        }
        
        public async Task CheckedIsCreditAgent(ClaimsPrincipal user)
        {
            _isCreditAgent = await _authorizeService.MustHasPermissionAsync(user, RolePolicies.CreditOfficerOnly);
        }
        
        public async Task CheckedCanDisbursemnt(ClaimsPrincipal user) 
        {
            _canDisbursed = await _authorizeService.MustHasPermissionAsync(user, RequestsPolicies.Disburse);
        }
        
        public async Task CheckedCanApprved(ClaimsPrincipal user)  
        {
            _canApproved = await _authorizeService.MustHasPermissionAsync(user, RequestsPolicies.Approve);
        }
        
        public async Task CheckedCanRequest(ClaimsPrincipal user)   
        {
            _canRequest = await _authorizeService.MustHasPermissionAsync(user, RequestsPolicies.Request);
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