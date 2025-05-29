using Applications.Responses.ViewModels.Loans.Payments;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments
{
    public partial class SchedulesPaid
    {

        private readonly ILoanPaymentServiceImpl _loanPaymentService;
        public SchedulesPaid(ILoanPaymentServiceImpl loanPaymentService)
        {
            _loanPaymentService = loanPaymentService;
        }
        [Parameter] public int? CurrencyId { get; set; } = 1;
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? LoanId { get; set; } = string.Empty;
        int counts = 0;
        IEnumerable<VSchedule> _schedules = [];
        ScheduleVm? _schedule = null;
        bool isLoading = false;
        int pageSize = 15;
        long ScheduleId = 0;
        protected override async Task OnInitializedAsync()
        {
            await GetCustomerSchedules();

            await base.OnInitializedAsync();
        }

        void RowSelecting(VSchedule s) { }
        async Task ShowScheduleDetails(VSchedule p)
        {

            ContextMenuService.Close();

            var rsp = await _loanPaymentService.GetScheduleByIdAsync(p.LoanScheduleId, BranchId, LoanId);

            if (rsp is { IsSuccess: false, Data: null }) return;

            _schedule = rsp.Data;

            var param = new Dictionary<string, object?>
            {
                {"LoanId",LoanId },
                {"CurrencyId",CurrencyId },
                {"BranchId",BranchId },
                {"ScheduleId",p.LoanScheduleId },
                {"Schedule",_schedule  }
            };


            var options = new SideDialogOptions
            {
                Position = DialogPosition.Right,
                ShowMask = true,
                CloseDialogOnOverlayClick = true,
                Width = "55vw"
            };

            await DialogService.OpenSideAsync<ScheduleDetails>($"Schedule-{p.LoanScheduleId}", parameters: param, options);


        }
        async Task GetCustomerSchedules()
        {
            try
            {
                isLoading = true;

                var rsp = await _loanPaymentService.GetPaidedScheduleAsync(BranchId, LoanId);

                if (rsp is { IsSuccess: true, Data: not null })
                {
                    _schedules = rsp.Data;

                    counts = rsp.Data.Count();
                }

                isLoading = false;
            }
            catch (Exception ex)
            {
                isLoading = false;

                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            finally
            {
                isLoading = false;
            }
            StateHasChanged();
        }
    }
}