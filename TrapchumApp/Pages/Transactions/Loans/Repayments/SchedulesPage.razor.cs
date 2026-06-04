using Applications.Calculations;
using Applications.Features.Branches.Dtos;
using Applications.Features.LoanPayments.Requests;
using Applications.Responses.ViewModels.Loans.Schedules;
using Radzen.Blazor;
using TrapchumApp.Pages.Transactions.RecievePayements;
using Applications.Calculations.Extensions;
using Applications.Commons.Mappers;
using Applications.Extensions.NumberExtensions;
using Applications.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Services;
using Applications.Services.Loans;
using Applications.Features.Loans.Dtos.Responses;
namespace TrapchumApp.Pages.Transactions.Loans.Repayments;

public partial class SchedulesPage
{

    [Parameter] public string? LoanId { get; set; }
    [Parameter] public int BranchId { get; set; }
    [Parameter] public string? CreditAgentId { get; set; }
    [Parameter] public int CurrencyId { get; set; }
    [Parameter] public EventCallback OnMultiplePayment { get; set; }
    [Parameter] public EventCallback OnClosed { get; set; } 

    private IEnumerable<VSchedule> schedules = [];

    private readonly ILoanServiceImpl _loanService;

    private readonly ILoanScheduleServiceImpl _scheduleService;

    public SchedulesPage(ILoanServiceImpl loanService, ILoanScheduleServiceImpl loanScheduleService)
    {
        _loanService = loanService;

        _scheduleService = loanScheduleService;

    }


    private string? _userId = string.Empty;
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    DateTime paymentDate = DateTime.Now;

    List<LoanPaymentLineDtoCreate> LoanPaymentLines = [];

    LoanPaymentDtoCreate LoanRepayment = new();

    IEnumerable<VBranch> Branches = [];

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

    void OnCellContextMenu(DataGridCellMouseEventArgs<VSchedule> args)
    {

        if (args == null || args.Data == null)
            return;

        ContextMenuService.Open(args,
            new List<ContextMenuItem> {
                new ContextMenuItem()
                    {
                      Text = "Recieve Payment",
                      Value = 1,
                      Icon = "paid",
                      Disabled=SetDisabled(args.Data)
                    },
                new ContextMenuItem(){
                    Text = "Show Details",
                    Value = 2,
                    Icon = "details"
                },
            },
            (e) =>
            {
                ContextMenuService.Close();

                var selected = (int)e?.Value!;

                if (selected == 1)
                {
                    _ = PayBySchedules(args.Data);
                }
                else if (selected == 2)
                {
                    _ = ShowScheduleDetails(args.Data);
                }

            }
         );
    }


    async Task ShowScheduleDetails(VSchedule? p)
    {
        //await popup.ToggleAsync(buttonItem.Element);
        ContextMenuService.Close();

        var param = new Dictionary<string, object?>
            {
                {"LoanId",p.LoanId },
                {"CurrencyId",p.CurrencyId },
                {"BranchId",p.BranchId },
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


    async Task ApplySchedulePayment()
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

        var options = new SideDialogOptions
        {
            Resizable = false,
            Width = "35rem",
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

        var parameters = new Dictionary<string, object?>{
                {"CurrencyId" , CurrencyId},
                {"LoanPaymentCreate",loanPayment },
            };

        var message = $"Recieve Payment from {LoanSchedule!.AltName}";

        bool? confirm = await DialogService.OpenSideAsync<PaymentSideBars>(message, parameters!, options);

        if (confirm is not null && confirm == true)
        {
            NotificationService.Notify(NotificationSeverity.Success, "Receive successfully.");

            await GetSchedulesByLoanIdAsync(ScheduleStatus.Unpaid);

            LoanPaymentLines.Clear();

            _loanPaymentLine = new();

            disableIsPaidNow = true;

            isPaidNow = false;

        }

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


        //AddMultiplePaymentTab(_selectedSchedules, loanPayment);

        //var options = new SideDialogOptions
        //{
        //    Width = "50vw",

        //    CloseDialogOnOverlayClick = true,

        //    Position = DialogPosition.Right
        //};

        //var result = await DialogService.OpenSideAsync<PaymentMultiples>("Payment", parameters, options);

        //if (result is bool dialogueResult && dialogueResult)
        //{
        //   // SelectedSchedules!.Clear();

        //    await GetSchedulesByLoanIdAsync(ScheduleStatus.Unpaid);

        //    await gridSchedule.Reload();
        //}
        //else
        //{
        //    //SelectedSchedules!.Clear();

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

        var query = new GetLoanByIdQuery(loanId, BranchId, ScheduleStatus.Unpaid, true);

        var rsp = await mediator.Send(query);

        if (!rsp.IsSuccess() && rsp.Value is null)
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
            SelectedSchedule = _selectedSchedules.First();

            disableIsPaidNow = false;
        }

        isLoading = false;


    }

    async Task GetSchedulesByLoanIdAsync(ScheduleStatus status)
    {
        isLoading = true;

        schedules = [];

        _selectedSchedules = [];

        var query = new GetLoanSchedulesByLoanIdQuery(LoanId, BranchId, status);

        var rsp = await mediator.Send(query);


        if (rsp.IsSuccess())
        {
            schedules = rsp.Value;

            _selectedSchedules = schedules.Take(1).ToList();
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
        }

        isLoading = false;


        StateHasChanged();


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


    void ShowPaidSchedules(VSchedule? p)
    {
        ContextMenuService.Close();

        //_selectedTabIndex = 1;
    }
    async Task PayBySchedules(VSchedule p)
    {
        ContextMenuService.Close();

        var options = new SideDialogOptions
        {
            Position = DialogPosition.Right,
            ShowMask = true,
            CloseDialogOnOverlayClick = false,
            Width = "35rem"
        };

        var param = new Dictionary<string, object?>
            {
                {"LoanId",p.LoanId },
                {"CurrencyId",p.CurrencyId },
                {"BranchId",p.BranchId },
                {"LoanScheduleId",p.LoanScheduleId },
            };

        await DialogService.OpenSideAsync<RecievePaymentSidebar>($"Schedule-{p.LoanScheduleId}", parameters: param!, options);

    }


    private bool SetDisabled(VSchedule schedule)
    {
        return (IsAlreadyPaidSchedule(schedule) || !IsFirstScheduleSelected(schedule));
    }

    private bool IsAlreadyPaidSchedule(VSchedule p)
    {
        if (p.IsPaid)
        {
            NotificationService.Notify(NotificationSeverity.Error, "This schedule is already paid.");

            return true;
        }

        return false;
    }

    private bool IsFirstScheduleSelected(VSchedule p)
    {
        var schedulesList = schedules?.ToList();

        var firstSchedule = schedulesList?.First(x => x.IsPaid == false);

        if (p != firstSchedule)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Cannot add new schedule due to previouse schedule not complete paid yet.");

            return false;
        }

        return true;
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

//async Task CloseTab()
//{

//    if (isAllowMultiplePayment && _isMultiplePaymentTab)
//    {
//        _isPaymentTab = false;

//        _selectedIndexTab = 1;
//    }
//    if (!isAllowMultiplePayment && _isPaymentTab)
//    {
//        _isPaymentTab = false;

//        _selectedIndexTab = 0;

//        isPaidNow = false;
//    }

//    _showExchanged = false;

//    await GetLoanWithSchedules(BranchId, LoanId);

//    _loanPaymentLine = new();
//}


//void ClosedTab()
//{

//    if ((isAllowMultiplePayment && _isMultiplePaymentTab) ||
//        (isAllowMultiplePayment && !_isPaymentTab))
//    {
//        _isMultiplePaymentTab = false;

//        _isPaymentTab = false;

//        _selectedIndexTab = 0;
//    }

//    _selectedSchedules?.Clear();

//}


//void CreatePayment(LoanPaymentDtoCreate? arg)
//{
//    if (arg is null) return;

//    {
//        _isPaymentTab = true;

//        _selectedIndexTab = 2;

//        _loanPayment = arg;
//    }

//}

