using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Loans.Services;
using Applications.Responses.ViewModels.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using TrapchumApp.Pages.Transactions.Loans.Repayments;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements
{
    public partial class LoanInfos
    {
        private readonly ILoanServiceImpl _loanService;

        public LoanInfos(ILoanServiceImpl loanService)
        {
            _loanService = loanService;
        }

        [Parameter] public VLoanDisbursments? LoanSchedule { get; set; } = new();
        [Parameter] public string? LoanId { get; set; }=string.Empty;
        [Parameter] public int BranchId { get; set; } = 1;
        [Parameter] public int? Currency { get; set; } = 1; 
        private async Task DownLoadScheduleAsExcel()
        {
            if (LoanId.IsNotNullOrEmpty() && BranchId > 0)
            {
                await js.InvokeVoidAsync("open", $"/Reports/Schedules/{LoanId}/{BranchId}/Exel", "_blank");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (LoanId.IsNotNullOrEmpty() && BranchId > 0 
                && LoanSchedule!.LoanId.IsNullOrEmpty())
            {
                await GetLoanWithSchedules(BranchId, LoanId);
            }
            await base.OnInitializedAsync();
        }



        async Task GetLoanWithSchedules(int? branchId, string? loanId)
        {

            var rsp = await _loanService.GetLoanByIdAsync(loanId, BranchId, ScheduleStatus.All, false);

            if (rsp.IsSuccess == false)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = rsp.ErrMessage,
                    Duration = 3000
                });
            }
            else
            {
                LoanSchedule = rsp.Data;
            }
        }


        private async Task DownLoadScheduleAsPdf()
        {

            if (LoanId.IsNotNullOrEmpty() && BranchId > 0)
            {
                await js.InvokeVoidAsync("open", $"/Reports/Schedules/{LoanId}/{BranchId}", "_blank");
            }
        }


        private  async Task Priner()
        {
            await js.InvokeVoidAsync("printPdfFromUrl", $"/Reports/Schedules/{LoanId}/{BranchId}");
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