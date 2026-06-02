using Applications.Calculations;
using Applications.Calculations.Extensions;
using Applications.Commons.Mappers;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Queries.GetLoanWithScedules;
using Applications.Responses.ViewModels.Loans.Schedules;
using TrapchumApp.Pages.Transactions.Loans.Repayments;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class LoanDetails
{
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [CascadingParameter] public CascadingAppState BState { get; set; } = default!;
    [Parameter] public int BranchId { get; set; }
    [Parameter] public string? LoanId { get; set; }
    [Parameter] public int CurrencyId { get; set; }

    private string? _creditAgentId = string.Empty;

    private string? _userId = string.Empty;

    private VLoanDisbursments? loanSchedule = new();

    private IList<VSchedule> SelectedSchedules = [];

    private IEnumerable<VSchedule> Schedules = [];

    private IEnumerable<LoanResponse> _loanSchedules = [];

    private string receiptNo = "";

    private bool isVible = false;

    async Task SelectedTabChanged(int index)
    {
        _selectedTabIndex = index;

        if (index == 2)
        {
            await GetSplitSchedules();
        }
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

        await PayBySchedules(arg.Data);
    }

    private bool _isPaymentTab = false;

    private int _selectedTabIndex = 0;

    private LoanPaymentDtoCreate? _loanPayment = new();

    private string _getPaymentSchedulesUrl = string.Empty;

    void CloseTab()
    {
        _isPaymentTab = false;

        _selectedTabIndex = 0;
    }

    async Task PayBySchedules(VSchedule p)
    {
        ContextMenuService.Close();

        if (!IsFirstScheduleSelected(p))
            return;

        if (IsAlreadyPaidSchedule(p))
            return;
        decimal? total = p.SubTotal();

        List<LoanPaymentLineDtoCreate> loanRepaymentLines = [];

        PaymentDtoCreate recievePayment = new();

        var line = p?.ToPaymentLine(recievePayment);

        if (line is not null)
        {
            loanRepaymentLines.Add(line);
        }

        if (!loanRepaymentLines.Any())
        {
            _toastService.ShowError("There is any schedule selected yet!");

            return;
        }

        var loanPaymentLine = p!.MapToLoanPaymentLine();

        var create = loanRepaymentLines.ToPaymentCreate(CurrencyId);

        var options = new SideDialogOptions
        {
            Resizable = false,
            Width = "35rem",
            CssClass = "rz-p-1 rz-m-0 rz-background-color-base-200",
            ShowClose = false,
            Position = DialogPosition.Right,
            ShowMask = true,
            CloseDialogOnOverlayClick = true
        };


        var loanPayment = loanSchedule?.Create(_userId, "");

        if (loanPayment is null)
        {
            _toastService.ShowError("There is any problem with payment");

            return;
        }

        loanPayment.LoanPaymentLines.Add(loanPaymentLine);

        if (loanPayment is not null)
        {
            _loanPayment = loanPayment;
        }

        {
            var parameters = new Dictionary<string, object?>();

            parameters.Add("CurrencyId", CurrencyId);

            parameters.Add("LoanPaymentCreate", loanPayment);

            parameters.Add("CustomerName", loanSchedule!.AltName);

            var message = $"Recieve Payment from {loanSchedule!.AltName}";

            bool? confirm = await DialogService.OpenSideAsync<PaymentSideBar>(message, parameters!, options);

            if (confirm is not null && confirm == true)
            {
                _toastService.ShowSuccess("Receive successfully.");
            }

        }

    }

    private bool SetDisabled(VSchedule schedule)
    {
        return (IsAlreadyPaidSchedule(schedule) || !IsFirstScheduleSelected(schedule));
    }

    private bool IsAlreadyPaidSchedule(VSchedule p)
    {
        return p.IsPaid;
    }

    private bool IsFirstScheduleSelected(VSchedule p)
    {
        var schedulesList = loanSchedule?.Schedules?.ToList();

        var firstSchedule = schedulesList?.First(x => x.IsPaid == false);

        return p == firstSchedule;

    }

    void OnCellContextMenu(DataGridCellMouseEventArgs<VSchedule> args)
    {
        if (args is null) return;

        if (args.Data is null) return;

        ContextMenuService.Open(args,
            new List<ContextMenuItem> {
                new ContextMenuItem(){ Text = "Recieve Payment", Value = 1, Icon = "save",Disabled = (SetDisabled(args.Data) || !BState.CanRecievePayment) },
                new ContextMenuItem(){ Text = "Receive By Schedule", Value = 2, Icon = "paid", Disabled = (SetDisabled(args.Data) || !BState.CanRecievePayment) },
                new ContextMenuItem(){ Text = "Show Details", Value = 3, Icon = "info" },
            }, DoShowContextMenuAction(args));
    }

    private Action<MenuItemEventArgs> DoShowContextMenuAction(DataGridCellMouseEventArgs<VSchedule> args)
    {
        if (args.Data is null) return (e) => { };

        return (e) =>
        {

            ContextMenuService.Close();

            if (e.Value is not null && int.TryParse(e.Value!.ToString(), out int value))
            {
                switch (value)
                {
                    case 1:
                        GetPaymentSchedules();
                        break;
                    case 2:
                        _ = PayBySchedules(args.Data!);
                        break;
                    case 3:
                        _ = ShowScheduleDetails(args.Data!);
                        break;
                }
            }
        };
    }

    void GetPaymentSchedules()
    {
        ContextMenuService.Close();

        NavigationManager.NavigateTo($"/App/Loans/{BranchId}/Branches/{LoanId}/Schedules?CreditAgentId={_creditAgentId}&CurrencyId={CurrencyId}&UserId={_userId}");

        return;
    }


    async Task GetSplitSchedules()
    {

        Tracker.Show();

        var query = new GetLoanWithScedulesQuery(BranchId, LoanId);

        var rsp = await mediator.Send(query);

        if (rsp.IsSuccess())
        {
            _loanSchedules = rsp.Value ?? [];
        }

        Tracker.Hide();

        StateHasChanged();
    }



    async Task ShowAllScedules(bool? arg)
    {
        if (arg.HasValue && arg == true)
        {
            await GetLoanSchedulesById(ScheduleStatus.All);
        }
        else
        {
            await GetLoanSchedulesById(ScheduleStatus.Unpaid);
        }
    }

    async Task GetLoanSchedulesById(ScheduleStatus status = ScheduleStatus.All)
    {
        isVible = true;

        var query = new GetLoanByIdQuery(LoanId, BranchId, status, true);

        var rsp = await mediator.Send(query);

        if (rsp.IsSuccess())
        {
            loanSchedule = rsp.Value;

            _creditAgentId = loanSchedule.CreditAgentRefId;

            Schedules = rsp.Value.Schedules ?? [];

        }
        else
        {
            loanSchedule = new();

            NotificationService.Notify(NotificationSeverity.Info, rsp.GetMessage());
        }

        isVible = false;

        StateHasChanged();
    }

    async Task ShowScheduleDetails(VSchedule p)
    {

        ContextMenuService.Close();

        var param = new Dictionary<string, object?>
            {
                {"LoanId",LoanId },
                {"CurrencyId",CurrencyId },
                {"BranchId",BranchId },
                {"ScheduleId",p.LoanScheduleId },
            };
        var options = new SideDialogOptions
        {
            Position = DialogPosition.Right,
            ShowMask = true,
            CloseDialogOnOverlayClick = true,
            Width = "60vw"
        };
        await DialogService.OpenSideAsync<ScheduleDetailsPage>($"Schedule-{p.LoanScheduleId}", parameters: param!, options);


    }

    protected override async Task OnInitializedAsync()
    {

        var auth = await AuthenticationState;

        await BState.GetAuthenticatedUser(auth.User);

        _userId = auth.GetUserId();

        await GetLoanSchedulesById(ScheduleStatus.Unpaid);

        await base.OnInitializedAsync();
    }
}



