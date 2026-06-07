using Applications.Calculations;
using Applications.Commons.Mappers;
using Applications.Extensions.NumberExtensions;
using Applications.Features.Branches.Dtos;
using Applications.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Schedules.Queries.GetDueScheduleByLoanId;
using Applications.Responses.ViewModels.Loans.Schedules;
using Radzen.Blazor;
using TrapchumApp.Pages.Transactions.Loans.Repayments;
namespace TrapchumApp.Pages.Transactions.RecievePayements;

public partial class CollectionPaymentPage
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

    IEnumerable<VBranch> Branches = [];

    VSchedule? _selectedSchedule = null;

    VLoanDisbursments? LoanSchedule = new();

    RadzenDataGrid<VSchedule> gridSchedule = default!;

    PaymentLines paymentLinesRef = default!;

    IEnumerable<VLoanRepaymentLine> VLoanRepaymentLines = [];

    VLoanDisbursments? vLoan = new();

    IList<VSchedule>? _selectedSchedules = null;

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


        btnDisabled = true;

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

    async Task ApplySchedulePayment()
    {

        if (!isAllowMultiplePayment && !isPaidNow)
        {
            _toastService.ShowError("Please select schedule to apply payment.");

            return;
        }
        if (!isAllowMultiplePayment && isPaidNow)
        {
            LoanPaymentLines.Clear();

            var line = _selectedSchedule?.ToPaymentLine(recievePayment);

            if (line is not null)
            {
                LoanPaymentLines.Add(line);
            }
        }

        if (LoanPaymentLines.Count <= 0)
        {
            _toastService.ShowError("There is any schedule selected yet!");

            return;
        }

        var options = new SideDialogOptions
        {
            Resizable = true,
            Width = "35rem",
            CssClass = "rz-p-1 rz-m-0 rz-background-color-base-200",
            ShowClose = false,
            CloseDialogOnOverlayClick = false
        };

        var loanPayment = new LoanPaymentDtoCreate
        {
            LoanRefId = LoanId,
            CreditAgentRefId = CreditAgentId,
            BranchRefId = BranchId,
            CurrencyRefId = CurrencyId,
            UserRefId = _userId,
            PayDate = DateTime.Now,
            CustomerName = AltName,
            OfficialReceiptNo = _loanPaymentLine?.OfficialReceiptNo
        };

        if (_loanPaymentLine is not null)
        {
            loanPayment.LoanPaymentLines.Add(_loanPaymentLine);

        }

        _loanPayment = loanPayment;

        var parameters = new Dictionary<string, object?>{
                {"CurrencyId" , CurrencyId},
                {"LoanPaymentCreate",loanPayment },
                {"CustomerName",AltName }
            };

        var message = $"Recieve Payment from {AltName}";

        bool? confirm = await DialogService.OpenSideAsync<PaymentOffSideBar>(message, parameters!, options);


        if (confirm == true)
        {

            disableIsPaidNow = true;

            isPaidNow = false;

            await GetSchedulesByLoanIdAsync(ScheduleStatus.Unpaid);

            await ReloadTab.InvokeAsync();
        }

    }



    async Task ApplyMultipleSchedulePayment()
    {

        if (!isAllowMultiplePayment)
        {
            _toastService.ShowError("Please select schedule to apply payment.");

            return;
        }

        if (_selectedSchedules is null || !_selectedSchedules.Any())
        {
            _toastService.ShowWarning("There no any schedules are selected.");

            return;

        }

        var confirm = await MsgBox.ConfirmAsync("Do you want to apply this schedules?", "Receive Payment");

        if (confirm == MsgBoxResult.No) return;

        var loanPayment = new LoanPaymentDtoCreate
        {
            LoanRefId = LoanId,
            CreditAgentRefId = CreditAgentId,
            PayDate = DateTime.Now,
            OfficialReceiptNo = "",
            BranchRefId = BranchId,
            UserRefId = _userId,
            CurrencyRefId = CurrencyId,
            // CustomerName = LoanSchedule!.AltName
        };

        loanPayment.LoanPaymentLines = LoanPaymentLines;

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



    async Task CheckPaymentChanged(bool arg)
    {

        if (!arg)
        {

            _loanPaymentLine = new();

            isPaidNow = false;

            btnDisabled = true;

            return;
        }
        else
        {
            btnDisabled = true;
        }

        if (_selectedSchedule is null)
        {

            _toastService.ShowError("There is no any schedule is selected.");

            return;
        }

        isPaidNow = true;

        _loanPaymentLine = _selectedSchedule.MapToLoanPaymentLine();

        decimal? total = _loanPaymentLine.AsSubTotal();

        if (total > 0)
        {

            btnDisabled = false;
        }
        else
        {
            Clear();
        }

    }
    void Clear()
    {
        recievePayment = new();
        principleAmount = 0.00M;
        interestAmount = 0.00M;
        collectionfeeAmount = 0.00M;
        penaltyAmount = 0.00M;
        totalPaymentAmount = 0.00M;
        recieptNumber = string.Empty;
        receiptNo = string.Empty;
        isPaidNow = false;
        disableIsPaidNow = false;
        btnDisabled = true;
    }

    async Task CheckedChanged(bool arg, VSchedule schedule)
    {
        _selectedSchedule = null;

        if (arg)
        {
            var schedulesList = schedules.ToList();

            var currentSelected = schedulesList.IndexOf(schedule);

            if (currentSelected > 0)
            {
                var previousSchedule = schedulesList[currentSelected - 1];

                bool addedItem = LoanPaymentLines.Any(x => x.InstallmentNo == previousSchedule.InstallmentNo);

                if (addedItem)
                {

                    var existingLine = LoanPaymentLines.FirstOrDefault(x => x.InstallmentNo == previousSchedule.InstallmentNo);

                    if (existingLine!.PaidPrinciple != previousSchedule.LeftPrinciple)
                    {
                        _toastService.ShowWarning("Please update the payment line for the previous schedule before selecting this one.");

                        _selectedSchedule = null;

                        return;
                    }

                }

                if (!addedItem)
                {
                    _toastService.ShowWarning("Please select previous schedule first.");

                    _selectedSchedule = previousSchedule;

                    return;
                }
            }

            await UpdateMultipleSelection(schedule);

            if (_selectedSchedules is { Count: > 0 })
            {
                _selectedSchedule = _selectedSchedules.Last();
            }
        }
        else
        {
            bool removed = NotAllowFirstSelected(schedule);

            if (removed)
            {
                var removedIndex = LoanPaymentLines.FindIndex(x => x.InstallmentNo == schedule.InstallmentNo &&
                                                                    x.LoanScheduleRefId == schedule.LoanScheduleId);
                if (removedIndex != -1)
                {
                    LoanPaymentLines.RemoveAt(removedIndex);

                    _ = paymentLinesRef.ReLoad();
                }
            }
        }

        await gridSchedule.Reload();

        StateHasChanged();
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

        //_selectedSchedules = schedules.Take(1).ToList();

        //if (_selectedSchedules is not null && _selectedSchedules.Count != 0)
        //{
        //    _selectedSchedule = null;

        //}

        Tracker.Hide();

    }


    async Task GetSchedules(int? branchId, string? loanId, ScheduleStatus status = ScheduleStatus.Unpaid)
    {

        isLoading = true;

        var query = new GetDueScheduleByLoanIdQuery(branchId, loanId, status);

        var rsp = await mediator.Send(query);

        LoanSchedule = null;

        schedules = rsp.Items;

        _selectedSchedules = schedules.Take(1).ToList();

        if (_selectedSchedules.Any() && !isAllowMultiplePayment)
        {
            _selectedSchedule = _selectedSchedules.First();

            disableIsPaidNow = false;
        }

        isLoading = false;

    }



    async Task GetLoanWithSchedules(int? branchId, string? loanId)
    {

        isLoading = true;


        var query = new GetLoanByIdQuery(LoanId, BranchId, ScheduleStatus.Unpaid, false);

        var rsp = await mediator.Send(query);

        if (!rsp.IsSuccess())
        {
            LoanSchedule = null;

            schedules = [];

            _selectedSchedules = [];

            isLoading = false;

            return;
        }

        LoanSchedule = rsp.Value;

        if (LoanSchedule is null)
        {
            schedules = [];

            _selectedSchedules = [];

            isLoading = false;

            return;
        }

        schedules = LoanSchedule.Schedules!.ToList();

        _selectedSchedules = schedules.Take(1).ToList();

        if (_selectedSchedules.Any() && !isAllowMultiplePayment)
        {
            _selectedSchedule = _selectedSchedules.First();

            disableIsPaidNow = false;
        }

        isLoading = false;


    }

    async Task GetSchedulesByLoanIdAsync(ScheduleStatus status)
    {
        try
        {
            isLoading = true;

            schedules = [];

            _selectedSchedules = [];


            var query = new GetLoanSchedulesByLoanIdQuery(LoanId, BranchId, status);

            var rsp = await mediator.Send(query);

            if (!rsp.IsError)
            {
                schedules = rsp.Value;

                _selectedSchedules = schedules.Take(1).ToList();
            }

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



    async Task OnRemoveAdded(LoanPaymentLineDtoCreate value)
    {
        if (_selectedSchedules is null || !_selectedSchedules.Any())
        {
            _toastService.ShowError("There is no any schedule is selected.");

            return;
        }


        var removeIndex = LoanPaymentLines.IndexOf(value);

        if (removeIndex < LoanPaymentLines.Count - 1)
        {
            _toastService.ShowWarning("Cannot remove first schedule.");

            return;
        }

        var confirm = await MsgBox.ConfirmAsync("Do you want to remove this schedule?", "Remove Schedule");

        if (confirm == MsgBoxResult.No) return;

        if (removeIndex == LoanPaymentLines.Count - 1)
        {
            LoanPaymentLines.RemoveAt(removeIndex);

            var changedSchedule = _selectedSchedules.Where(x => x.InstallmentNo == value.InstallmentNo).FirstOrDefault();

            if (changedSchedule is not null)
            {
                var scheduleIndex = _selectedSchedules.IndexOf(changedSchedule);

                _selectedSchedules.RemoveAt(scheduleIndex);

            }

            totalPaidAmount = LoanPaymentLines.Total();

        }
    }



    bool IsPaymentLineAlreadyAdd(LoanPaymentLineDtoCreate value)
    {
        return LoanPaymentLines.Any(x => x.InstallmentNo == value.InstallmentNo);

    }


    void AddPaymentLines()
    {
        if (!isPaidNow)
        {
            _toastService.ShowError("Please select schedule to apply payment.");

            return;
        }

        if (_loanPaymentLine is null) return;


        if (IsPaymentLineAlreadyAdd(_loanPaymentLine))
        {

            _toastService.ShowInfo("selected schedule is already selected.");

            return;
        }

        LoanPaymentLines.Add(_loanPaymentLine);

        _ = paymentLinesRef.ReLoad();

        _loanPaymentLine = new();

        _selectedSchedule = null;

        Clear();

        StateHasChanged();


    }

    void SetDefaultValue()
    {
        principleAmount = 0.00M;
        interestAmount = 0.00M;
        collectionfeeAmount = 0.00M;
        penaltyAmount = 0.00M;
        btnDisabled = true;
        isPaidNow = false;
        disableIsPaidNow = true;
        receiptNo = string.Empty;
    }
    void PrincipleAmountValueChanged(ChangeEventArgs e)
    {

        if (string.IsNullOrEmpty(e.Value?.ToString()))
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Priciple amount must greater than zero.");
            recievePayment.RecievePrincipleAmount = _selectedSchedule?.LeftPrinciple ?? 0.00M;
            return;
        }
        var inputValue = e.Value?.ToNumber<decimal>();

        if (inputValue > _selectedSchedule?.LeftPrinciple)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Priciple amount must greater than zero.");
            recievePayment.RecievePrincipleAmount = _selectedSchedule?.LeftPrinciple ?? 0.00M;
            return;
        }
    }
    void InterestAmountValueChanged(ChangeEventArgs e)
    {

        if (string.IsNullOrEmpty(e.Value?.ToString()))
        {
            interestAmount = _selectedSchedule?.LeftInterestAmount ?? 0.00M;
            return;
        }

        var inputValue = e.Value?.ToNumber<decimal>();

        if (inputValue >= 0 && inputValue <= _selectedSchedule?.LeftInterestAmount)
        {
            totalPaymentAmount = LoanExtensions.SubTotal(principleAmount, inputValue, collectionfeeAmount, penaltyAmount);
            return;
        }
        if (inputValue <= 0 || inputValue > _selectedSchedule?.LeftInterestAmount)
        {
            interestAmount = _selectedSchedule?.LeftInterestAmount ?? 0.00M;
            return;
        }

    }
    void CollectionFeeAmountValueChanged(ChangeEventArgs e)
    {


        if (string.IsNullOrEmpty(e.Value?.ToString()))
        {
            collectionfeeAmount = _selectedSchedule?.LeftCollectionFeeAmount ?? 0.00M;
            return;
        }


        var inputValue = e.Value?.ToNumber<decimal>();

        if (inputValue >= 0 && inputValue <= _selectedSchedule?.LeftCollectionFeeAmount)
        {
            totalPaymentAmount = LoanExtensions.SubTotal(principleAmount, interestAmount, inputValue, penaltyAmount);
            return;
        }
        if (inputValue < 0 || inputValue > _selectedSchedule?.LeftCollectionFeeAmount)
        {
            collectionfeeAmount = _selectedSchedule?.LeftCollectionFeeAmount ?? 0.00M;
            return;
        }
    }


    private bool NotAllowFirstSelected(VSchedule schedule)
    {
        if (_selectedSchedules is null || !_selectedSchedules.Any()) return false;

        // Block unchecking the first item when multiple are selected
        if (_selectedSchedules.Count > 1 && _selectedSchedules.First() == schedule)
        {
            _toastService.ShowError("Cannot uncheck the first selected item.");
            return false;
        }

        // Only allow unchecking the last item (or the only remaining item)
        if (_selectedSchedules.Last() == schedule)
        {
            _selectedSchedules.Remove(schedule);
            return true;
        }

        // Block unchecking any middle item
        _toastService.ShowError("Only the last selected item can be unchecked.");
        return false;
    }


    async Task UpdateMultipleSelection(VSchedule schedule)
    {
        if (schedule is null)
        {
            _selectedSchedules = [];

            await gridSchedule.Reload();

            return;
        }

        _selectedSchedules ??= new List<VSchedule>();

        var schedulesList = schedules.ToList();

        var firstSchedule = schedulesList.First();

        if (!_selectedSchedules.Any())
        {
            if (schedule != firstSchedule)
            {
                _toastService.ShowWarning("Only the first schedule is allowed to be selected first.");

                return;
            }

            _selectedSchedules.Add(schedule);

            return;
        }

        int newIndex = schedulesList.IndexOf(schedule);

        var selectedIndices = _selectedSchedules.Select(e => schedulesList.IndexOf(e)).ToList();

        int minIndex = selectedIndices.Min();

        int maxIndex = selectedIndices.Max();

        if (selectedIndices.Contains(newIndex))
        {
            return;
        }

        bool isContiguous = newIndex == minIndex - 1 || newIndex == maxIndex + 1;

        if (!isContiguous)
        {
            _toastService.ShowWarning("You can only select schedules in a sequential order without skipping.");

            return;
        }

        _selectedSchedules.Add(schedule);
    }


}
