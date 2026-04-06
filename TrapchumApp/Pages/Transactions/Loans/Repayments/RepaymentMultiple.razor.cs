using Applications.Calculations;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.Transactions.ForiegnExchanges;
using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using Applications.Features.Currencies.Responses;
using Applications.Features.Loans.Responses;
using Applications.Features.Branches.Dtos;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments;

public partial class RepaymentMultiple
{
    [Parameter] public string? LoanId { get; set; }
    [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
    [Parameter, SupplyParameterFromQuery] public long CreditAgentId { get; set; }
    [Parameter, SupplyParameterFromQuery] public int CurrencyId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string UserId { get; set; }
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    List<VSchedule> schedules { get; set; } = [];
    List<LoanRepaymentDto> RepaymentLists = [];
    List<LoanPaymentLineDtoCreate> loanRepaymentLines = [];
    LoanPaymentLineDtoCreate? LineStore = new();
    LoanPaymentDtoCreate LoanRepayment = new();
    IEnumerable<VBranch> Branches = [];
    VBranch _selectedBranchResponse = new();
    VSchedule? selectedRepayment = new();
    List<VSchedule>? selectedRepayments = [];
    DateTime paymentDate = DateTime.Now;
    bool allowSelectedRow = false;
    string recieptNumber = "";
    private bool btnDisabled = false;
    private VLoanDisbursments? LoanSchedule = new();
    private int selectedIndex = 0;
    protected decimal? penaltyRate = 0;

    RadzenNumeric<decimal?> tb;

    TrxForeignExchangeDtoCreate model = new TrxForeignExchangeDtoCreate();
    ForeignExchangeDtoCreate c = new ForeignExchangeDtoCreate();
    private IEnumerable<VCurrenciesList> Currencies = Enumerable.Empty<VCurrenciesList>();
    protected VCurrenciesList? selectedCurrency;
    RadzenDataGrid<VSchedule> grid = default!;
    private RadzenDataGrid<LoanPaymentLineDtoCreate> gridLoanPaymentLines = default!;
    private IEnumerable<VLoanRepaymentLine> VLoanRepaymentLines = [];
    private IList<VSchedule?>? SelectedSchedules = [];
    private bool isLoading = false;
    decimal? totalPayment = 0.00M;
    bool disableIsPaidNow = true;
    string receiptNo = string.Empty;
    int disableIndex = 0;
    int nextIndex = 1;

    private readonly ILoanService _loanService;
    private readonly ILoanPaymentServiceImpl _paymentService;
    public RepaymentMultiple(ILoanService scheduleService, ILoanPaymentServiceImpl paymentService)
    {
        _loanService = scheduleService;
        _paymentService = paymentService;
    }


    void GetLoanDetails()
    {
        NavigationManager.NavigateTo($"/Loans/Details/{LoanId}?BranchId={BranchId}&CreditAgentId={CreditAgentId}&CurrencyId={CurrencyId}&UserId={UserId}");
        return;
    }


    async Task OpenSideDialog(VSchedule SelectedRepayment)
    {
        ContextMenuService.Close();
        var parameters = new Dictionary<string, object?>{
                    {"LoanId",LoanId},
                    {"CurrencyId",CurrencyId},
                    {"IntallmentNo",SelectedRepayment!.InstallmentNo!},
                    {"SelectedRepayment",SelectedRepayment}
                };
        var options = new SideDialogOptions
        {
            CloseDialogOnOverlayClick = true,
            ShowClose = true,
            Position = DialogPosition.Right,
            WrapperCssClass = "rz-p-0 rz-m-0",
            ShowTitle = true,
            Width = "600px"
        };
        await DialogService.OpenSideAsync<LoanPaymentHistory>("Schedule History", parameters, options);
    }
    async void ShowPaymentDialogue(VSchedule payment)
    {
        var options = new DialogOptions
        {
            Width = "720px",
            Height = "360px",
            Draggable = true,
            Resizable = false
        };
        var parameters = new Dictionary<string, object>{
                        { "LoanPayment",payment},
                        { "CurrencyId",payment.CurrencyId!.Value },
                        { "LoanId",payment.LoanId ! },
                        { "UserId",UserId },
                        { "BranchId",BranchId },
                        { "CreditAgentId",payment.CreditAgentId!
                        }};
        var response = await DialogService.OpenAsync<ConfirmPaymentDialogue>("Recieve Payment", parameters, options);

        if ((bool)response == true)
        {

        }
    }


    protected async Task ApplySchedulePayment()
    {

        if (SelectedSchedules!.Count <= 0)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Please select schedule to apply payment.");
            return;
        }
        loanRepaymentLines = SelectedSchedules.Select(x => new LoanPaymentLineDtoCreate
        {
            LoanRefId = LoanId,
            LoanScheduleRefId = x!.LoanScheduleId,
            InstallmentNo = x!.InstallmentNo,
            NumberOfDay = x!.TotalDay,
            DayArrea = x.DayArrea,
            PayDate = x!.PaymentDate,
            PaidPrinciple = x!.Principle,
            PaidInterest = x.Interest,
            PaidCollectionFee = x.CollectionFee,
            PaidPenaltyAmount = x.PenaltyAmount,
            SubTotal = LoanExtensions.SubTotal(x.TotalPay, x.PenaltyAmount, 0, 0)
        }).ToList();

        if (loanRepaymentLines.Count <= 0)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "There is any schedule selected yet!");
            return;
        }

        var options = new DialogOptions
        {
            Width = "700px",
            Height = "500px",
            Resizable = false,
            Draggable = true,
            WrapperCssClass = "rz-p-1 rz-m-0 rz-background-color-base-100"
        };
        var parameters = new Dictionary<string, object?>{
                {"LoanRepaymentLines" , loanRepaymentLines},
                {"CurrencyId" , CurrencyId},
                {"LoanId" , LoanId},
                {"CreditAgentId" , CreditAgentId},
                { "BranchId",BranchId },
                { "ReceiptNo",receiptNo },
                { "CustomerName",""},
                {"UserId",UserId}
            };

        var message = $"Recieve Payment from Name";
        var confirm = await DialogService.OpenAsync<PaymentSideBar>(message, parameters, options);
        if ((bool)confirm == true)
        {
            await GetCustomerPaymentSchedulesByLoanIdAsync();
            loanRepaymentLines.Clear();
            await grid.Reload();
            StateHasChanged();
        }
        else
        {
            Clear();
        }

        await Task.CompletedTask;

    }


    void Clear()
    {
        totalPayment = 0.00M;
        recieptNumber = string.Empty;
    }
    protected async Task GetCustomerPaymentSchedulesByLoanIdAsync(int status = 1)
    {
        try
        {
            isLoading = true;

            //var responseSchedule = await _repaymentService.GetPaymentSchedulesAsync(LoanId, BranchId, status);

            //if (responseSchedule.IsSuccess)
            //{
            //    schedules = responseSchedule.Data!;
            //}
            //var selectedResp = await _loanService.GetLateScheduleLineDetails(LoanId, BranchId, paymentDate);

            //if (selectedResp.IsSuccess)
            //{
            //    var tempSelected = selectedResp.Data!.Select(x => x.LoanScheduleId).ToList();
            //    SelectedSchedules = schedules.Where(x => tempSelected.Contains(x.LoanScheduleId)).ToList()!;
            //    SelectedSchedules.ToList().ForEach(x =>
            //    {
            //        x!.Selected = true;
            //    });
            //}
            //else
            //{
            //    var message = selectedResp.Errors!.First();
            //    NotificationService.Notify(NotificationSeverity.Warning, message);
            //    SelectedSchedules = [];
            //}

            //var responseResult = await _loanService.GetLoanByIdAsync(LoanId, BranchId,ScheduleStatus.OnDate);

            //if (responseResult.IsSuccess)
            //{
            //    LoanSchedule = responseResult.Data!;
            //}
            //else
            //{
            //    LoanSchedule = new();

            //    NotificationService.Notify(NotificationSeverity.Error,"");
            //}


            await Task.CompletedTask;

            isLoading = false;
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            isLoading = false;
        }
        finally
        {
            isLoading = false;
        }
    }
    protected override async Task OnInitializedAsync()
    {
        btnDisabled = true;
        await GetCustomerPaymentSchedulesByLoanIdAsync(1);
        //var branchResponse = await _branchService.GetBranchesAsync(UserId);
        //if (branchResponse.IsSuccess)
        //{
        //    Branches = branchResponse.Data!;
        //    SelectedBranch = Branches.First();
        //}

        //Currencies = await _curService.GetCurrencies(null);

        if (Currencies.Any())
        {
            selectedCurrency = Currencies.FirstOrDefault(x => x.CurrencyId == 1);
        }
        await base.OnInitializedAsync();
    }
    void CheckBoxCheckedChanged(bool arg, VSchedule s)
    {
        selectedIndex = schedules.IndexOf(s);

        if (selectedIndex > SelectedSchedules!.Count)
        {
            NotificationService.Notify(NotificationSeverity.Info, "Privoius schedule not selected yet!.");
            return;
        }

        if (!allowSelectedRow && SelectedSchedules.Any() && !arg && s.Selected == true)
        {
            var selectedScheduleIndex = SelectedSchedules.IndexOf(s);
            if (selectedScheduleIndex == SelectedSchedules.Count - 1)
            {
                s.Selected = arg;
                s.PenaltyAmount = 0.00M;
                grid.SelectRow(s, false);
            }
            else
            {
                s.Selected = true;
                NotificationService.Notify(NotificationSeverity.Info, "Please select last schedule.");

            }
            StateHasChanged();
            return;
        }

        if (!allowSelectedRow && !s.Selected && arg)
        {
            s.Selected = arg;
            grid.SelectRow(s);
            StateHasChanged();
            return;
        }
        if (!allowSelectedRow && s.Selected && !arg)
        {
            s.Selected = arg;
            s.PenaltyAmount = 0.00M;
            grid.SelectRow(s, false);
        }

    }

    void PenaltyTextChanged(decimal? value, VSchedule s)
    {
        if (SelectedSchedules!.Count == 0) return;
        var selectedIndex = SelectedSchedules!.IndexOf(s);
        var preIndex = selectedIndex == 0 ? 0 : selectedIndex - 1;
        if (preIndex == 0)
        {
            if (SelectedSchedules[preIndex]!.PenaltyAmount == 0)
            {
                value = 0.00M;
                for (int i = 0; i < SelectedSchedules.Count; i++)
                {
                    SelectedSchedules[i]!.PenaltyAmount = 0.00M;
                }
                StateHasChanged();
            }
            return;
        }

        if (SelectedSchedules[preIndex]!.PenaltyAmount == 0)
        {
            value = 0.00M;
            for (int i = SelectedSchedules.Count - 1; i > preIndex; i--)
            {
                SelectedSchedules[i]!.PenaltyAmount = 0.00M;
            }
            StateHasChanged();
            return;
        }
    }
    void RowRenderChanged(RowRenderEventArgs<VSchedule> e)
    {
        if (e.Data.IsLate)
        {
            e.Attributes.Add("style", "color:red;font-weight:bold;");
        }

    }




    protected void SelectedRowEvt(VSchedule s)
    {
        //selectedIndex = schedules.IndexOf(s);

        //Console.WriteLine(selectedIndex);

        //Console.WriteLine(SelectedSchedules!.Count);

        //if (selectedIndex > SelectedSchedules!.Count)
        //{
        //    grid.SelectRow(s, false);
        //    return;
        //}
        //if (SelectedSchedules.Any() && s.Selected == true)
        //{
        //    var selectedScheduleIndex = SelectedSchedules.IndexOf(s);
        //    if (selectedScheduleIndex == SelectedSchedules.Count - 1)
        //    {
        //        s.Selected = false;
        //        grid.SelectRow(s, false);
        //    }
        //    else
        //    {
        //        s.Selected = true;
        //        NotificationService.Notify(NotificationSeverity.Info, "Please select last schedule.");
        //    }
        //    return;
        //}

        //if (!s.Selected)
        //{
        //    s.Selected = true;
        //    grid.SelectRow(s);
        //    return;
        //}
        //else
        //{
        //    s.Selected = false;
        //    grid.SelectRow(s, false);
        //}



        //if (selectedIndex > 0 && !IsPreviousRowSelected(selectedIndex))
        //{
        //    NotificationService.Notify(NotificationSeverity.Error, "Previos schedule not pay yet.");
        //    schedules[selectedIndex].Selected = false;
        //    selectedRepayment = new();
        //    Clear();
        //    return;
        //}
        //if (isAllowMultiplePayment && s.Selected || s.IsPaid)
        //{
        //    SetDefaultValue();
        //    selectedRepayment = s;
        //    Clear();
        //    return;
        //}

        //if (s.IsPaid && selectedIndex != -1)
        //{
        //    btnDisabled = false;
        //    disableIsPaidNow = false;
        //    selectedRepayment = s;
        //    isPaidNow = false;
        //    Clear();
        //    return;
        //}

        //if (!isAllowMultiplePayment && (selectedIndex == 0 || IsPreviousRowSelected(selectedIndex)))
        //{
        //    btnDisabled = false;
        //    disableIsPaidNow = false;
        //    schedules[selectedIndex].Selected = true;
        //    selectedRepayment = s;
        //}

        //if (isAllowMultiplePayment && IsPreviousRowSelected(selectedIndex))
        //{
        //    btnDisabled = false;
        //    disableIsPaidNow = false;
        //    selectedRepayment = s;
        //}

    }
    protected void DeSelectedRowEvt(VSchedule s)
    {
        //if (!isAllowMultiplePayment)
        //{
        //    btnDisabled = false;
        //    SelectedSchedules = null;
        //    Clear();
        //    s.Selected = false;
        //}
    }

}
