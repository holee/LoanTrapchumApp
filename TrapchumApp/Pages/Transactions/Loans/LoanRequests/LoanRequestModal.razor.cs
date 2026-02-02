using Applications.Extensions.mappers;
using Applications.Features.Branches.Dtos;
using Applications.Features.Customers.Response;
using Applications.Features.LoanRequests.Command.ApplyLoanRequest;
using Applications.Features.LoanRequests.Requests;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Applications.Responses.ViewModels.Branches;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Loans.LoanRequests
{
    public partial class LoanRequestModal
    {

        [Parameter] public string? CustomerId { get; set; }
        [Parameter] public string? RequestUserId { get; set; }
        [Parameter] public int BranchId { get; set; }
        [Parameter] public decimal? Amount { get; set; } = 0;
        [Parameter] public string? LoanRequestId { get; set; } = string.Empty;
        [Parameter] public string? AprovedUserId { get; set; } = string.Empty;
        IEnumerable<VUser> RequestUsers = [];
        IEnumerable<VCustomerDetails> Customers = [];
        IEnumerable<BranchDto> Branches = [];
        IEnumerable<VUser> AcceptUsers = [];
        string memo = "";
        string? UserId;

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
            var empRsp = await _userService.GetUsersAsync(RequestUserId, string.Empty);

            if (empRsp.IsSuccess)
            {
                RequestUsers = empRsp.Data!;
            }
            var custResp = await _customerService.GetCustomersAsync(CustomerId, 1);

            if (custResp.IsSuccess)
            {
                Customers = custResp.Data!;
            }
            var branchResp = await _branchService.GetBranchesAsync<VBranchResponse>(BranchId, new QueryParam(0, 25, string.Empty));

            if (branchResp.IsSuccess)
            {
                Branches = branchResp.Data!.Maps<BranchDto>();
            }

            var userResp = await _userService.GetUsersAsync(UserId, string.Empty);

            if (userResp.IsSuccess)
            {
                AcceptUsers = userResp.Data!;
            }

            StateHasChanged();
        }

        async void LoanRequestVerified()
        {
            var resp = await mediator.Send(new AppliedLoanRequestCommand(new RequestDtoBaseCreate(BranchId, CustomerId, LoanRequestId, memo)));
            if (!resp.IsError)
            {
                NotificationService.Notify(NotificationSeverity.Success, resp.Value.Message);
                DialogService.Close(true);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resp.FirstError.Description);
            }
        }





    }
}