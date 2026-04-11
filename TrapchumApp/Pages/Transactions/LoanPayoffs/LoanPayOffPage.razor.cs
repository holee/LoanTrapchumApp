using Applications.Enumerations;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Transactions.RecievePayements;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.LoanPayoffs;

public partial class LoanPayOffPage
{
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [CascadingParameter] public CascadingAppState BState { get; set; } = default!;
    [Parameter] public int BranchId { get; set; }
    [Parameter] public string? LoanId { get; set; }
    [Parameter, SupplyParameterFromQuery] public int CurrencyId { get; set; }

    private VLoanDisbursments? loanSchedule = new();

    private IList<VSchedule> SelectedSchedules = [];

    private IEnumerable<VSchedule> Schedules = [];

    private IEnumerable<LoanResponse> _loanSchedules = [];
    protected override async Task OnInitializedAsync()
    {
        await GetLoanSchedules();

        await base.OnInitializedAsync();
    }


    async Task GetLoanSchedules()
    {
        Tracker.Show();

        var query = new GetLoanByIdQuery(LoanId, BranchId, ScheduleStatus.Unpaid, true);

        var rsp = await mediator.Send(query);

        if (rsp.IsSuccess())
        {
            loanSchedule = rsp.Value;

            Schedules = rsp.Value.Schedules;

        }
        else
        {
            loanSchedule = new();
            Schedules = [];
            NotificationService.Notify(NotificationSeverity.Info, rsp.GetMessage());
        }

        Tracker.Hide();

        StateHasChanged();
    }


    void CellRender(DataGridCellRenderEventArgs<VSchedule> args)
    {
        if (args is null) return;

        if (args.Data is null) return;

        if (args.Data.IsPaid)
        {
            args.Attributes.Add("style", "background-color:var(--rz-danger-lighter)");
        }

    }

    async Task OnRowDoubleClick(DataGridRowMouseEventArgs<VSchedule> arg)
    {
        if (arg is null) return;

        if (arg.Data is null) return;

        //await PayBySchedules(arg.Data);
    }

    async Task HandlePayOff()
    {

        if (loanSchedule is null) return;
        if (!Schedules.Any()) return;
        var schedule = Schedules.Where(s => !s.IsPaid).First();

        var param = new Dictionary<string, object?>
                {
                    {"LoanId",LoanId },
                    {"CurrencyId",CurrencyId },
                    {"BranchId",BranchId },
                    {"CustomerName", loanSchedule?.CustomerName },
                    {"Schedule", schedule }
                };

        var options = new SideDialogOptions
        {
            Width = "40vw",
            ShowClose = false,
            Position = DialogPosition.Right,
            ShowMask = true,
            CloseDialogOnOverlayClick = false
        };


        var dia = await DialogService.OpenSideAsync<PaymentOffSideBar>($"បង់ប្រាក់ផ្តាច់/Payment Off from {loanSchedule?.AltName}", param!, options);

        if (dia is bool res && res)
        {
            await GetLoanSchedules();
        }

    }


}
