using Applications.Calculations;
using Applications.Calculations.Extensions;
using Applications.Commons.Mappers;
using Applications.Enumerations;
using Applications.Extensions.NumberExtensions;
using Applications.Features.LoanPayments.Interfaces;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.Loans.Services;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Domains.Models.Loans;
using Radzen.Blazor;
using System.Threading.Tasks;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments;

public partial class PaymentSchedule
{

    private readonly ILoanServiceImpl _loanService;

    private readonly ILoanScheduleServiceImpl _scheduleService;

    private readonly IAddRecieptNumberUseCase _addRecieptNumberUseCase;
    public PaymentSchedule(ILoanServiceImpl loanService, ILoanScheduleServiceImpl loanScheduleService, IAddRecieptNumberUseCase addRecieptNumberUseCase)
    {
        _loanService = loanService;

        _scheduleService = loanScheduleService;

        _addRecieptNumberUseCase = addRecieptNumberUseCase;
    }

    [Parameter] public string? LoanId { get; set; }
    [Parameter] public int BranchId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? CreditAgentId { get; set; }
    [Parameter, SupplyParameterFromQuery] public int CurrencyId { get; set; }
    private string? _userId = string.Empty;
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    DateTime paymentDate = DateTime.Now;

    IEnumerable<VSchedule> schedules = [];

    List<LoanPaymentLineDtoCreate> LoanPaymentLines = [];

    LoanPaymentDtoCreate LoanRepayment = new();

    IEnumerable<VBranchResponse> Branches = [];

    VSchedule? SelectedSchedule = new();

    VLoanDisbursments? LoanSchedule = new();

    RadzenDataGrid<VSchedule> gridSchedule = default!;

    PaymentLines paymentLinesRef = default!;

    IEnumerable<VLoanRepaymentLine> VLoanRepaymentLines = [];

    VLoanDisbursments? vLoan = new();

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
        //btnDisabled = true;
        _userId = (await AuthenticationState).GetUserId();

        await GetLoanWithSchedules(BranchId, LoanId);

        _isSelectionLocked = !isAllowMultiplePayment;

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



    async Task CloseTab()
    {

        if (isAllowMultiplePayment && _isMultiplePaymentTab)
        {
            _isPaymentTab = false;

            _selectedIndexTab = 1;
        }
        if (!isAllowMultiplePayment && _isPaymentTab)
        {
            _isPaymentTab = false;

            _selectedIndexTab = 0;

            isPaidNow = false;
        }

        _showExchanged = false;

        await GetLoanWithSchedules(BranchId, LoanId);

        _loanPaymentLine = new();
    }


    void ClosedTab()
    {

        if ((isAllowMultiplePayment && _isMultiplePaymentTab) ||
            (isAllowMultiplePayment && !_isPaymentTab))
        {
            _isMultiplePaymentTab = false;

            _isPaymentTab = false;

            _selectedIndexTab = 0;
        }

        _selectedSchedules?.Clear();

    }


    void CreatePayment(LoanPaymentDtoCreate? arg)
    {
        if (arg is null) return;

        {
            _isPaymentTab = true;

            _selectedIndexTab = 2;

            _loanPayment = arg;
        }

    }


    void ApplySchedulePayment()
    {

        if (!isAllowMultiplePayment && !isPaidNow)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Please select schedule to apply payment.");

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
            NotificationService.Notify(NotificationSeverity.Error, "There is any schedule selected yet!");

            return;
        }

        PaymentCreate create = LoanPaymentLines.ToPaymentCreate(CurrencyId);

        var options = new DialogOptions
        {
            Resizable = false,

            Draggable = true,

            CssClass = "rz-p-1 rz-m-0 rz-background-color-base-200"
        };

        var loanPayment = new LoanPaymentDtoCreate
        {
            LoanRefId = LoanId,
            CreditAgentRefId = CreditAgentId,
            BranchRefId = BranchId,
            CurrencyRefId = CurrencyId,
            UserRefId = _userId,
            PayDate = DateTime.Now,
            CustomerName = LoanSchedule!.AltName,
            OfficialReceiptNo = _loanPaymentLine.OfficialReceiptNo
        };

        loanPayment.LoanPaymentLines.Add(_loanPaymentLine);

        _isPaymentTab = true;

        _selectedIndexTab = 2;

        _loanPayment = loanPayment;

        //LoanPaymentCreate
        //var parameters = new Dictionary<string, object?>{
        //        {"CurrencyId" , CurrencyId},
        //        {"LoanPaymentCreate",loanPayment },
        //    };

        //var message = $"Recieve Payment from {LoanSchedule!.AltName}";

        //bool? confirm = await DialogService.OpenAsync<PaymentModal>(message, parameters, options);




        //if (confirm is not null && confirm == true)
        //{
        //    NotificationService.Notify(NotificationSeverity.Success, "Receive successfully.");

        //    await GetSchedulesByLoanIdAsync(ScheduleStatus.Unpaid);

        //    LoanPaymentLines.Clear();

        //    await gridSchedule.Reload();

        //    Reset();
        //}



    }



    async Task ApplyMultipleSchedulePayment()
    {

        if (!isAllowMultiplePayment)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Please select schedule to apply payment.");

            return;
        }

        if (!_selectedSchedules!.Any())
        {
            NotificationService.Notify(NotificationSeverity.Info, "There no any schedules are selected.");
            return;

        }

        var confirm = await DialogService.Confirm("Do you want to apply this schedules?", "Receive Payment");

        if (confirm == false)
        {
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

        var options = new SideDialogOptions
        {
            Width = "50vw",

            CloseDialogOnOverlayClick = true,

            Position = DialogPosition.Right
        };


        _selectedIndexTab = 1;

        _isMultiplePaymentTab = true;


        //var result = await DialogService.OpenSideAsync<PaymentMultiples>("Payment", parameters, options);

        //if (result is bool dialogueResult && dialogueResult)
        //{
        //    SelectedSchedules!.Clear();

        //    await GetSchedulesByLoanIdAsync(ScheduleStatus.Unpaid);

        //    await gridSchedule.Reload();
        //}
        //else
        //{
        //    SelectedSchedules!.Clear();

        //    await GetSchedulesByLoanIdAsync(ScheduleStatus.Unpaid);

        //    await gridSchedule.Reload();
        //}
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
            NotificationService.Notify(NotificationSeverity.Error, "There is no any schedule is selected.");

            return;
        }

        var schedulesList = schedules.ToList();

        var firstSchedule = schedulesList.First();

        if (SelectedSchedule != firstSchedule)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Cannot add new schedule due to previouse schedule not complete paid yet.");

            SetDefaultValue();

            StateHasChanged();

            return;
        }

        isPaidNow = true;

        decimal? total = SelectedSchedule.SubTotal();

        if (total > 0)
        {
            _loanPaymentLine = SelectedSchedule.MapToLoanPaymentLine();

            var rsp = await _addRecieptNumberUseCase.ExecuteAsync();

            if (rsp.IsSuccess)
            {
                _loanPaymentLine.OfficialReceiptNo = rsp.Data!;
            }

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

        var responseSchedule = await _loanService.GetLoanByIdAsync(loanId, BranchId, ScheduleStatus.Unpaid, true);

        if (!responseSchedule.IsSuccess && responseSchedule.Data is null)
        {
            LoanSchedule = null;
            schedules = [];
            _selectedSchedules = [];
            isLoading = false;
            return;
        }

        LoanSchedule = responseSchedule.Data;

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
            SelectedSchedule = _selectedSchedules.First();

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

            var rsp = await _scheduleService.GetLoanScheduleByIdAsync(LoanId, BranchId, status);

            if (rsp.IsSuccess)
            {
                schedules = rsp.Data!;

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
    //void DeSelectedSchedule()
    //{
    //    if (isAllowMultiplePayment)
    //    {

    //    }

    //}

    //async void OnPaid()
    //{
    //    if (SelectedSchedule is not null && LoanPaymentLines.Any(x => x.InstallmentNo == SelectedSchedule!.InstallmentNo))
    //    {
    //        SetDefaultValue();
    //        return;
    //    }
    //    if (SelectedSchedule is not null && recievePayment is not null)
    //    {

    //        var confirm = await DialogService.Confirm("Are you sure want add this payment?", "Confirm");

    //        if (confirm == false) return;

    //        var line = SelectedSchedule.ToPaymentLine(recievePayment);

    //        schedules.ToList()[selectedIndex].Selected = true;

    //        LoanPaymentLines.Add(line);

    //        await paymentLinesRef.ReLoad();

    //        Clear();

    //        btnDisabled = true;
    //    }
    //    StateHasChanged();
    //}

    //void Reset()
    //{
    //    DeSelectedSchedule();
    //    Clear();
    //    LoanPaymentLines.Clear();
    //    SelectedSchedules = [];
    //    SelectedSchedule = new();
    //    disableIsPaidNow = true;

    //}

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
    }
    void PrincipleAmountValueChanged(ChangeEventArgs e)
    {

        if (string.IsNullOrEmpty(e.Value?.ToString()))
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Priciple amount must greater than zero.");
            recievePayment.RecievePrincipleAmount = SelectedSchedule?.LeftPrinciple ?? 0.00M;
            return;
        }
        var inputValue = e.Value?.ToNumer<decimal>();

        if (inputValue > SelectedSchedule?.LeftPrinciple)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Priciple amount must greater than zero.");
            recievePayment.RecievePrincipleAmount = SelectedSchedule?.LeftPrinciple ?? 0.00M;
            return;
        }

        //if (inputValue <= SelectedSchedule?.LeftPrinciple)
        //{
        //    totalPaymentAmount = LoanExtensions.SubTotal(inputValue, interestAmount, collectionfeeAmount, penaltyAmount);
        //    return;
        //}


    }
    void InterestAmountValueChanged(ChangeEventArgs e)
    {

        if (string.IsNullOrEmpty(e.Value?.ToString()))
        {
            interestAmount = SelectedSchedule?.LeftInterestAmount ?? 0.00M;
            return;
        }

        var inputValue = e.Value?.ToNumer<decimal>();

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


        var inputValue = e.Value?.ToNumer<decimal>();

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
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Cannot Uncheck",
                Detail = "Cannot uncheck the first selected item.",
                Duration = 3000
            });

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
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Cannot Uncheck",
                    Detail = "Only the last selected item can be unchecked.",
                    Duration = 3000
                });
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
        if (!isAllowMultiplePayment)
        {
            var schedulesList = schedules.ToList();

            VSchedule firstRow = schedulesList.First();

            if (schedule == firstRow)
            {
                // Allow selection of the first row
                _selectedSchedules = new List<VSchedule> { schedule };

                SelectedSchedule = schedule;

                disableIsPaidNow = false;
            }
            else
            {
                // Clear selection for non-first row
                _selectedSchedules!.Clear();

                SelectedSchedule = null;

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Invalid Selection",
                    Detail = "Only the first row can be selected.",
                    Duration = 3000
                });

            }

            await gridSchedule.Reload();

            StateHasChanged();

        }
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
                //SelectedSchedules!.Add(firstSchedule);
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "First Row Selected",
                    Detail = "Only The first schedule is allow selected.",
                    Duration = 3000
                });
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

    // Handle select all checkbox
    //async Task SelectAll(bool? selectAll)
    //{
    //    if (selectAll == true)
    //    {
    //        SelectedSchedules = schedules.ToList();
    //    }
    //    else
    //    {
    //        SelectedSchedules!.Clear();
    //    }

    //    await gridSchedule.Reload();

    //    StateHasChanged();
    //}

    // Clear selection
    //void ClearSelection()
    //{
    //    SelectedSchedules!.Clear();
    //    isSelectionLocked = false; // Unlock selection
    //    StateHasChanged();
    //}
}



//void SetSelectedSchedule(VSchedule? s)
//{
//    if (s is not null)
//    {

//        SelectedSchedule = s;

//        SelectedSchedule.LoanId = LoanId;

//        SelectedSchedule.Selected = true;

//        disableIsPaidNow = false;
//    }
//    else
//    {
//        SelectedSchedule = null;
//    }
//}

//bool IsPreviousRowSelected(int rowIndex)
//{
//    if (rowIndex == 0) return true;
//    return schedules.ToList()[rowIndex - 1].Selected == true ||
//        schedules.ToList()[rowIndex - 1].IsPaid == true;
//}

//selectedIndex = schedules.IndexOf(s);

//if (LoanRepaymentLines.Any(x => x.InstallmentNo == s!.InstallmentNo))
//{
//    SetDefaultValue();
//    SelectedSchedule = s;
//    return;
//}

//if (selectedIndex > 0 && !IsPreviousRowSelected(selectedIndex))
//{
//    NotificationService.Notify(NotificationSeverity.Error, "Select previous schedule to pay.");
//    SelectedSchedules = null;
//    Clear();
//    StateHasChanged();
//    return;
//}
//if (selectedIndex == 0 || IsPreviousRowSelected(selectedIndex))
//{
//    btnDisabled = false;
//    disableIsPaidNow = false;
//    schedules[selectedIndex].Selected = true;
//    SelectedSchedule = s;
//}

//if (s.IsPaid && selectedIndex != -1)
//{
//    btnDisabled = false;
//    disableIsPaidNow = false;
//    SelectedSchedule = s;
//    isPaidNow = false;
//    Clear();
//    return;
//}


//await gridSchedule.Reload();


//if (arg)
//{
//    selectIndex = index;
//}
//else
//{
//    selectIndex -= 1;
//}

//if ((index > 0 || schedules[index - 1].Selected == false) &&
//    schedules[index - 1].InstallmentNo < SelectedSchedules?[index].InstallmentNo)
//{
//    NotificationService.Notify(NotificationSeverity.Info, "Privoius Schedule not selected yet!.");

//    return;
//}
//else
//{
//    s.Selected = true;
//}

//if (index > SelectedSchedules!.Count)
//{
//    arg = false;
//    s.Selected = false;
//    NotificationService.Notify(NotificationSeverity.Info, "Privoius Schedule not selected yet!.");
//    return;
//}

//if (index > 0 && SelectedSchedules.Count > LoanRepaymentLines!.Count && !s.Selected)
//{

//    s.Selected = false;
//    NotificationService.Notify(NotificationSeverity.Info, "Privoius Schedule not in Loan Payment line yet.");
//    return;

//}

// s.Selected = arg;
// s.PenaltyAmount = 0.00M;
//gridSchedule.SelectRow(s, false);

//if (!allowRowSelected && SelectedSchedules!.Any() && !arg && s.Selected == true)
//{
//    var selectedScheduleIndex = SelectedSchedules!.IndexOf(s);

//    if (selectedScheduleIndex == SelectedSchedules.Count - 1)
//    {
//        s.Selected = arg;
//        s.PenaltyAmount = 0.00M;
//        gridSchedule.SelectRow(s, false);
//    }
//    else
//    {
//        s.Selected = true;
//        NotificationService.Notify(NotificationSeverity.Info, "Please select last schedule.");

//    }
//    StateHasChanged();
//    return;
//}

//if (!allowRowSelected && !s.Selected && arg)
//{
//    s.Selected = arg;

//    gridSchedule.SelectRow(s);

//    SetSelectedSchedule(s);

//    StateHasChanged();

//    return;
//}
//if (!allowRowSelected && s.Selected && !arg)
//{
//    s.Selected = arg;

//    gridSchedule.SelectRow(s, false);

//    var removeSchedule = LoanRepaymentLines.Where(x => x.InstallmentNo == s.InstallmentNo).FirstOrDefault();

//    if (removeSchedule is not null)
//    {
//        OnRemoveAdded(removeSchedule);
//    }

//    StateHasChanged();
//    return;
//}


//if (SelectedSchedules.Count > 1 && SelectedSchedules.First() == s)
//{
//    // Block unchecking the first item if more than 2 are selected
//    NotificationService.Notify(new NotificationMessage
//    {
//        Severity = NotificationSeverity.Error,
//        Summary = "Cannot Selected",
//        Detail = "Allow select only first schedule.",
//        Duration = 3000
//    });

//    SelectedSchedule = null;
//    SelectedSchedules.Clear();
//    return;
//}

//if (SelectedSchedules.Last() == s)
//{
//    // Allow unchecking the last item
//    SelectedSchedules.Remove(s);
//}
//else
//{
//    // Block unchecking non-last items if more than 2 are selected
//    if (SelectedSchedules.Count > 1)
//    {
//        NotificationService.Notify(new NotificationMessage
//        {
//            Severity = NotificationSeverity.Error,
//            Summary = "Cannot Uncheck",
//            Detail = "Only firest schedule selected is allowed.",
//            Duration = 3000
//        });

//        return;
//    }
//    else
//    {
//        // Allow unchecking if 2 or fewer items
//        SelectedSchedules.Remove(s);

//        await gridSchedule.Reload();

//        StateHasChanged();
//    }
//}
