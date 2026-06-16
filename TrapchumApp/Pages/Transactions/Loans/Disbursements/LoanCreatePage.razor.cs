using Applications.Calculations;
using Applications.Commons.Extensions;
using Applications.Exceptions;
using Applications.Extensions;
using Applications.Features.Customers.Response;
using Applications.Features.Holidays.Dtos;
using Applications.Features.LoanRequests.Command.SaveLoanRequests;
using Applications.Features.LoanRequests.Queryies.GetApprovedLoanRequest;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Command.CreateLoan;
using Applications.Features.Loans.Dtos.Requests;
using Applications.Features.Loans.Mappers;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Lists;
using Domains.Enumerations;
using Domains.Models.Identity;
using TrapchumApp.Pages.Lists.Customers;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class LoanCreatePage(IHolidayServiceImpl holidayService)
{
    private const int FixedTabCount = 3;

    private int selectedTabIndex;

    private bool IsLoading;

    private bool isWaiting = false;

    private bool disburseStatus;

    private int RequestStatusId;

    private string memo = string.Empty;

    private string? _userId = string.Empty;

    private decimal penaltyRate;

    private IEnumerable<HolidayDto> holidays = [];

    private List<CustomTab> _tabs = [];

    protected LoanDtoCreate model = new();

    protected EditForm editForm = default!;

    private VLoanRequests req = new();

    private VCustomerDetails _customer = new();

    [Parameter] public int BranchId { get; set; }
    [Parameter] public string? LoanRequestId { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public string? CustomerId { get; set; } = string.Empty;
    [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [CascadingParameter] private CascadingAppState BState { get; set; } = default!;
    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authState = await AuthenticationState;

            _userId = authState.User?.GetId();

            if (CustomerId.IsNotNullOrEmpty() && LoanRequestId.IsNotNullOrEmpty())
            {
                await GetLoanRequest();

                model = req.ToLoanDtoCreate();

                model.PayementSechuleModeId = 2;

                model.ScheduleMode = AdjustmentOption.After;

                await GetHolidays(model.FirstPaymentDate, model.Installments);

                model.Holidays = holidays.ToList();
            }

            await BState.WhoCanDisbursemnt(req);

        }
        catch (Exception ex)
        {
            _toastService.ShowError($"An error occurred while loading the loan request due to {ex.Message}");
        }

        await base.OnInitializedAsync();
    }

    private async Task GetLoanRequest()
    {
        IsLoading = true;

        var query = new GetApprovedLoanRequestQuery(BranchId, CustomerId, LoanRequestId, true);

        var request = await mediator.Send(query);

        if (!request.IsError)
        {
            req = request.Value;

            RequestStatusId = request.Value.StatusId!.Value;

            _customer = request.Value.Customer;
        }

        IsLoading = false;
    }

    private async Task GetHolidays(DateTimeOffset? date, int? installment = 12)
    {
        var yearAdded = (installment / 12) + 1;

        var selectedYear = date!.Value.DateTime;

        holidays = await holidayService.GetActiveHolidays(selectedYear, yearAdded);
    }

    private void OnScheduleChanged(int arg)
    {
        model.ScheduleMode = arg switch
        {
            1 => AdjustmentOption.Before,

            2 => AdjustmentOption.After,

            _ => AdjustmentOption.Skip
        };
    }

    private void OnStartDateChanged(DateTime? args)
    {
        if (args is null) return;

        model.RegisterDate = args.Value;

        int addedDays = model.InstallmentTypeId switch
        {
            (int)InstallmentOption.Weekly => 7,

            (int)InstallmentOption.BiWeekly => 14,

            (int)InstallmentOption.Monthly => 28,

            _ => 1,
        };

        if (addedDays > 0)
        {
            model.FirstPaymentDate = model.StartDate!.Value.AddDays(addedDays);    //args.Value.AddDays(addedDays);
        }

        StateHasChanged();
    }

    private void PrepareModelDefaults()
    {
        model.BankRefId = 11;

        model.TermTypeId = 11;

        model.BranchId = BranchId;

        model.UserRefId = _userId;

        model.PenaltyRate = penaltyRate;

        model.LoanStatusRefId = (int)LoanStatuses.Active;

        model.LoanSubTypeRefId = (int)LoanSubTypeOption.Standard;

        model.LoanProductRefId = (int)ProductTypeOption.Mortgage;
    }

    private void PrintPreview()
    {

        var test = _calSceduleService.Calculate(model).MapToLoanSchedules();

        ShowPreviewTab(test, model.CurrencyRefId);

        StateHasChanged();
    }

    private async Task HandleFormSubmit()
    {

        isWaiting = true;

        var confirmed = await MsgBox.ConfirmAsync("Do you want to disburse this loan request?", "Confirm");


        if (confirmed == MsgBoxResult.No)
        {
            _toastService.ShowError("Loan disburse was cancelled.");

            isWaiting = false;

            return;
        }

        PrepareModelDefaults();

        var scheduleDto = _calSceduleService.Calculate(model).MapToLoanSchedules();

        //model.GenerateSchedules();

        model.LastPaymentDate = scheduleDto!.LastOrDefault()!.PaymentDate;

        try
        {
            editForm.EditContext?.Validate();

            model.LoanId = $"dis-{Guid.CreateVersion7()}";

            model.LoanSchedules = scheduleDto.ToList();

            var command = new CreateLoanCommand(model);

            var rsp = await mediator.Send(command);

            if (!rsp.IsSuccess())
            {

                _toastService.ShowError(rsp.GetMessage());


                isWaiting = false;

                return;
            }

            _toastService.ShowSuccess(rsp.Value.Message);

            isWaiting = false;

            string loanUrl = $"/App/Loan/{rsp.Value.LoanId}/Branches/{BranchId}/Currencies/{model.CurrencyRefId}";

            NavigationManager.NavigateTo(loanUrl);

            StateHasChanged();
        }
        catch (Exception ex)
        {
            _toastService.ShowError($"An error occurred while disbursing the loan request due to {ex.Message}");

            isWaiting = false;
        }
    }

    private async Task SaveLoanDisbursement()
    {

        var confirm = await MsgBox.ConfirmAsync("Do you want to save this loan request?", "Confirm");

        if (confirm == MsgBoxResult.No) return;

        var mod = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Approved);

        mod.Remark = model.Remark;

        var command = new SaveLoanRequestsCommand(mod);

        try
        {
            editForm.EditContext!.Validate();

            var rsp = await mediator.Send(command);

            if (!rsp.IsSuccess())
            {

                _toastService.ShowError(rsp.GetMessage());

                return;
            }
            else
            {

                _toastService.ShowSuccess(rsp.Value.Message);
            }

            StateHasChanged();
        }
        catch (MediatorException ex)
        {
            _toastService.ShowError($"An error occurred while saving the loan request due to {ex.Message}");
        }
        catch (Exception ex)
        {
            _toastService.ShowError($"An error occurred while saving the loan request due to {ex.Message}.");
        }
    }

    private async Task SendRechecked()
    {

        var comfirm = await MsgBox.ConfirmAsync("Do you want to send this loan request for rechecking?");

        if (comfirm == MsgBoxResult.No) return;

        var mod = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Rechecked);

        mod.Remark = this.model.Remark;

        var command = new SaveLoanRequestsCommand(mod);

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {

            _toastService.ShowSuccess($"{rsp.Value.Message}");

            disburseStatus = rsp.Value.Success;

            StateHasChanged();
        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
        }
    }

    private void ShowCustomerTab()
    {
        var tabId = "customer_" + (_customer.CustomerId ?? Guid.NewGuid().ToString("N"));

        var exists = _tabs.FindIndex(t => t.Key == tabId);

        if (exists != -1)
        {
            selectedTabIndex = exists + FixedTabCount;

            StateHasChanged();

            return;
        }

        RenderFragment content = builder =>
        {
            builder.OpenComponent(0, typeof(CustomerInfo));

            builder.AddAttribute(1, "Customer", _customer);

            builder.SetKey(tabId);

            builder.CloseComponent();
        };

        _tabs.Add(new CustomTab(tabId, "Customer Info", TabActions.Details, content));

        selectedTabIndex = _tabs.Count + FixedTabCount - 1;
    }

    private void ShowPreviewTab(IEnumerable<LoanScheduleDtoCreate> schedules, int? currencyId = 1)
    {
        var tabId = "preview_" + (_customer.CustomerId ?? Guid.NewGuid().ToString("N"));

        var exists = _tabs.FindIndex(t => t.Key == tabId);

        if (exists != -1)
        {
            selectedTabIndex = exists + FixedTabCount;

            StateHasChanged();

            return;
        }

        RenderFragment content = builder =>
        {
            builder.OpenComponent(0, typeof(TabPreviewSchedules));

            builder.AddAttribute(1, "CurrencyId", currencyId);

            builder.AddAttribute(2, "Schedules", schedules);

            builder.SetKey(tabId);

            builder.CloseComponent();
        };

        _tabs.Add(new CustomTab(tabId, "Schedules", TabActions.Details, content));

        selectedTabIndex = _tabs.Count + FixedTabCount - 1;
    }

    private void CloseTab(string tabId)
    {
        var tabIndex = _tabs.FindIndex(t => t.Key == tabId);

        if (tabIndex != -1)
        {
            _tabs.RemoveAt(tabIndex);
        }

        selectedTabIndex = _tabs.Count + FixedTabCount - 1;
    }
}