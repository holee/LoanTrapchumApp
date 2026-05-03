using Applications.Calculations;
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

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class LoanCreatePage(IHolidayServiceImpl holidayService)
{
    private const int FixedTabCount = 3;

    private int selectedTabIndex;
    private bool IsLoading;
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
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
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
        int addedDays = model.InstallmentTypeId switch
        {
            (int)InstallmentOption.Weekly => 7,
            (int)InstallmentOption.BiWeekly => 14,
            (int)InstallmentOption.Monthly => 28,
            _ => 1,
        };

        if (args.HasValue)
        {
            model.FirstPaymentDate = args.Value.AddDays(addedDays);
        }
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

    private void ShowLoanPreviews()
    {
        if (model.Principle == 0) return;

        PrepareModelDefaults();

        var schedules = model.CreateSchedules().MapToLoanSchedules();
        ShowPreviewTab(schedules, model.CurrencyRefId);
    }

    private async Task<bool> ConfirmActionAsync(string message)
    {
        var confirmed = await DialogService.Confirm(message, "Confirm");
        return confirmed == true;
    }

    private async Task HandleFormSubmit()
    {
        if (!await ConfirmActionAsync("Do you want to disburse this loan request?"))
        {
            NotificationService.Notify(NotificationSeverity.Info, "Loan disburse was cancelled.");
            return;
        }

        PrepareModelDefaults();

        var scheduleDto = model.GenerateSchedules();

        model.LastPaymentDate = scheduleDto!.LastOrDefault()!.PaymentDate;

        try
        {
            editForm.EditContext!.Validate();

            model.LoanId = $"dis-{Guid.CreateVersion7()}";

            model.LoanSchedules = scheduleDto.ToList();

            var command = new CreateLoanCommand(model);

            var rsp = await mediator.Send(command);

            if (!rsp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());

                return;
            }

            disburseStatus = rsp.Value.Success;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }

    private async Task SaveLoanDisbursement()
    {
        if (!await ConfirmActionAsync("Do you want to save this loan request?"))
        {
            NotificationService.Notify(NotificationSeverity.Info, "Loan save was cancelled.");

            return;
        }


        var mod = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Approved);

        mod.Remark = this.model.Remark;

        var command = new SaveLoanRequestsCommand(mod);

        try
        {
            editForm.EditContext!.Validate();

            var rsp = await mediator.Send(command);

            if (!rsp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());

                return;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message!);
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }

    private async Task SendRechecked()
    {

        var mod = new SaveLoanRequestDtoCreate(BranchId, CustomerId, LoanRequestId, memo, RequestStatus.Rechecked);

        mod.Remark = this.model.Remark;

        var command = new SaveLoanRequestsCommand(mod);

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {

            NotificationService.Notify(NotificationSeverity.Success, rsp.Value.Message!);

            StateHasChanged();
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
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
            builder.OpenComponent(0, typeof(LoanSchedules));
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