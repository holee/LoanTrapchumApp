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

    IList<VSchedule> SelectedSchedules = [];

    IEnumerable<VSchedule> Schedules = [];

    IEnumerable<LoanResponse> _loanSchedules = [];

    string receiptNo = "";

    bool isVible = false;
    async Task SelectedTabChanged(int index)
    {
        _selectedTabIndex = index;

        if (index == 1)
        {
            await GetSplitSchedules();
        }

    }

    void CellRender(DataGridCellRenderEventArgs<VSchedule> args)
    {
        if (args.Data.IsPaid)
        {
            args.Attributes.Add("style", "background-color:var(--rz-danger-lighter)");
        }

    }

    async Task OnRowDoubleClick(DataGridRowMouseEventArgs<VSchedule> arg)
    {
        await PayBySchedules(arg.Data);
    }


    void ShowPaidSchedules()
    {
        ContextMenuService.Close();

        _selectedTabIndex = 1;
    }


    private bool _isPaymentTab = false;

    private int _selectedTabIndex = 0;

    private LoanPaymentDtoCreate? _loanPayment = new();
    void CloseTab()
    {
        _isPaymentTab = false;

        _selectedTabIndex = 0;
    }

    async Task PayBySchedules(VSchedule p)
    {
        ContextMenuService.Close();

        var schedulesList = loanSchedule?.Schedules?.ToList();

        var firstSchedule = schedulesList?.First(x => x.IsPaid == false);

        if (p != firstSchedule)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Cannot add new schedule due to previouse schedule not complete paid yet.");
            return;
        }

        if (p.IsPaid)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "This schedule is already paid.");

            return;
        }

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

        var options = new DialogOptions
        {
            Resizable = false,

            Draggable = true,

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

            //_isPaymentTab = true;

            //_selectedTabIndex = 2;
        }

        {
            var parameters = new Dictionary<string, object?>();

            parameters.Add("CurrencyId", CurrencyId);

            parameters.Add("LoanPaymentCreate", loanPayment);

            var message = $"Recieve Payment from {loanSchedule!.AltName}";

            bool? confirm = await DialogService.OpenAsync<PaymentModal>(message, parameters, options);

            if (confirm is not null && confirm == true)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Receive successfully.");
            }

        }

    }
    void GetPaymentSchedules()
    {
        ContextMenuService.Close();

        NavigationManager.NavigateTo($"/App/Loans/{BranchId}/Branches/{LoanId}/Schedules?CreditAgentId={_creditAgentId}&CurrencyId={CurrencyId}&UserId={_userId}");

        return;
    }


    async Task GetSplitSchedules()
    {

        var query = new GetLoanWithScedulesQuery(BranchId, LoanId);

        var rsp = await mediator.Send(query);

        if (rsp.IsSuccess())
        {
            _loanSchedules = rsp.Value ?? [];
        }

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
        await DialogService.OpenSideAsync<ScheduleDetails>($"Schedule-{p.LoanScheduleId}", parameters: param, options);


    }




    protected override async Task OnInitializedAsync()
    {

        _userId = (await AuthenticationState).GetUserId();

        await GetLoanSchedulesById(ScheduleStatus.Unpaid);

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
