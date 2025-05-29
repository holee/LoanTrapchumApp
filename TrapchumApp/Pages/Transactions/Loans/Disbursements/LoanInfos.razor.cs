using Applications.Extensions;
using Applications.Responses.ViewModels.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using TrapchumApp.Pages.Transactions.Loans.Repayments;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements
{
    public partial class LoanInfos
    {
        [Parameter] public VLoanDisbursments LoanSchedule { get; set; } = new();
        [Parameter] public string? LoanId { get; set; }=string.Empty;
        [Parameter] public int BranchId { get; set; } = 1;
        [Parameter] public int? Currency { get; set; } = 1; 
        private async Task DownLoadScheduleAsExcel()
        {
            if (LoanId.IsNotNullOrEmpty() && BranchId > 0)
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


        private void ShowPaidSchedule()
        {
            var parametets = new Dictionary<string, object?>
            {
                {  "BranchId", BranchId },
                {  "LoanId", LoanId }
            };

            var options = new SideDialogOptions
            {
                Width="62vw",
                ShowMask=true,
                CloseDialogOnOverlayClick=true,
                Title="Paid Schedule"
            };

            DialogService.OpenSide<SchedulesPaid>("Paid Schedule",parametets,options);
        }
    }
}