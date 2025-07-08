using Applications.Calculations;
using Applications.Commons.Mappers;
using Applications.Enumerations;
using Applications.Requests.Loans.Payments;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Domains.Models.Loans;
using System.Threading.Tasks;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments;
public partial class PaymentDetails
{
    readonly ILoanServiceImpl _dService;

    public PaymentDetails(ILoanServiceImpl dService)
    {
        _dService = dService;
    }

    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [Parameter] public int BranchId { get; set; }
    [Parameter] public string? LoanId { get; set; }
    [Parameter, SupplyParameterFromQuery] public int CurrencyId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? CreditAgentId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }

    private VLoanDisbursments? loanSchedule = new();

    IList<VSchedule> SelectedSchedules = [];

    IEnumerable<VSchedule> Schedules = [];

    bool collapse = false;
    string collapseIcon = "expand_more";
    bool isVible = false;
    int selectedTabIndex = 0;
    string receiptNo = "";
    bool ShowAll = false;
    int? selectedItem = 0;
    void SelectedTabChanged(int index)
    {
        selectedTabIndex = index;
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

        selectedTabIndex = 1;
    }

    async Task PayBySchedules(VSchedule p)
    {
        ContextMenuService.Close();

        var schedulesList = loanSchedule?.Schedules?.ToList();

        var firstSchedule = schedulesList?.First(x=>x.IsPaid==false);

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


        var loanPayment = loanSchedule?.Create(UserId, "");

        if (loanPayment is null)
        {
            NotificationService.Notify(NotificationSeverity.Error, "There is any problem with payment");

            return;
        }

        loanPayment.LoanPaymentLines.Add(loanPaymentLine);
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

        NavigationManager.NavigateTo($"/App/Loans/{BranchId}/Branches/{LoanId}/Schedules?CreditAgentId={CreditAgentId}&CurrencyId={CurrencyId}&UserId={UserId}");
       
        return;
    }


    async Task OnChanged(bool args)
    {
        if (args)
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

        var rsp = await _dService.GetLoanByIdAsync(LoanId, BranchId, status, true, default);

        if (rsp.IsSuccess)
        {
            loanSchedule = rsp.Data!;

            Schedules = rsp.Data!.Schedules ?? [];

        }
        else
        {
            loanSchedule = new();

            NotificationService.Notify(NotificationSeverity.Info, rsp.ErrMessage);
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
        try
        {
            await GetLoanSchedulesById(ScheduleStatus.Unpaid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException!.Message);
        }
        await base.OnInitializedAsync();
    }

    private void DownLoadScheduleAsExcel(string loanRefId)
    {
        //var stream = await _dService.DownLoadScheduleAsExcel(loanRefId, BranchId);
        //using var streamRef = new DotNetStreamReference(stream: stream);
        //await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.xlsx", streamRef);
    }

    private void DownLoadScheduleAsPdf(string loanRefId)
    {
        //var stream = await _dService.DownLoadScheduleAsPdf(loanRefId, BranchId);

        //using var streamRef = new DotNetStreamReference(stream: stream);

        //await js.InvokeVoidAsync("downloadFileFromStream", "Schedule.pdf", streamRef);
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
