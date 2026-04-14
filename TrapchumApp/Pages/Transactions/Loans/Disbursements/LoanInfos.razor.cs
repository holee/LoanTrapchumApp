using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Loans.Queries.GetLoanById;
using Microsoft.JSInterop;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Transactions.Loans.Repayments;
using TrapchumApp.Services;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements
{
    public partial class LoanInfos(ExcelExportService excelExportService)
    {
        [Parameter] public VLoanDisbursments? LoanSchedule { get; set; } = new();
        [Parameter] public string? LoanId { get; set; } = string.Empty;
        [Parameter] public int BranchId { get; set; } = 1;
        [Parameter] public int? Currency { get; set; } = 1;

        private bool showLoader = false;

        private async Task DownLoadScheduleAsExcel()
        {
            Tracker.Show();

            if (string.IsNullOrEmpty(LoanId) || BranchId <= 0)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Warning",
                    Detail = "LoanId and BranchId are required to download the schedule as Excel.",
                    Duration = 3000
                });

                Tracker.Hide();

                return;
            }

            var fileName = $"Loan_Schedule_{LoanId}.xlsx";

            var stream = excelExportService.ScheduleExportAll(LoanSchedule, LoanSchedule?.Schedules);

            var base64 = Convert.ToBase64String(stream!.ToArray());

            if (string.IsNullOrEmpty(base64))
            {

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "Failed to generate the Excel file.",
                    Duration = 3000
                });

                Tracker.Hide();

                return;
            }

            await js.InvokeVoidAsync("downloadFile", fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", base64);
           
            await Task.Delay(1000);
            
            Tracker.Hide();
        }

        protected override async Task OnInitializedAsync()
        {
            if (LoanId.IsNotNullOrEmpty() && BranchId > 0 && LoanSchedule!.LoanId.IsNullOrEmpty())
            {
                await GetLoanWithSchedules(BranchId, LoanId);
            }
            await base.OnInitializedAsync();
        }



        async Task GetLoanWithSchedules(int? branchId, string? loanId)
        {

            var query = new GetLoanByIdQuery(loanId, BranchId, ScheduleStatus.All, false);

            var rsp = await mediator.Send(query);

            if (!rsp.IsSuccess())
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = rsp.GetMessage(),
                    Duration = 3000
                });
            }
            else
            {
                LoanSchedule = rsp.Value;
            }
        }


        private async Task DownLoadScheduleAsPdf()
        {
            if (string.IsNullOrEmpty(LoanId) || BranchId <= 0)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Warning",
                    Detail = "LoanId and BranchId are required to download the schedule as PDF.",
                    Duration = 3000
                });
                return;
            }

            var fileName = $"Loan_Schedule_{Guid.CreateVersion7()}.pdf";

            await js.InvokeVoidAsync("open", $"/Reports/Schedules/{LoanId}/{BranchId}/{fileName}", "_blank");


        }


        private async Task Priner()
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
                Width = "62vw",
                ShowMask = true,
                CloseDialogOnOverlayClick = true,
                Title = "Paid Schedule"
            };

            DialogService.OpenSide<SchedulesPaid>("Paid Schedule", parametets, options);
        }
    }
}