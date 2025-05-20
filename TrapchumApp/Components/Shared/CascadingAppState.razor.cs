using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Applications.ApplicationPolicies;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;

namespace TrapchumApp.Components.Shared
{
    public partial class CascadingAppState : ComponentBase, IAppStateService
    {
        [Parameter] public RenderFragment ChildContent { get; set; } = default!;

        private bool _state;
        private bool _isAdmin;
        private bool _isAccount;
        private bool _isManager;
        private bool _isCreditAgent;
        private bool _canDisbursed;
        private bool _canApproved;
        private bool _canRequest;

        private bool _vaultIn;

        public bool VaultIn
        {
            get { return _vaultIn; }
            set
            {
                _vaultIn = value;
                StateHasChanged();
            }
        }

        private bool _vaultOut;

        public bool VaultOut
        {
            get { return _vaultOut; }
            set
            {
                _vaultOut = value;
                StateHasChanged();
            }
        }


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

        public bool IsManager
        {
            get => _isManager;
            set
            {
                _isManager = value;

                StateHasChanged();
            }
        }
        internal async Task CheckedIsAdmin(ClaimsPrincipal user)
        {
            if (user is null && user?.Identity?.IsAuthenticated == false)
            {
                _isAdmin = false;

                return;
            }

            _isAdmin = await _authorizeService.MustHasPermissionAsync(user!, RolePolicies.AdminOnly);
        }

        internal async Task CheckedIsManager(ClaimsPrincipal user)
        {
            if (user is null && user?.Identity?.IsAuthenticated == false)
            {
                _isManager = false;
                return;
            }
            _isManager = await _authorizeService.MustHasPermissionAsync(user!, RolePolicies.ManagerOnly);
        }

        internal async Task CheckedIsAccount(ClaimsPrincipal user)
        {
            if (user is null && user?.Identity?.IsAuthenticated == false)
            {
                _isAccount = false;
                return;
            }

            _isAccount = await _authorizeService.MustHasPermissionAsync(user!, RolePolicies.AccountOnly);
        }

        internal async Task CheckedIsCreditAgent(ClaimsPrincipal user)
        {
            if (user is null && user?.Identity?.IsAuthenticated == false)
            {
                _isCreditAgent = false;
                return;
            }

            _isCreditAgent = await _authorizeService.MustHasPermissionAsync(user!, RolePolicies.CreditOfficerOnly);
        }

        internal async Task CheckedCanDisbursemnt(ClaimsPrincipal user)
        {
            if (user is null && user?.Identity?.IsAuthenticated == false)
            {
                _canDisbursed = false;

                return;
            }

            _canDisbursed = await _authorizeService.MustHasPermissionAsync(user!, RequestsPolicies.Disburse);
        }

        internal async Task CheckedCanVaultIn(ClaimsPrincipal user)
        {
            if (user is null && user?.Identity?.IsAuthenticated == false)
            {
                _vaultIn = false;

                return;
            }

            _vaultIn = await _authorizeService.MustHasPermissionAsync(user!, TranxsPolicies.VaultIn);
        }

        internal async Task CheckedCanVaultOut(ClaimsPrincipal user)
        {
            if (user is null && user?.Identity?.IsAuthenticated == false)
            {
                _vaultOut = false;

                return;
            }

            _vaultOut = await _authorizeService.MustHasPermissionAsync(user!, TranxsPolicies.VaultOut);
        }


        internal async Task CheckedCanApprved(ClaimsPrincipal user)
        {

            if (user is null && user?.Identity?.IsAuthenticated == false)
            {
                _canApproved = false;

                return;
            }

            _canApproved = await _authorizeService.MustHasPermissionAsync(user!, RequestsPolicies.Approve);
        }

        internal async Task CheckedCanRequest(ClaimsPrincipal user)
        {
            if (user is null && user?.Identity?.IsAuthenticated == false)
            {
                _canRequest = false;

                return;
            }

            _canRequest = await _authorizeService.MustHasPermissionAsync(user!, RequestsPolicies.Request);
        }


        public async Task GetAuthenticatedUser(ClaimsPrincipal user)
        {
            await CheckedCanApprved(user);
            await CheckedCanDisbursemnt(user);
            await CheckedCanRequest(user);
            await CheckedIsAccount(user);
            await CheckedIsAdmin(user);
            await CheckedIsCreditAgent(user);
            await CheckedIsManager(user);
            await CheckedIsCreditAgent(user);
            await CheckedCanVaultIn(user);
            await CheckedCanVaultOut(user);
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