using Applications.Calculations;
using Applications.Calculations.Extensions;
using Applications.Commons.Mappers;
using Applications.Enumerations;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Queries.GetLoanWithScedules;
using Applications.Features.Loans.Responses;
using Applications.Features.Loans.Services;
using Applications.Responses.ViewModels.Loans.Schedules;
using TrapchumApp.Helpers.Extensions;
using TrapchumApp.Pages.Transactions.Loans.Repayments;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class LoanDetails
{
    private readonly ScheduleService _scheduleService;
    public LoanDetails(ScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
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
            NotificationService.Notify(NotificationSeverity.Error, "There is any schedule selected yet!");

            return;
        }

        var loanPaymentLine = p!.MapToLoanPaymentLine();

        var create = loanRepaymentLines.ToPaymentCreate(CurrencyId);

        var options = new SideDialogOptions
        {
            Resizable = true,
            Width = "35rem",
            CssClass = "rz-p-1 rz-m-0 rz-background-color-base-200"
        };


        var loanPayment = loanSchedule?.Create(_userId, "");

        if (loanPayment is null)
        {
            NotificationService.Notify(NotificationSeverity.Error, "There is any problem with payment");

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

            var message = $"Recieve Payment from {loanSchedule!.AltName}";

            bool? confirm = await DialogService.OpenSideAsync<PaymentModal>(message, parameters!, options);

            if (confirm is not null && confirm == true)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Receive successfully.");
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

        //if (p.IsPaid)
        //{
        //    NotificationService.Notify(NotificationSeverity.Error, "This schedule is already paid.");

        //    return true;
        //}

        //return false;
    }

    private bool IsFirstScheduleSelected(VSchedule p)
    {
        var schedulesList = loanSchedule?.Schedules?.ToList();

        var firstSchedule = schedulesList?.First(x => x.IsPaid == false);

        return p == firstSchedule;

        //{
        //    return false;
        //}

        //return true;

        // NotificationService.Notify(NotificationSeverity.Error, "Cannot add new schedule due to previouse schedule not complete paid yet.");

    }

    void OnCellContextMenu(DataGridCellMouseEventArgs<VSchedule> args)
    {
        if (args is null) return;

        if (args.Data is null) return;

        ContextMenuService.Open(args,
            new List<ContextMenuItem> {
                new ContextMenuItem(){ Text = "Recieve Payment", Value = 1, Icon = "save",Disabled = SetDisabled(args.Data) },
                new ContextMenuItem(){ Text = "Receive By Schedule", Value = 2, Icon = "paid", Disabled = SetDisabled(args.Data) },
                new ContextMenuItem(){ Text = "Show Details", Value = 3, Icon = "info" },
            },
            (e) =>
            {

                ContextMenuService.Close();

                if (e.Value is not null && int.TryParse(e.Value!.ToString(), out int value))
                {

                    //if (!IsFirstScheduleSelected(args.Data))
                    //    return;

                    //if (IsAlreadyPaidSchedule(args.Data))
                    //    return;

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
            }
         );
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


    void ShowPaidSchedules()
    {
        ContextMenuService.Close();

        //_selectedTabIndex = 1;
    }

    async Task ShowScheduleDetails(VSchedule p)
    {
        //await popup.ToggleAsync(buttonItem.Element);


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

        _userId = (await AuthenticationState).GetUserId();

        await GetLoanSchedulesById(ScheduleStatus.All);

        await base.OnInitializedAsync();
    }
}



//var schedules = loanSchedule!.Schedules!.ToList();

//int selectedIndex = schedules.IndexOf(p);

//if (selectedIndex > 0)
//{
//    var preSchedule = schedules[selectedIndex - 1];

//    if (!preSchedule.IsPaid)
//    {
//        NotificationService.Notify(NotificationSeverity.Info, "Previous schedule not pay yet.");

//        return;
//    }
//}
//var options = new DialogOptions { };

//var paymentCreate = p.ToPaymentDtoCreate();

//var line = p.ToPaymentLine(paymentCreate);

//loanRepaymentLines.Add(line);
//PaymentCreate create = new PaymentCreate();
//create.TotalAmountWithPenalty = loanRepaymentLines.Total();
//create.OriginalPaidAmount = loanRepaymentLines.TotalAmountWithPenalty();
//create.TotalAmountNoPenalty = loanRepaymentLines.TotalAmountWithPenalty();
//create.PenaltyAmount = loanRepaymentLines.Sum(x => x.PaidPenaltyAmount);
//create.OriginalPenaltyAmount = loanRepaymentLines.Sum(x => x.PaidPenaltyAmount);
//create.RecieptNumber = "";
//if (CurrencyId == 1)
//{
//    create.RecieveKhrAmount = loanRepaymentLines.Total();
//}
//else
//{
//    create.RecieveAmount = loanRepaymentLines.Total();
//}
//create.ChangeAmount = 0;
//create.ChangeKhrAmount = 0;
//var data = new Dictionary<string, object?>
//{
//        {"LoanRepaymentLines" , loanRepaymentLines},
//        {"PaymentCreate" ,create},
//        {"CurrencyId" , CurrencyId},
//        {"LoanId" , LoanId},
//        {"CreditAgentId" , CreditAgentId},
//        { "BranchId",BranchId },
//        { "ReceiptNo",receiptNo },
//        { "CustomerName",p.AltName},
//        {"UserId",UserId}
//};


//bool confirm = await DialogService.OpenAsync<PaymentModal>("Confirm", data, options);

//if (confirm)
//{
//    await GetLoanSchedulesById(ScheduleStatus.Unpaid);
//}

//var loanPayment = new LoanPaymentDtoCreate
//{
//    LoanRefId = LoanId,

//    CreditAgentRefId = CreditAgentId,

//    BranchRefId = BranchId,

//    CurrencyRefId = CurrencyId,

//    UserRefId = UserId,

//    PayDate = DateTime.Now,

//    CustomerName = loanSchedule!.AltName,

//    OfficialReceiptNo = loanPaymentLine.OfficialReceiptNo
//};
