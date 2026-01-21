using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Responses;
using Microsoft.JSInterop;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Transactions.Loans.Repayments;
using TrapchumApp.Services;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements
{
    public partial class LoanInfos
    {
        private readonly ExcelExportService _excelExportService;

        public LoanInfos(ExcelExportService excelExportService)
        {
            _excelExportService = excelExportService;
        }

        [Parameter] public VLoanDisbursments? LoanSchedule { get; set; } = new();
        [Parameter] public string? LoanId { get; set; } = string.Empty;
        [Parameter] public int BranchId { get; set; } = 1;
        [Parameter] public int? Currency { get; set; } = 1;

        private bool showLoader = false;
        private async Task DownLoadScheduleAsExcel()
        {
            if (LoanId.IsNotNullOrEmpty() && BranchId > 0)
            {
                try
                {
                    showLoader = true;
                    //await js.InvokeVoidAsync("open", $"/Reports/Schedules/{LoanId}/{BranchId}/Exel", "_blank");
                    var fileName = $"Loan_Schedule_{LoanId}.xlsx";
                    var stream = _excelExportService.ScheduleExportAll(
                        LoanSchedule,
                        LoanSchedule?.Schedules
                    );

                    var base64 = Convert.ToBase64String(stream!.ToArray());
                    if (stream != null)
                    {
                        await js.InvokeVoidAsync("downloadFile", fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", base64);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    showLoader = false;
                }
            }
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

            if (LoanId.IsNotNullOrEmpty() && BranchId > 0)
            {

                var fileName = $"Loan_Schedule_{Guid.CreateVersion7()}.pdf";

                await js.InvokeVoidAsync("open", $"/Reports/Schedules/{LoanId}/{BranchId}/{fileName}", "_blank");


                //NavigationManager.NavigateTo($"/Reports/Schedules/{LoanId}/{BranchId}/{fileName}", true);

            }
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