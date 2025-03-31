using Applications.Requests.Loans.LoanRequests;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanRequests
{
    public partial class ConfirmModal
    {
        [Parameter] public string AltName { get; set; } = string.Empty;
        [Parameter] public Guid? CustomerId { get; set; }
        [Parameter] public string BranchName { get; set; } = string.Empty;
        [Parameter] public string RequestUserName { get; set; } = string.Empty;
        [Parameter] public Guid? RequestUserId { get; set; }
        [Parameter] public string AprovedUserName { get; set; } = string.Empty;
        [Parameter] public Guid? AprovedUserId { get; set; }
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public decimal AprovedAmount { get; set; } = 0;
        [Parameter] public decimal AdmindFeeAmount { get; set; } = 0;
        [Parameter] public Guid? LoanRequestId { get; set; }
        string memo = "";
        async void LoanRequestVerified()
        {

            var confirm = await DialogService.Confirm("Are you sure to aprove this loan request", "Loan Request");

            if (confirm == true)
            {

                var model = new AproveLoanDtoCreate(BranchId, CustomerId, LoanRequestId, AprovedAmount, AdmindFeeAmount, DateTime.Now, memo);

                var resp = await _loanRequestService.AprovedLoanRequestAsync(model);

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





        // async Task GetAllData()
        // {
        //     // var empRsp = await _userService.GetUsersAsync(RequestUserId, string.Empty);

        //     // if (empRsp.IsSuccess)
        //     // {
        //     //     RequestUsers = empRsp.Data!;
        //     // }
        //     // var custResp = await _customerService.GetCustomersAsync(Data?.CustomerId, 1);

        //     // if (custResp.IsSuccess)
        //     // {
        //     //     Customers = custResp.Data!;
        //     // }
        //     // var branchResp = await _branchService.GetBranchesAsync(Data!.BranchId, 1);

        //     // if (branchResp.IsSuccess)
        //     // {
        //     //     Branches = branchResp.Data!;
        //     // }

        //     // var userResp = await _userService.GetUsersAsync(loginUser);

        //     // if (userResp.IsSuccess)
        //     // {
        //     //     AcceptUsers = userResp.Data!;
        //     // }

        //     StateHasChanged();
        // }


    }
}