using Applications.Extensions.Identity;
using Applications.Extensions.mappers;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Loans.LoanRequests;
using Applications.Responses;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Customers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestModal
    {

        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public Guid? RequestUserId { get; set; }
        [Parameter] public int BranchId { get; set; }
        [Parameter] public decimal? Amount { get; set; } = 0;
        [Parameter] public Guid? LoanRequestId { get; set; } = Guid.Empty;
        [Parameter] public Guid? AprovedUserId { get; set; } = Guid.Empty;
        IEnumerable<VUser> RequestUsers = [];
        IEnumerable<VCustomerList> Customers = [];
        IEnumerable<BranchDto> Branches = [];
        IEnumerable<VUser> AcceptUsers = [];
        string memo = "";
        long UserId;

        protected override void OnParametersSet()
        {
            //UserId = _stateProvider.AuthenticationStateUser.GetId();
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetAllData();
            await base.OnInitializedAsync();
        }
        async Task GetAllData()
        {
            var empRsp = await _userService.GetUsersAsync(RequestUserId.ToString(), string.Empty);

            if (empRsp.IsSuccess)
            {
                RequestUsers = empRsp.Data!;
            }
            var custResp = await _customerService.GetCustomersAsync(CustomerId, 1);

            if (custResp.IsSuccess)
            {
                Customers = custResp.Data!;
            }
            var branchResp = await _branchService.GetBranchesAsync(BranchId);

            if (branchResp.IsSuccess)
            {
                Branches = branchResp.Data!.Maps<BranchDto>();
            }

            var userResp = await _userService.GetUsersAsync(UserId.ToString(), string.Empty);

            if (userResp.IsSuccess)
            {
                AcceptUsers = userResp.Data!;
            }

            StateHasChanged();
        }

        async void LoanRequestVerified()
        {
            var resp = await _loanRequestService.AppliedLoanRequestAsync(new RequestDtoBaseCreate(BranchId, CustomerId, LoanRequestId, memo));

            if (resp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, resp.Data);
                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.ErrMessage);
            }
        }





    }
}