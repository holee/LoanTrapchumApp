using Applications.Enumerations;
using Applications.Extensions.NumberExtions;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Payments;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Currencies;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;
using TCLMS.Front.Pages.Transactions.Loans;
using TrapchumApp.Components.Pages.Transactions.Loans;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments;

public partial class ReceiveRepayment
{

    private readonly ILoanServiceImpl _loanService;
    private readonly ILoanScheduleService _scheduleService;
    private readonly ISchedulesServiceImpl _schedulesServiceImpl;
    public ReceiveRepayment(ILoanServiceImpl loanService, ILoanScheduleService loanScheduleService, ISchedulesServiceImpl schedulesServiceImpl)
    {
        _loanService = loanService;
        _scheduleService = loanScheduleService;
        _schedulesServiceImpl = schedulesServiceImpl;
    }

    [Parameter] public Guid LoanId { get; set; }
    [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
    [Parameter, SupplyParameterFromQuery] public long CreditAgentId { get; set; }
    [Parameter, SupplyParameterFromQuery] public int CurrencyId { get; set; }
    [Parameter, SupplyParameterFromQuery] public long UserId { get; set; }
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    DateTime paymentDate = DateTime.UtcNow;
    List<VSchedule> schedules { get; set; } = [];
    List<LoanRepaymentDto> RepaymentLists = [];
    List<LoanRepaymentLineDtoCreate> loanRepaymentLines = [];
    LoanRepaymentDtoCreate LoanRepayment = new();
    IEnumerable<VBranch> Branches = [];
    VSchedule? SelectedSchedule = new();
    List<VSchedule>? selectedRepayments = [];
    VLoanDisbursments? LoanSchedule = new();
    IEnumerable<VCurrenciesList> Currencies = Enumerable.Empty<VCurrenciesList>();
    VCurrenciesList? selectedCurrency = new();
    RadzenDataGrid<VSchedule> gridSchedule = default!;
    PaymentLines paymentLinesRef = default!;
    IEnumerable<VLoanRepaymentLine> VLoanRepaymentLines = [];
    IList<VSchedule>? SelectedSchedules = [];
    VLoanDisbursments? vLoan = new();
    DataGridSelectionMode gridSelectionMode = DataGridSelectionMode.Single;
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
    bool isAllowMultiplePayment = false;
    bool allowRowSelected = true;
    bool disableIsPaidNow = true;
    bool btnDisabled = false;
    string receiptNo = string.Empty;
    string recieptNumber = "";
    int pageIndex = 1;
    int pageSize = 25;
    string sortField = "LoanId";
    string filter = string.Empty;
    protected override async Task OnInitializedAsync()
    {
        btnDisabled = true;

        await GetLoanWithSchedules(BranchId, LoanId);

        await base.OnInitializedAsync();
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
            loanRepaymentLines.Clear();

            var line = SelectedSchedule?.ToPaymentLine(recievePayment);

            if (line is not null)
            {
                loanRepaymentLines.Add(line);
            }
        }

        if (loanRepaymentLines.Count <= 0)
        {
            NotificationService.Notify(NotificationSeverity.Error, "There is any schedule selected yet!");

            return;
        }

        PaymentCreate create = loanRepaymentLines.ToPaymentCreate(CurrencyId);

        var options = new DialogOptions
        {
            Resizable = false,
            Draggable = true,
            CssClass = "rz-p-1 rz-m-0 rz-background-color-base-200"
        };

        var parameters = new Dictionary<string, object?>{
                {"LoanRepaymentLines" , loanRepaymentLines},
                {"CurrencyId" , CurrencyId},
                {"PaymentCreate" ,create},
                {"LoanId" , LoanId},
                {"CreditAgentId" , CreditAgentId},
                {"BranchId",BranchId },
                {"ReceiptNo",receiptNo },
                {"CustomerName",LoanSchedule!.AltName},
                {"UserId",UserId}
            };

        var message = $"Recieve Payment from {LoanSchedule!.AltName}";

        bool? confirm = await DialogService.OpenAsync<PaymentModal>(message, parameters, options);

        if (confirm is not null && confirm == true)
        {
            NotificationService.Notify(NotificationSeverity.Success, "Receive successfully.");

            await GetCustomerSchedulesAsync(ScheduleStatus.Unpaid);

            loanRepaymentLines.Clear();

            await gridSchedule.Reload();

            Reset();
        }
    }
    void CheckPaymentChanged(bool arg)
    {
        decimal? total = SelectedSchedule.SubTotal();

        if (selectedIndex > 0 && arg)
        {
            var previousSchedule = schedules[selectedIndex - 1];

            if (!previousSchedule.Selected && !previousSchedule.IsPaid)
            {
                NotificationService.Notify(NotificationSeverity.Warning, "Cannot add new schedule due to previouse schedule not complete paid yet.");
                SetDefaultValue();
                return;
            }
        }

        if (arg && total > 0)
        {
            recievePayment = SelectedSchedule!.ToPaymentDtoCreate();

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
        SelectedSchedules = [];
        SelectedSchedule = new();
        isPaidNow = false;
        btnDisabled = true;
        DeSelectedSchedule();
        loanRepaymentLines.Clear();
        Clear();
        await gridSchedule.Reload();
    }
    void CheckedChanged(bool arg, VSchedule s)
    {
        var index = schedules.IndexOf(s);

        if (!isAllowMultiplePayment) return;

        if (index > SelectedSchedules!.Count)
        {
            arg = false;
            s.Selected = false;
            NotificationService.Notify(NotificationSeverity.Info, "Privoius Schedule not selected yet!.");
            return;
        }

        if (index > 0 && SelectedSchedules.Count > loanRepaymentLines!.Count && !s.Selected)
        {

            s.Selected = false;
            NotificationService.Notify(NotificationSeverity.Info, "Privoius Schedule not in Loan Payment line yet.");
            return;

        }

        if (!allowRowSelected && SelectedSchedules.Any() && !arg && s.Selected == true)
        {
            var selectedScheduleIndex = SelectedSchedules.IndexOf(s);

            if (selectedScheduleIndex == SelectedSchedules.Count - 1)
            {
                s.Selected = arg;
                s.PenaltyAmount = 0.00M;
                gridSchedule.SelectRow(s, false);
            }
            else
            {
                s.Selected = true;
                NotificationService.Notify(NotificationSeverity.Info, "Please select last schedule.");

            }
            StateHasChanged();
            return;
        }

        if (!allowRowSelected && !s.Selected && arg)
        {
            s.Selected = arg;
            gridSchedule.SelectRow(s);
            SetSelectedSchedule(s);
            StateHasChanged();
            return;
        }
        if (!allowRowSelected && s.Selected && !arg)
        {
            s.Selected = arg;
            gridSchedule.SelectRow(s, false);
            var removeSchedule = loanRepaymentLines.Where(x => x.InstallmentNo == s.InstallmentNo).FirstOrDefault();
            if (removeSchedule is not null)
            {
                OnRemoveAdded(removeSchedule);
            }
            StateHasChanged();
            return;
        }



    }

    async Task GetLoanWithSchedules(int? branchId, Guid loanId)
    {

        isLoading = true;

        var responseSchedule = await _loanService.GetLoanByIdAsync(loanId, BranchId, ScheduleStatus.Unpaid);

        if (!responseSchedule.IsSuccess && responseSchedule.Data is null)
        {
            LoanSchedule = null;
            schedules = [];
            SelectedSchedules = [];
            isLoading = false;
            return;
        }

        LoanSchedule = responseSchedule.Data;

        if (LoanSchedule is null)
        {
            schedules = [];
            SelectedSchedules = [];
            isLoading = false;
            return;
        }

        schedules = LoanSchedule.Schedules!.ToList();

        SelectedSchedules = schedules.Take(1).ToList();

        if (SelectedSchedules.Any() && !isAllowMultiplePayment)
        {
            SetSelectedSchedule(SelectedSchedules.FirstOrDefault());
        }

        isLoading = false;


    }
    async Task GetCustomerSchedulesAsync(ScheduleStatus status)
    {
        try
        {
            isLoading = true;

            schedules = [];

            SelectedSchedules = [];

            var param = new QueryParam(pageIndex, pageSize, sortField, filter);

            var responseSchedule = await _scheduleService.GetLoanScheduleByIdAsync(LoanId, BranchId, status, param);


            schedules = responseSchedule.Items;

            SelectedSchedules = schedules.Take(1).ToList();

            if (SelectedSchedules.Any() && !isAllowMultiplePayment)
            {
                SetSelectedSchedule(SelectedSchedules.FirstOrDefault());
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
    bool IsPreviousRowSelected(int rowIndex)
    {
        if (rowIndex == 0) return true;
        return schedules[rowIndex - 1].Selected == true || schedules[rowIndex - 1].IsPaid == true;
    }
    void SelectedRowEvt(VSchedule s)
    {

        selectedIndex = schedules.IndexOf(s);

        if (isAllowMultiplePayment)
        {
            return;
        }

        if (loanRepaymentLines.Any(x => x.InstallmentNo == s!.InstallmentNo))
        {
            SetDefaultValue();
            SelectedSchedule = s;
            return;
        }

        if (selectedIndex > 0 && !IsPreviousRowSelected(selectedIndex))
        {
            NotificationService.Notify(NotificationSeverity.Error, "Select previous schedule to pay.");
            SelectedSchedules = null;
            Clear();
            StateHasChanged();
            return;
        }
        if (selectedIndex == 0 || IsPreviousRowSelected(selectedIndex))
        {
            btnDisabled = false;
            disableIsPaidNow = false;
            schedules[selectedIndex].Selected = true;
            SelectedSchedule = s;
        }

        if (s.IsPaid && selectedIndex != -1)
        {
            btnDisabled = false;
            disableIsPaidNow = false;
            SelectedSchedule = s;
            isPaidNow = false;
            Clear();
            return;
        }



    }
    void SetSelectedSchedule(VSchedule? s)
    {
        if (s is not null)
        {
            SelectedSchedule = s;
            SelectedSchedule.Selected = true;
            disableIsPaidNow = false;
        }
        else
        {
            SelectedSchedule = null;
        }
    }
    void DeSelectedRowEvt(VSchedule s)
    {
        if (!isAllowMultiplePayment)
        {
            btnDisabled = false;
            SelectedSchedules = null;
            Clear();
            s.Selected = false;
        }
    }
    void DeSelectedSchedule()
    {
        schedules.ForEach(x => x.Selected = false);
    }

    async void OnPaid()
    {
        if (SelectedSchedule is not null && loanRepaymentLines.Any(x => x.InstallmentNo == SelectedSchedule!.InstallmentNo))
        {
            SetDefaultValue();
            return;
        }
        if (SelectedSchedule is not null && recievePayment is not null)
        {

            var confirm = await DialogService.Confirm("Are you sure want add this payment?", "Confirm");

            if (confirm == false) return;

            var line = SelectedSchedule.ToPaymentLine(recievePayment);

            schedules[selectedIndex].Selected = true;

            loanRepaymentLines.Add(line);

            await paymentLinesRef.ReLoad();

            Clear();

            btnDisabled = true;
        }
        StateHasChanged();
    }

    void Reset()
    {
        DeSelectedSchedule();
        Clear();
        loanRepaymentLines.Clear();
        SelectedSchedules = [];
        SelectedSchedule = new();
        disableIsPaidNow = true;

    }

    void OnRemoveAdded(LoanRepaymentLineDtoCreate value)
    {
        var removeIndex = loanRepaymentLines.IndexOf(value);

        if (removeIndex < loanRepaymentLines.Count - 1)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Cannot remove first schedule.");
            return;
        }

        if (removeIndex == loanRepaymentLines.Count - 1)
        {
            loanRepaymentLines.RemoveAt(removeIndex);

            var changedSchedule = SelectedSchedules!.Where(x => x.InstallmentNo == value.InstallmentNo).FirstOrDefault();

            if (changedSchedule is not null)
            {
                var scheduleIndex = SelectedSchedules!.IndexOf(changedSchedule);

                SelectedSchedules[scheduleIndex].Selected = false;

                gridSchedule.SelectRow(changedSchedule, false);

            }

            totalPaidAmount = loanRepaymentLines.Total();

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

}