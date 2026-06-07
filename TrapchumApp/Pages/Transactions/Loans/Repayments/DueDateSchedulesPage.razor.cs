using Applications.Calculations;
using Applications.Commons.Mappers;
using Applications.Extensions.NumberExtensions;
using Applications.Features.Branches.Dtos;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Schedules.Queries.GetDueScheduleByLoanId;
using Applications.Responses.ViewModels.Loans.Schedules;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments;

public partial class DueDateSchedulesPage
{
    [Parameter] public string? LoanId { get; set; }
    [Parameter] public int BranchId { get; set; }
    [Parameter] public string? CreditAgentId { get; set; }
    [Parameter] public string? CustomerName { get; set; } = string.Empty;
    [Parameter] public int CurrencyId { get; set; }
    [Parameter] public bool ShowOnScheduleOnly { get; set; } = true;
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    DateTime paymentDate = DateTime.Now;

    IEnumerable<VSchedule> schedules = [];

    List<LoanPaymentLineDtoCreate> LoanPaymentLines = [];

    LoanPaymentDtoCreate LoanRepayment = new();

    IEnumerable<VBranch> Branches = [];

    VSchedule? SelectedSchedule = new();

    VLoanDisbursments? LoanSchedule = new();

    RadzenDataGrid<VSchedule> gridSchedule = default!;

    PaymentLines paymentLinesRef = default!;

    IEnumerable<VLoanRepaymentLine> VLoanRepaymentLines = [];

    VLoanDisbursments? vLoan = new();

    private string? _userId = string.Empty;

    IList<VSchedule>? _selectedSchedules = null;

    DataGridSelectionMode gridSelectionMode = DataGridSelectionMode.Single;

    bool isAllowMultiplePayment = false;

    bool allowRowSelected = true;

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
    private LoanPaymentLineDtoCreate _loanPaymentLine = new();

    LoanPaymentDtoCreate _loanPayment = new();

    protected override async Task OnInitializedAsync()
    {
        _userId = (await AuthenticationState).GetUserId();

        _isSelectionLocked = !isAllowMultiplePayment;

        await base.OnInitializedAsync();
    }


    protected override async Task OnParametersSetAsync()
    {
        await GetSchedules();

        await base.OnParametersSetAsync();
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

            var line = SelectedSchedule?.ToPaymentLine(recievePayment);

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
            Resizable = false,
            Width = "35rem",
            ShowClose = false
        };

        var loanPayment = new LoanPaymentDtoCreate
        {
            LoanRefId = LoanId,
            CreditAgentRefId = CreditAgentId,
            BranchRefId = BranchId,
            CurrencyRefId = CurrencyId,
            UserRefId = _userId,
            PayDate = DateTime.Now,
            CustomerName = CustomerName,
            OfficialReceiptNo = _loanPaymentLine.OfficialReceiptNo
        };

        loanPayment.LoanPaymentLines.Add(_loanPaymentLine);

        var parameters = new Dictionary<string, object?>{
                {"CurrencyId" , CurrencyId},
                {"LoanPaymentCreate",loanPayment },
            };

        var message = $"Recieve Payment from {CustomerName}";

        bool? confirm = await DialogService.OpenSideAsync<PaymentSideBars>(message, parameters!, options);

        if (confirm is not null && confirm == true)
        {

            _toastService.ShowSuccess("Receive successfully.");

            await GetSchedules();

            SetDefaultValue();

        }
        else
        {

            _toastService.ShowInfo("Receive cancelled.");

            SetDefaultValue();
        }

    }

    private async Task GetSchedules()
    {
        if (ShowOnScheduleOnly)
        {
            await GetSchedules(BranchId, LoanId, ScheduleStatus.Unpaid);
        }
        else
        {
            await GetSchedules(BranchId, LoanId, ScheduleStatus.All);
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
        var schedulesList = schedules?.ToList();

        var firstSchedule = schedulesList?.First(x => x.IsPaid == false);

        return p == firstSchedule;

    }

    async Task ApplyMultipleSchedulePayment()
    {

        if (!isAllowMultiplePayment)
        {
            _toastService.ShowError("Please select schedule to apply payment.");

            return;
        }

        if (!_selectedSchedules!.Any())
        {
            _toastService.ShowInfo("There no any schedules are selected.");

            return;

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
            CustomerName = LoanSchedule!.AltName
        };

        var parameters = new Dictionary<string, object?>
        {
            {"Schedules",_selectedSchedules},

            {"CurrencyId" , CurrencyId},

            {"LoanPayment" , loanPayment},
        };


        AddMultiplePaymentTab(_selectedSchedules, loanPayment);


    }


    async Task CheckPaymentChanged(bool arg)
    {

        if (!arg)
        {

            _loanPaymentLine = new();

            isPaidNow = false;

            if (_isPaymentTab)
            {
                _isPaymentTab = false;

                _selectedIndexTab = 0;
            }

            return;
        }

        if (SelectedSchedule is null)
        {

            _toastService.ShowError("There is no any schedule is selected.","Recieve Payment");

            return;
        }

        var schedulesList = schedules.ToList();

        var firstSchedule = schedulesList.First(x=>x.IsPaid == false);

        if (!IsFirstScheduleSelected(firstSchedule))
        {
            _toastService.ShowError("Cannot add new schedule due to previouse schedule not complete paid yet.", "Recieve Payment");

            SetDefaultValue();

            StateHasChanged();

            return;
        }

        isPaidNow = true;

        decimal? total = SelectedSchedule.SubTotal();

        if (total > 0)
        {
            _loanPaymentLine = SelectedSchedule.MapToLoanPaymentLine();

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
    }
    async void MultipleCheckChanged(bool arg)
    {

        isAllowMultiplePayment = arg;

        if (arg)
        {
            gridSelectionMode = DataGridSelectionMode.Multiple;

            allowRowSelected = false;
        }
        else
        {
            gridSelectionMode = DataGridSelectionMode.Single;

            allowRowSelected = true;
        }
        _selectedSchedules = schedules.Take(1).ToList();

        SelectedSchedule = new();

        isPaidNow = false;

        btnDisabled = true;

        LoanPaymentLines.Clear();

        Clear();

        await gridSchedule.Reload();
    }


    async Task CheckedChanged(bool arg, VSchedule schedule)
    {

        if (!isAllowMultiplePayment) return;

        if (arg)
        {
            await UpdateMultipleSelection(schedule);
        }
        else
        {
            await NotAllowFirstSelected(schedule);
        }


        await gridSchedule.Reload();

        StateHasChanged();
    }


    async Task GetLoanWithSchedules(int? branchId, string? loanId)
    {

        isLoading = true;

        var query = new GetDueScheduleByLoanIdQuery(branchId, loanId);

        var rsp = await mediator.Send(query);

        schedules = rsp.Items.ToList();

        _selectedSchedules = schedules.Take(1).ToList();

        if (_selectedSchedules.Any() && !isAllowMultiplePayment)
        {
            SelectedSchedule = _selectedSchedules.First();

            disableIsPaidNow = false;
        }

        isLoading = false;


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
            SelectedSchedule = _selectedSchedules.First();

            disableIsPaidNow = false;
        }

        isLoading = false;

    }

    async Task SelectedRowEvt(VSchedule schedule)
    {
        if (isAllowMultiplePayment)
        {
            await UpdateMultipleSelection(schedule);
        }
        else
        {
            await UpdateSingleSelection(schedule);
        }

    }

    async Task DeSelectedRowEvt(VSchedule schedule)
    {
        if (_selectedSchedules != null && _selectedSchedules.Contains(schedule))
        {
            _selectedSchedules.Remove(schedule);

            await gridSchedule.Reload();

            StateHasChanged();
        }
    }



    void OnRemoveAdded(LoanPaymentLineDtoCreate value)
    {
        var removeIndex = LoanPaymentLines.IndexOf(value);

        if (removeIndex < LoanPaymentLines.Count - 1)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Cannot remove first schedule.");
            return;
        }

        if (removeIndex == LoanPaymentLines.Count - 1)
        {
            LoanPaymentLines.RemoveAt(removeIndex);

            var changedSchedule = _selectedSchedules!.Where(x => x.InstallmentNo == value.InstallmentNo).FirstOrDefault();

            if (changedSchedule is not null)
            {
                var scheduleIndex = _selectedSchedules!.IndexOf(changedSchedule);

                _selectedSchedules[scheduleIndex].Selected = false;

                gridSchedule.SelectRow(changedSchedule, false);

            }

            totalPaidAmount = LoanPaymentLines.Total();

        }
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

        LoanPaymentLines.Clear();

        _loanPaymentLine = new();

    }
    void PrincipleAmountValueChanged(ChangeEventArgs e)
    {

        if (string.IsNullOrEmpty(e.Value?.ToString()))
        {

            _toastService.ShowWarning("Priciple amount must greater than zero.");

            recievePayment.RecievePrincipleAmount = SelectedSchedule?.LeftPrinciple ?? 0.00M;

            return;
        }
        var inputValue = e.Value?.ToNumber<decimal>();

        if (inputValue > SelectedSchedule?.LeftPrinciple)
        {
            _toastService.ShowWarning("Priciple amount must less than or equal to left principle amount.");

            recievePayment.RecievePrincipleAmount = SelectedSchedule?.LeftPrinciple ?? 0.00M;

            return;
        }

    }
    void InterestAmountValueChanged(ChangeEventArgs e)
    {

        if (string.IsNullOrEmpty(e.Value?.ToString()))
        {
            interestAmount = SelectedSchedule?.LeftInterestAmount ?? 0.00M;
            return;
        }

        var inputValue = e.Value?.ToNumber<decimal>();

        if (inputValue >= 0 && inputValue <= SelectedSchedule?.LeftInterestAmount)
        {
            totalPaymentAmount = LoanExtensions.SubTotal(principleAmount, inputValue, collectionfeeAmount, penaltyAmount);
            return;
        }
        if (inputValue <= 0 || inputValue > SelectedSchedule?.LeftInterestAmount)
        {
            interestAmount = SelectedSchedule?.LeftInterestAmount ?? 0.00M;
            return;
        }

    }
    void CollectionFeeAmountValueChanged(ChangeEventArgs e)
    {


        if (string.IsNullOrEmpty(e.Value?.ToString()))
        {
            collectionfeeAmount = SelectedSchedule?.LeftCollectionFeeAmount ?? 0.00M;
            return;
        }


        var inputValue = e.Value?.ToNumber<decimal>();

        if (inputValue >= 0 && inputValue <= SelectedSchedule?.LeftCollectionFeeAmount)
        {
            totalPaymentAmount = LoanExtensions.SubTotal(principleAmount, interestAmount, inputValue, penaltyAmount);
            return;
        }
        if (inputValue < 0 || inputValue > SelectedSchedule?.LeftCollectionFeeAmount)
        {
            collectionfeeAmount = SelectedSchedule?.LeftCollectionFeeAmount ?? 0.00M;
            return;
        }
    }


    private async Task NotAllowFirstSelected(VSchedule schedule)
    {
        if (!_selectedSchedules!.Any()) return;

        if (_selectedSchedules!.Count > 1 && _selectedSchedules.First() == schedule)
        {
            // Block unchecking the first item if more than 2 are selected
            _toastService.ShowError("Cannot uncheck the first selected item.");

            return;
        }

        if (_selectedSchedules.Last() == schedule)
        {
            // Allow unchecking the last item
            _selectedSchedules.Remove(schedule);

            await gridSchedule.SelectRow(schedule, false);
        }
        else
        {
            // Block unchecking non-last items if more than 2 are selected
            if (_selectedSchedules.Count > 1)// default 2
            {

                _toastService.ShowError("Only the last selected item can be unchecked.");

                return;
            }
            else
            {
                // Allow unchecking if 2 or fewer items
                _selectedSchedules.Remove(schedule);
            }
        }
    }


    private async Task UpdateSingleSelection(VSchedule schedule)
    {
        if (isAllowMultiplePayment) return;

        if (IsAlreadyPaidSchedule(schedule)) return;

        if (!IsFirstScheduleSelected(schedule))
        {
            _toastService.ShowError("Cannot add new schedule due to previouse schedule not complete paid yet.","Recieve Payment");

            _selectedSchedules!.Clear();

            SelectedSchedule = null;

            await gridSchedule.Reload();

            StateHasChanged();

            return;
        }

        _selectedSchedules = new List<VSchedule> { schedule };

        SelectedSchedule = schedule;

        disableIsPaidNow = false;

    }

    async Task UpdateMultipleSelection(VSchedule schedule)
    {
        if (schedule == null)
        {
            _selectedSchedules = [];
        }

        var schedulesList = schedules.ToList();

        var firstSchedule = schedulesList.First();


        // If no rows are selected, add the clicked row
        if (!_selectedSchedules!.Any())
        {
            if (schedule != firstSchedule)
            {
                // Automatically include the first row
                _toastService.ShowWarning("Only The first schedule is allow selected.");

                return;
            }
            else
            {
                _selectedSchedules!.Add(schedule);

                await gridSchedule.SelectRow(schedule);
            }

        }

        int newIndex = schedulesList.IndexOf(schedule!);

        var selectedIndices = _selectedSchedules!.Select(e => schedulesList.IndexOf(e)).ToList();

        // Check if the new selection is contiguous
        int minIndex = selectedIndices.Min();

        int maxIndex = selectedIndices.Max();

        bool isContiguous = newIndex == minIndex - 1 || newIndex == maxIndex + 1 || selectedIndices.Contains(newIndex);

        if (isContiguous)
        {
            if (!_selectedSchedules!.Contains(schedule!))
            {
                _selectedSchedules.Add(schedule!);

                await gridSchedule.SelectRow(schedule!);
            }
        }


        await gridSchedule.Reload();

        StateHasChanged();

    }

}


