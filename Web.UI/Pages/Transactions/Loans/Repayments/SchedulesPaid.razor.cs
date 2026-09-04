using Applications.Features.LoanPayments.Queries.GetPaidSchedule;
using Applications.Features.LoanPayments.Queries.GetScheduleById;
using Applications.Features.Schedules.Dto;
using Applications.Responses.ViewModels.Loans.Schedules;

namespace Web.UI.Pages.Transactions.Loans.Repayments
{
    public partial class SchedulesPaid
    {

        [Parameter] public int? CurrencyId { get; set; } = 1;
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? LoanId { get; set; } = string.Empty;

        int counts = 0;

        IEnumerable<VSchedule> _schedules = [];

        ScheduleDetailsDto? _schedule = null;

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

            var query=new GetScheduleByIdQuery(p.LoanScheduleId, BranchId, LoanId);

            var rsp = await mediator.Send(query);

            if (rsp.IsError) return;

            _schedule = rsp.Value;

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

            await DialogService.OpenSideAsync<ScheduleDetailsPage>($"Schedule-{p.LoanScheduleId}", parameters: param!, options);


        }
        async Task GetCustomerSchedules()
        {

            Tracker.Show();

            var command = new GetPaidScheduleQuery(BranchId, LoanId);

            var rsp = await mediator.Send(command);

            if (rsp.IsSuccess())
            {
                _schedules = rsp.Value;

                counts = rsp.Value.Count();
            }

            Tracker.Hide();
            
            StateHasChanged();
        }
    }
}