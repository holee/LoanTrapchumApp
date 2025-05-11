using Applications.Extensions;
using Applications.Responses.ViewModels.Loans;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements
{
    public partial class LoanInfos
    {
        [Parameter] public VLoanDisbursments LoanSchedule { get; set; } = new();
        private async Task DownLoadScheduleAsExcel()
        {
            if (LoanSchedule.LoanId.IsNotNullOrEmpty() && LoanSchedule.BranchId > 0)
            {
                await js.InvokeVoidAsync("open", $"/Reports/Schedules/{LoanSchedule.LoanId}/{LoanSchedule.BranchId}/Exel", "_blank");
            }
            return;
        }

        private async Task DownLoadScheduleAsPdf()
        {

            if (LoanSchedule.LoanId.IsNotNullOrEmpty() && LoanSchedule.BranchId > 0)
            {
                await js.InvokeVoidAsync("open", $"/Reports/Schedules/{LoanSchedule.LoanId}/{LoanSchedule.BranchId}", "_blank");
            }
            return;
        }
    }
}