using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using DocumentFormat.OpenXml.Spreadsheet;
using Domains.Models.Identity;
using System.Security.Claims;
using System.Text.Json;

namespace TrapchumApp.Shared
{
    public partial class CascadingAppState : ComponentBase, IAppStateService
    {
        public event Action? OnChange;
        [Parameter] public RenderFragment ChildContent { get; set; } = default!;
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        private bool _state;
        private bool _isAdmin;
        private bool _isAccount;
        private bool _isCeo;
        private bool _isManager;
        private bool _isCreditAgent;
        private bool _canDisbursed;
        private bool _canApproved;
        private bool _canRequest;
        private bool _vaultIn;
        private int _branchId;
        private string? _userId;
        private string? _employeeId;
        private bool _canRecievePayment;
        private bool _canPayOff;
        private bool _canRefund;
        private bool _canOpenBranch;
        private int _unreadCount;
        public bool CanPayOff
        {
            get { return _canPayOff; }

            set
            {
                _canPayOff = value;

                StateHasChanged();
            }
        }


        public bool CanRecievePayment
        {
            get { return _canRecievePayment; }

            set
            {
                _canRecievePayment = value;

                StateHasChanged();
            }
        }

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
        private bool _canWriteOff;
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

        public bool CanOpenBranch
        {
            get => _canOpenBranch;

            set
            {
                _canOpenBranch = value;

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


        public bool IsCEO
        {
            get => _isCeo;

            set
            {
                _isCeo = value;

                StateHasChanged();
            }
        }

        public int BranchId { get => _branchId; set => _branchId = value; }
        public string? UserId { get => _userId; set => _userId = value; }
        public bool CanWriteOff
        {
            get => _canWriteOff;
            set
            {
                _canWriteOff = value;

                StateHasChanged();
            }
        }

        internal async Task CheckedIsAdmin(ClaimsPrincipal user)
        {
            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _isAdmin = false;

                return;
            }

            _isAdmin = await _authorizeService.MustHasPermissionAsync(user!, Policies.AdminOnly);
        }


        internal async Task CheckedCanPayOff(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == false)
            {
                _canPayOff = await _authorizeService.MustHasPermissionAsync(user, Policies.PayOff);
            }
            else
            {
                _canPayOff = false;
            }

        }


        internal async Task CheckedCanWriteOff(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == false)
            {
                CanWriteOff = await _authorizeService.MustHasPermissionAsync(user, Policies.WriteOff);
            }
            else
            {
                CanWriteOff = false;
            }

        }

        internal async Task CheckedIsManager(ClaimsPrincipal user)
        {
            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _isManager = false;
                return;
            }
            _isManager = await _authorizeService.MustHasPermissionAsync(user!, Policies.ManagerOnly);
        }

        internal async Task CheckedIsCEO(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _isCeo = await _authorizeService.MustHasPermissionAsync(user!, Policies.CEOOnly);

                return;
            }

            _isManager = false;

        }


        internal async Task CheckRecievePayment(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canRecievePayment = await _authorizeService.MustHasPermissionAsync(user, Policies.RecievePayment);
            }
            else
            {
                _canRecievePayment = false;
            }
        }


        internal async Task CheckedIsAccount(ClaimsPrincipal user)
        {
            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _isAccount = false;
                return;
            }

            _isAccount = await _authorizeService.MustHasPermissionAsync(user!, Policies.AccountOnly);
        }

        internal async Task CheckedIsCreditAgent(ClaimsPrincipal user)
        {
            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _isCreditAgent = false;
                return;
            }

            _isCreditAgent = await _authorizeService.MustHasPermissionAsync(user!, Policies.CreditOfficerOnly);
        }

        internal async Task CheckedCanDisbursemnt(ClaimsPrincipal user)
        {
            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _canDisbursed = false;

                return;
            }

            _canDisbursed = await _authorizeService.MustHasPermissionAsync(user!, Policies.Disburse);
        }





        internal async Task CheckedCanVaultIn(ClaimsPrincipal user)
        {
            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _vaultIn = false;

                return;
            }

            _vaultIn = await _authorizeService.MustHasPermissionAsync(user!, Policies.TranxVaultIn);
        }

        internal async Task CheckedCanVaultOut(ClaimsPrincipal user)
        {
            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _vaultOut = false;

                return;
            }

            _vaultOut = await _authorizeService.MustHasPermissionAsync(user!, Policies.TranxVaultOut);
        }


        internal async Task CheckedCanApproved(ClaimsPrincipal? user)
        {

            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _canApproved = false;
                return;
            }

            _canApproved = await _authorizeService.MustHasPermissionAsync(user!, Policies.ApproveLoanRequest);

        }


        internal async Task CheckedCanOpenBranch(ClaimsPrincipal user)
        {

            //canOpenBranch = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.OpenBranch);

            //canCloseBranch = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.CloseBranch);

            //canVaultOut = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.TranxVaultOut);

            //canVaultIn = await _authorizeService.MustHasPermissionAsync(auth.User, Policies.TranxVaultIn);

            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _canOpenBranch = false;
            }
            else
            {
                _canOpenBranch = await _authorizeService.MustHasPermissionAsync(user!, Policies.OpenBranch);
            }
        }

        internal async Task CheckedCanRequest(ClaimsPrincipal user)
        {
            if (user is null || (user is not null && user?.Identity?.IsAuthenticated == false))
            {
                _canRequest = false;

                return;
            }

            _canRequest = await _authorizeService.MustHasPermissionAsync(user!, Policies.RequestLoanRequest);
        }


        public async Task GetAuthenticatedUser(ClaimsPrincipal user)
        {
            await CheckedCanApproved(user);
            await CheckedCanDisbursemnt(user);
            await CheckedCanRequest(user);
            await CheckedIsAccount(user);
            await CheckedIsAdmin(user);
            await CheckedIsCreditAgent(user);
            await CheckedIsManager(user);
            await CheckedIsCreditAgent(user);
            await CheckedCanVaultIn(user);
            await CheckedCanVaultOut(user);
            await CheckedIsCEO(user);
            await CheckRecievePayment(user);
            await CheckedCanWriteOff(user);
            await CheckedCanOpenBranch(user);
        }

        internal async Task GetUserId()
        {
            var authState = await AuthenticationState;

            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                _userId = user.GetId();
            }
            else
            {
                _userId = string.Empty;
            }
        }


        internal async Task<string?> GetEmployeeId()
        {
            var authState = await AuthenticationState;

            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {

                var rsp = await _userService.GetUserByIdAsync(user.GetId());

                _employeeId = rsp.Data?.EmployeeRefId;
            }
            else
            {
                _employeeId = string.Empty;
            }

            return _employeeId;
        }

        internal async Task GetBranchIdByUser()
        {
            var authState = await AuthenticationState;

            var user = authState.User;

            if (user is not null && user.Identity?.IsAuthenticated == true)
            {
                _userId = user.GetId();
            }
            else
            {
                _branchId = 0;
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public IAppStateService CopyState()
        {
            var state = (IAppStateService)this;

            var json = JsonSerializer.Serialize(state);

            var copy = JsonSerializer.Deserialize<AppStateService>(json);

            return copy!;
        }
    }
}