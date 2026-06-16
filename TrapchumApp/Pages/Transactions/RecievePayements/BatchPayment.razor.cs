using Applications.Calculations;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Responses.ViewModels.Loans.Schedules;
using Radzen.Blazor;
using Applications.Features.Schedules.Queries.GetDueScheduleByLoanId;
using TrapchumApp.Pages.Transactions.Loans.Repayments;

namespace TrapchumApp.Pages.Transactions.RecievePayements;

public partial class BatchPayment
{
    [Parameter] public string? LoanId { get; set; }
    [Parameter] public int BranchId { get; set; } = 0;
    [Parameter] public EventCallback<string> OnClose { get; set; }
    [Parameter] public EventCallback ReloadTab { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? CreditAgentId { get; set; }
    [Parameter, SupplyParameterFromQuery] public int CurrencyId { get; set; }
    [Parameter] public string? AltName { get; set; } = "";

    private string? _userId = string.Empty;
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    DateTime paymentDate = DateTime.Now;

    IEnumerable<VSchedule> schedules = [];

    List<LoanPaymentLineDtoCreate> LoanPaymentLines = [];

    LoanPaymentDtoCreate LoanRepayment = new();

    VLoanDisbursments? LoanSchedule = new();

    RadzenDataGrid<VSchedule> gridSchedule = default!;

    PaymentLines paymentLinesRef = default!;

    VLoanDisbursments? vLoan = new();

    bool isAllowMultiplePayment = true;

    PaymentDtoCreate recievePayment = new();

    decimal? principleAmount = 0;

    decimal? interestAmount = 0;

    decimal? collectionfeeAmount = 0;

    decimal? totalPaidAmount = 0;

    decimal? penaltyAmount = 0;

    decimal? totalPaymentAmount = 0.00M;

    decimal? penaltyRate = 0;

    int selectedIndex = 0;

    bool isPaidNow = false;

    bool isLoading = false;

    bool includePaid = false;

    bool? canPaidNow = false;

    bool? isRestoreDefault = false;

    bool disableIsPaidNow = true;

    bool btnDisabled = false;

    string receiptNo = string.Empty;

    string recieptNumber = "";

    int pageIndex = 1;

    int pageSize = 25;

    string sortField = "LoanId";

    string filter = string.Empty;

    int selectIndex = 0;

    int counts = 0;

    private bool _isSelectionLocked = false;

    bool _isPaymentTab = false;

    bool _isMultiplePaymentTab = false;

    int _selectedIndexTab = 0;

    private LoanPaymentLineDtoCreate? _loanPaymentLine = new();

    LoanPaymentDtoCreate _loanPayment = new();

    protected override async Task OnInitializedAsync()
    {
        _userId = (await AuthenticationState).GetUserId();

        await GetSchedules();

        await base.OnInitializedAsync();
    }


    bool _showExchanged = false;

    void ToggleExchangeChanged()
    {
        if (_isPaymentTab && _showExchanged)
        {
            _showExchanged = true;
        }
        else
        {
            _showExchanged = false;
        }
    }


    private async Task CloseOpenTab()
    {
        await OnClose.InvokeAsync(LoanId);
    }


    async Task ApplyMultipleSchedulePayment()
    {

        if (_demoSelected.Count == 0)
        {
            _toastService.ShowError("Please select schedule to apply payment.");

            return;
        }


        var confirm = await MsgBox.ConfirmAsync("Do you want to apply this schedules?", "Receive Payment");

        if (confirm == MsgBoxResult.No) return;

        decimal remainingAmount = _applyAmount ?? 0;

        var batchSchedules = schedules
                               .Where(x => !x.IsPaid)
                               .OrderBy(x => x.InstallmentNo)
                               .Distinct()
                               .ToList();

        foreach (var schedule in batchSchedules)
        {
            if (remainingAmount <= 0)
                break;

            var line = CreatePaymentLine(schedule, ref remainingAmount);

            if (line != null)
                LoanPaymentLines.Add(line);
        }

        var loanPayment = new LoanPaymentDtoCreate
        {
            LoanRefId = LoanId,
            CreditAgentRefId = CreditAgentId,
            PayDate = DateTime.Now,
            OfficialReceiptNo = "",
            BranchRefId = BranchId,
            UserRefId = _userId,
            CurrencyRefId = CurrencyId,
            CustomerName = "",
            LoanPaymentLines = LoanPaymentLines
        };

        var parameters = new Dictionary<string, object?>
        {
            {"CurrencyId" , CurrencyId},

            {"LoanPaymentCreate" , loanPayment},
        };

        var options = new SideDialogOptions
        {
            Width = "45vw",

            CloseDialogOnOverlayClick = true,

            Position = DialogPosition.Right
        };

        var dia = await DialogService.OpenSideAsync<CollectionPaymentSidebar>("Receive Payment", parameters, options);


    }

    private async Task GetSchedules()
    {
        await GetSchedules(BranchId, LoanId);
    }

    async Task GetSchedules(int? branchId, string? loanId)
    {

        Tracker.Show();

        var query = new GetDueScheduleByLoanIdQuery(branchId, loanId, ScheduleStatus.Unpaid, null);

        var rsp = await mediator.Send(query);

        LoanSchedule = null;

        schedules = rsp.Items;

        Tracker.Hide();

    }

    bool IsPaymentLineAlreadyAdd(LoanPaymentLineDtoCreate value)
    {
        return LoanPaymentLines.Any(x => x.InstallmentNo == value.InstallmentNo);

    }

    private LoanPaymentLineDtoCreate CreatePaymentLine(VSchedule schedule, ref decimal remainingAmount)
    {
        var line = new LoanPaymentLineDtoCreate
        {
            InstallmentNo = schedule.InstallmentNo,
            LoanRefId = schedule.LoanId,
            BranchRefId = schedule.BranchId,
            LoanScheduleRefId = schedule.LoanScheduleId,
            NumberOfDay = schedule.TotalDay,
            PayDate = DateTime.Now
        };

        // Full payment
        if (remainingAmount >= schedule.TotalPay)
        {
            line.PaidPenaltyAmount = schedule.PenaltyAmount;

            line.PaidCollectionFee = schedule.LeftCollectionFeeAmount;

            line.PaidInterest = schedule.LeftInterestAmount;

            line.PaidPrinciple = schedule.LeftPrinciple;

            line.SubTotal = schedule.TotalPay;

            remainingAmount -= schedule.TotalPay ?? 0m;

            return line;
        }

        // Partial payment (Penalty -> CollectionFee -> Interest -> Principal)

        line.PaidPenaltyAmount = Deduct(ref remainingAmount, schedule.PenaltyAmount);

        line.PaidCollectionFee = Deduct(ref remainingAmount, schedule.LeftCollectionFeeAmount);

        line.PaidInterest = Deduct(ref remainingAmount, schedule.LeftInterestAmount);

        line.PaidPrinciple = Deduct(ref remainingAmount, schedule.LeftPrinciple);

        line.SubTotal = Subtotal(line);

        return line;
    }

    private decimal? Subtotal(LoanPaymentLineDtoCreate? line)
    {
        if (line is null) return 0m;

        var total = (line.PaidPenaltyAmount ?? 0m)
                    + (line.PaidCollectionFee ?? 0m)
                    + (line.PaidInterest ?? 0m)
                    + (line.PaidPrinciple ?? 0m);

        return total;
    }

    private static decimal Deduct(ref decimal remaining, decimal? amount)
    {
        if (remaining <= 0 || amount.GetValueOrDefault() <= 0)
            return 0m;

        var paid = Math.Min(remaining, amount!.Value);

        remaining -= paid;

        return paid;
    }




}

//foreach (var schedule in batchSchedules)
//{
//    if (remainingAmount <= 0) break;

//    LoanPaymentLineDtoCreate? line = null;

//    if (remainingAmount >= schedule.TotalPay)
//    {
//        line = new LoanPaymentLineDtoCreate
//        {
//            InstallmentNo = schedule.InstallmentNo,
//            LoanRefId = schedule.LoanId,
//            BranchRefId = schedule.BranchId,
//            LoanScheduleRefId = schedule.LoanScheduleId,
//            NumberOfDay = schedule.TotalDay,
//            PaidPrinciple = schedule.LeftPrinciple,
//            PaidInterest = schedule.LeftInterestAmount,
//            PaidCollectionFee = schedule.LeftCollectionFeeAmount,
//            PaidPenaltyAmount = schedule.PenaltyAmount,
//            PayDate = DateTime.Now,
//            SubTotal = schedule.TotalPay
//        };

//        remainingAmount -= schedule.TotalPay;
//    }
//    else
//    {
//        line = new();

//        line.LoanRefId = schedule.LoanId;

//        line.BranchRefId = schedule.BranchId;

//        line.PayDate = DateTime.Now;

//        line.NumberOfDay = schedule.TotalDay;

//        line.LoanScheduleRefId = schedule.LoanScheduleId;

//        line.PaidPenaltyAmount = Math.Min(remainingAmount!.Value, schedule.PenaltyAmount!.Value);

//        remainingAmount -= schedule.PenaltyAmount;

//        if (remainingAmount <= 0) break;

//        line.PaidCollectionFee = Math.Min(remainingAmount!.Value, schedule.LeftCollectionFeeAmount!.Value);

//        remainingAmount -= schedule.PaidCollectionFee;

//        if (remainingAmount <= 0) break;

//        line.PaidInterest = Math.Min(remainingAmount!.Value, schedule.PaidInterest!.Value);

//        remainingAmount -= schedule.PaidInterest;

//        if (remainingAmount <= 0) break;

//        line.PaidPrinciple = Math.Min(remainingAmount!.Value, schedule.PaidPrinciple!.Value);

//        remainingAmount -= schedule.PaidPrinciple;
//    }

//    if (line is not null)
//    {
//        LoanPaymentLines.Add(line);
//    }

//}
//async Task CheckedChanged(bool arg, VSchedule schedule)
//{
//    _selectedSchedule = null;

//    if (arg)
//    {
//        var schedulesList = schedules.ToList();

//        var currentSelected = schedulesList.IndexOf(schedule);

//        if (currentSelected > 0)
//        {
//            var previousSchedule = schedulesList[currentSelected - 1];

//            bool addedItem = LoanPaymentLines.Any(x => x.InstallmentNo == previousSchedule.InstallmentNo);

//            if (addedItem)
//            {

//                var existingLine = LoanPaymentLines.FirstOrDefault(x => x.InstallmentNo == previousSchedule.InstallmentNo);

//                if (existingLine!.PaidPrinciple != previousSchedule.LeftPrinciple)
//                {
//                    _toastService.ShowWarning("Please update the payment line for the previous schedule before selecting this one.");

//                    _selectedSchedule = null;

//                    return;
//                }

//            }

//            if (!addedItem)
//            {
//                _toastService.ShowWarning("Please select previous schedule first.");

//                _selectedSchedule = previousSchedule;

//                return;
//            }
//        }

//        await UpdateMultipleSelection(schedule);

//        if (_selectedSchedules is { Count: > 0 })
//        {
//            _selectedSchedule = _selectedSchedules.Last();
//        }
//    }
//    else
//    {
//        bool removed = NotAllowFirstSelected(schedule);

//        if (removed)
//        {
//            var removedIndex = LoanPaymentLines.FindIndex(x => x.InstallmentNo == schedule.InstallmentNo &&
//                                                                x.LoanScheduleRefId == schedule.LoanScheduleId);
//            if (removedIndex != -1)
//            {
//                LoanPaymentLines.RemoveAt(removedIndex);

//                _ = paymentLinesRef.ReLoad();
//            }
//        }
//    }

//    await gridSchedule.Reload();

//    StateHasChanged();
//}
