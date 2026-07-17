using Applications.Commons.Authorizations.Handlers;
using Applications.Commons.Authorizations.Rquirements;
using Applications.Extensions.Identity.Authorizations.Extensions;
using Applications.Extensions.Identity.Policies;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Settings.Dto;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Identity;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Authorization;
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
        private bool _canRecieveMultiple;
        private bool _isSameUser;

        private bool _isBranchManager;

        private bool _normalAmountApproved;

        private bool _maximumAmountApproved;

        private bool _manageDisbursement;


        /// <summary>
        /// For BM,CEO to approve and edit loan disbursement
        /// </summary>

        private bool _canApproveEditLoanRequest;

        public bool CanApproveEditLoanRequest
        {
            get { return _canApproveEditLoanRequest; }
            set { _canApproveEditLoanRequest = value; StateHasChanged(); }
        }

        internal async Task CanApproveRequestEditLoan(LoanDisbursment? resource)
        {
            if (resource is null)  
            {
                _canApproveEditLoanRequest = false;

                return;
            }

            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                var result = await _authorizeService.AuthorizeAsync(user, resource, new CanApprovedEditLoanRequestRequirement());

                _canApproveEditLoanRequest = result.Succeeded;
            }


        }

        /// <summary>
        /// For BM,CEO to edit loan disbursement
        /// </summary>
        private bool _isAllowEditLoan = false;

        public bool IsAllowEditLoan
        {
            get { return _isAllowEditLoan; }
            set
            {
                _isAllowEditLoan = value;

                StateHasChanged();
            }
        }

        internal async Task IsAllowEditLoanDisbursment(LoanDisbursment? resource)
        {
            if (resource is null)
            {
                _isAllowEditLoan = false;

                return;
            }

            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                var result = await _authorizeService.AuthorizeAsync(user, resource, new IsAllowEditLoanRequirement());

                _isAllowEditLoan = result.Succeeded;
            }

        }


        /// <summary>
        /// BM,CEO to approve and delete loan disbursement
        /// </summary>
        private bool _allowApproveDelete;

        public bool AllowApproveDelete
        {
            get => _allowApproveDelete;
            set
            {
                _allowApproveDelete = value;

                StateHasChanged();
            }
        }

        internal async Task AllowApproveDeleteLoanDisbursment(LoanDisbursment? resource)
        {
            if (resource is null)
            {
                _allowApproveDelete = false;

                return;
            }

            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                var result = await _authorizeService.AuthorizeAsync(user, resource, new AllowApproveDeleteLoanRequirement());

                _allowApproveDelete = result.Succeeded;
            }

        }


        /// <summary>
        /// For Account, Chief Account, Teller to edit loan disbursement
        /// </summary>
        private bool _canRequestEditLoan;

        public bool CanRequestEditLoan
        {
            get { return _canRequestEditLoan; }
            set
            {
                _canRequestEditLoan = value;

                StateHasChanged();
            }
        }

        internal async Task CanEditLoanDisbursement(VLoanDisbursments? resource)
        {
            if (resource is null)
            {
                _canRequestEditLoan = false;

                return;
            }

            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                var result = await _authorizeService.AuthorizeAsync(user, resource, new CanRequestEditLoanDisbursementRequirement());

                _canRequestEditLoan = result.Succeeded;
            }


        }


        /// <summary>
        /// For Account, Chief Account, Teller,BM to delete loan disbursement
        /// </summary>
        private bool _canRequestDeleteLoan;

        public bool CanRequestDeleteLoan
        {
            get
            {
                return _canRequestDeleteLoan;
            }
            set
            {

                _canRequestDeleteLoan = value;

                StateHasChanged();
            }
        }

        internal async Task CanDeleteLoanDisbursement(VLoanDisbursments? resource)
        {
            if (resource is null)
            {
                _canRequestDeleteLoan = false;

                return;
            }

            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                var result = await _authorizeService.AuthorizeAsync(user, resource, new CanRequestDeleteLoanDisbursementRequirement());

                _canRequestDeleteLoan = result.Succeeded;
            }


        }

        /// <summary>
        /// For Account, Chief Account, Teller to delete loan disbursement
        /// </summary>

        private bool _allowDeleteLoan;

        public bool AllowDeleteLoan
        {
            get { return _allowDeleteLoan; }

            set
            {
                _allowDeleteLoan = value;

                StateHasChanged();
            }

        }

        internal async Task AllowDeleteLoanDisbursment(LoanDisbursment? resource)
        {
            if (resource is null)
            {
                _allowDeleteLoan = false;

                return;
            }

            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                var result = await _authorizeService.AuthorizeAsync(user, resource, new AllowDeleteLoanRequirement());

                _allowDeleteLoan = result.Succeeded;
            }

        }

        private bool _manageRequest;
        public bool ManageRequest
        {
            get
            {
                return _manageRequest;
            }
            set
            {
                _manageRequest = value;

                StateHasChanged();
            }
        }


        public bool ManageDisbursement
        {
            get
            {
                return _manageDisbursement;
            }
            set
            {
                _manageDisbursement = value;

                StateHasChanged();
            }
        }


        public bool NormalAmountApproved
        {
            get => _normalAmountApproved;

            set
            {
                _normalAmountApproved = value;

                StateHasChanged();
            }
        }

        public bool MaximumAmountApproved
        {
            get => _maximumAmountApproved;

            set
            {
                _maximumAmountApproved = value;

                StateHasChanged();
            }
        }


        public bool IsSameUser
        {
            get => _isSameUser;

            set
            {
                _isSameUser = value;

                StateHasChanged();
            }
        }

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

        public bool CanRecieveMultiple
        {
            get => _canRecieveMultiple;
            set
            {
                _canRecieveMultiple = value;

                StateHasChanged();
            }
        }

        public bool IsBranchManager
        {
            get => _isBranchManager;
            set
            {
                _isBranchManager = value;

                StateHasChanged();
            }
        }


        internal async Task CheckedCanMultipleRecieve(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canRecieveMultiple = await _authorizeService.MustHasPermissionAsync(user!, Policies.MultiplePayment);

                return;
            }
        }

        internal async Task CheckedIsBranchManager(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _isBranchManager = await _authorizeService.MustHasPermissionAsync(user, Policies.BMOnly);
            }
        }

        internal async Task CheckedIsAdmin(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _isAdmin = await _authorizeService.MustHasPermissionAsync(user, Policies.AdminOnly);
            }
        }


        internal async Task CheckedCanPayOff(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canPayOff = await _authorizeService.MustHasPermissionAsync(user, Policies.PayOff);
            }
        }


        internal async Task CheckedCanWriteOff(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                CanWriteOff = await _authorizeService.MustHasPermissionAsync(user, Policies.WriteOff);
            }

        }

        internal async Task CheckedIsManager(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _isManager = await _authorizeService.MustHasPermissionAsync(user, Policies.ManagerOnly);

                return;
            }

        }

        internal async Task CheckedIsCEO(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _isCeo = await _authorizeService.MustHasPermissionAsync(user!, Policies.CEOOnly);

                return;
            }
        }


        internal async Task CheckRecievePayment(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canRecievePayment = await _authorizeService.MustHasPermissionAsync(user, Policies.RecievePayment);

                return;
            }
        }


        internal async Task CheckedIsAccount(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _isAccount = await _authorizeService.MustHasPermissionAsync(user, Policies.AccountOnly);

                return;
            }


        }

        internal async Task CheckedIsCreditAgent(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _isCreditAgent = await _authorizeService.MustHasPermissionAsync(user, Policies.CreditOfficerOnly);

                return;
            }


        }

        internal async Task CheckedCanDisbursemnt(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canDisbursed = await _authorizeService.MustHasPermissionAsync(user, Policies.Disburse);

                return;
            }


        }

        internal async Task ManageLoanDisbursement()
        {
            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {

                _manageDisbursement = await _authorizeService.MustHasPermissionAsync(user, Policies.Disburse);

                return;
            }

        }

        internal async Task ManageLoanRequest()
        {
            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {

                var result = await _authorizeService.AuthorizeAsync(user, Policies.RequestLoanRequest);

                _manageRequest = result.Succeeded;

                return;
            }

        }


        internal async Task WhoCanDisbursemnt(VLoanRequests? resource)
        {
            if (resource is null)
            {
                _canDisbursed = false;

                return;
            }

            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                var result = await _authorizeService.AuthorizeAsync(user, resource, new CanDisburseRequestRequirement());

                _canDisbursed = result.Succeeded;
            }


        }









        internal async Task CheckedCanVaultIn(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _vaultIn = await _authorizeService.MustHasPermissionAsync(user, Policies.TranxVaultIn);
                return;
            }


        }

        internal async Task CheckedCanVaultOut(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _vaultOut = await _authorizeService.MustHasPermissionAsync(user!, Policies.TranxVaultOut);

                return;
            }


        }


        internal async Task CheckedCanApproved(ClaimsPrincipal? user)
        {

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canApproved = await _authorizeService.MustHasPermissionAsync(user, Policies.ApproveLoanRequest);

                return;
            }

        }






        internal async Task CheckedCanOpenBranch(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canOpenBranch = await _authorizeService.MustHasPermissionAsync(user!, Policies.OpenBranch);

                return;
            }
        }

        internal async Task CheckedCanRequest(ClaimsPrincipal user)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canRequest = await _authorizeService.MustHasPermissionAsync(user!, Policies.RequestLoanRequest);

                return;
            }


        }


        internal async Task CheckedIsSameUser(ClaimsPrincipal? user, object? requests)
        {
            if (user is not null && user.Identity?.IsAuthenticated == true)
            {
                _isSameUser = (await _authorizeService.AuthorizeAsync(user, requests, Policies.SameUser)).Succeeded;

                return;
            }


        }

        internal async Task CheckedNormalApproved(ClaimsPrincipal user, VLoanRequests? resource)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                var result = await _authorizeService.AuthorizeAsync(user, resource, new IsCanApprovedNormalAmountRequirement());

                _normalAmountApproved = result.Succeeded;
            }
        }

        internal async Task CheckedMaximumApproved(ClaimsPrincipal user, VLoanRequests? resource)
        {
            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                var result = await _authorizeService.AuthorizeAsync(user, resource, new IsCanApprovedMaximumAmountRequirement());

                _maximumAmountApproved = result.Succeeded;

            }
        }


        internal async Task WhoCanApproved((VLoanRequests? request, VUserLoanSizes? loanSizes) resource)
        {
            if (resource.loanSizes is null || resource.request is null)
            {
                _canApproved = false;

                return;
            }

            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canApproved = (await _authorizeService.AuthorizeAsync(user, resource, new CanApprovedRequestRequirement())).Succeeded;
            }

        }

        internal async Task WhoCanRequest(VLoanRequests? resource)
        {
            if (resource is null)
            {
                _canRequest = false;

                return;
            }

            var user = (await AuthenticationState).User;

            if (user is not null && user?.Identity?.IsAuthenticated == true)
            {
                _canRequest = (await _authorizeService.AuthorizeAsync(user, resource, new CanRequestRequirement())).Succeeded;
            }

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
            await CheckedCanMultipleRecieve(user);
            await CheckedCanPayOff(user);
            await CheckedIsBranchManager(user);
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

                _employeeId = rsp.IsError ? null : rsp.Value?.EmployeeRefId;
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