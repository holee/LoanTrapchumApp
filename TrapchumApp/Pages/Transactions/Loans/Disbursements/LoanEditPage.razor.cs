using Applications.Exceptions;
using Applications.Features.Customers.Response;
using Applications.Features.Holidays.Dtos;
using Applications.Features.LoanRequests.Queryies.GetApprovedLoanRequest;
using Applications.Features.Loans.Command.ApproveDeleteReuquest;
using Applications.Features.Loans.Command.ApproveEditLoanRequest;
using Applications.Features.Loans.Command.CreateLoan;
using Applications.Features.Loans.Command.EditLoanDisbursment;
using Applications.Features.Loans.Command.RequestEditLoan;
using Applications.Features.Loans.Dtos;
using Applications.Features.Loans.Dtos.Requests;
using Applications.Features.Loans.Mappers;
using Applications.Features.Loans.Queries.GetLoanById;
using Applications.Features.Loans.Queries.GetLoanForEdit;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Domains.Models.Identity;
using Domains.Models.Loans;
using TrapchumApp.Pages.Lists.Customers;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class LoanEditPage
{
    [Parameter] public int BranchId { get; set; }
    [Parameter] public string? LoanRequestId { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public string? CustomerId { get; set; } = string.Empty;

    [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    [CascadingParameter] private CascadingAppState BState { get; set; } = default!;
    [Parameter] public string? LoanId { get; set; } = string.Empty;

    [PersistentState] public LoanDtoCreate? _model { get; set; }

    private const int FixedTabCount = 4;

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

    protected EditForm editForm = default!;

    private VLoanRequests req = new();

    private VCustomerDetails _customer = new();

    private LoanDisbursment? resource = null;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authState = await AuthenticationState;

            _userId = authState.User?.GetId();

            await GetLoanEdit();

            await BState.WhoCanDisbursemnt(req);
        }
        catch (Exception ex)
        {
            _toastService.ShowError($"An error occurred while loading the loan request due to {ex.Message}");
        }

        await base.OnInitializedAsync();
    }


    private async Task GetLoanEdit()
    {
        IsLoading = true;

        if (_model is null)
        {
            var query = new GetLoanForEditQuery(LoanId, BranchId);

            var rsp = await mediator.Send(query);

            if (rsp.IsSuccess())
            {
                _model = rsp.Value;

                ChangedSchedule(_model.RepaymentModeRefId);
            }
        }

        await GetLoan();

        IsLoading = false;
    }

    private async Task GetLoan()
    {
        var command = new GetLoanByIdQuery(LoanId, BranchId);

        var result = await mediator.Send(command);

        if (result.IsSuccess())
        {
            resource = result.Value;

            await BState.CanApproveRequestEditLoan(resource);

            await BState.IsAllowEditLoanDisbursment(resource);
        }
    }

    private async Task GetHolidays(int? installment = 12)
    {
        var yearAdded = (installment / 12) + 1;

        await _calSceduleService.GetHolidays(yearAdded);
    }

    private void OnScheduleChanged(int arg)
    {
        if (_model is not null)
        {
            _model.ScheduleMode = arg switch
            {
                1 => AdjustmentOption.Before,

                2 => AdjustmentOption.After,

                _ => AdjustmentOption.Skip
            };
        }
    }

    private void ChangedSchedule(int? arg)
    {
        if (_model is not null)
        {
            _model.ScheduleMode = arg switch
            {
                1 => AdjustmentOption.Before,

                2 => AdjustmentOption.After,

                _ => AdjustmentOption.Skip
            };
        }
    }

    private void OnStartDateChanged(DateTime? args)
    {
        if (args is null) return;

        if (_model is not null)
        {
            _model.RegisterDate = args.Value;

            int addedDays = _model.InstallmentTypeId switch
            {
                (int)InstallmentOption.Weekly => 7,

                (int)InstallmentOption.BiWeekly => 14,

                (int)InstallmentOption.Monthly => 28,

                _ => 1,
            };

            if (addedDays > 0)
            {
                _model.FirstPaymentDate = _model.StartDate!.Value.AddDays(addedDays); //args.Value.AddDays(addedDays);
            }
        }

        StateHasChanged();
    }

    private void PrepareModelDefaults()
    {
        if (_model is not null)
        {
            _model.BankRefId = 11;

            _model.TermTypeId = 11;

            _model.BranchId = BranchId;

            _model.UserRefId = _userId;

            _model.PenaltyRate = penaltyRate;

            _model.LoanStatusRefId = (int)LoanStatuses.Active;

            _model.LoanSubTypeRefId = (int)LoanSubTypeOption.Standard;

            _model.LoanProductRefId = (int)ProductTypeOption.Mortgage;
        }
    }

    private void PrintPreview()
    {
        try
        {
            if (_model is not null)
            {
                var test = _calSceduleService.Calculate(_model).MapToLoanSchedules();

                ShowPreviewTab(test, _model.CurrencyId);
            }
        }
        catch (MediatorException ex)
        {
            _toastService.ShowError($"An error occurred while calculating the loan schedule due to {ex.Message}");
        }
        catch (Exception ex)
        {
            _toastService.ShowError($"An error occurred while calculating the loan schedule due to {ex.Message}");
        }


        StateHasChanged();
    }

    private async Task HandleFormSubmit()
    {
        isWaiting = true;

        if (_model is null)
        {
            _toastService.ShowError("Loan model is not initialized.");

            isWaiting = false;

            return;
        }

        var confirmed = await MsgBox.ConfirmAsync("Do you want to disburse this loan request?", "Confirm");


        if (confirmed == MsgBoxResult.No)
        {
            _toastService.ShowError("Loan disburse was cancelled.");

            isWaiting = false;

            return;
        }

        var scheduleDto = _calSceduleService.Calculate(_model).MapToLoanSchedules();

        _model.LastPaymentDate = scheduleDto!.LastOrDefault()!.PaymentDate;

        try
        {
            editForm.EditContext?.Validate();

            _model.LoanSchedules = scheduleDto.ToList();

            var command = new EditLoanDisbursmentCommand(_model);

            var rsp = await mediator.Send(command);

            if (!rsp.IsSuccess())
            {
                //_toastService.ShowError(rsp.GetMessage());

                await Swal.ErrorAsync("Error", rsp.GetMessage());

                isWaiting = false;

                return;
            }

            _toastService.ShowSuccess(rsp.Value.Message);

            isWaiting = false;

            string loanUrl = $"/App/Loan/{rsp.Value.LoanId}/Branches/{BranchId}/Currencies/{_model.CurrencyId}";

            NavigationManager.NavigateTo(loanUrl, true);

            StateHasChanged();
        }
        catch (MediatorException ex)
        {
            _toastService.ShowError($"An error occurred while disbursing the loan request due to {ex.ErrorMessage}");

            isWaiting = false;
        }
        catch (Exception ex)
        {
            _toastService.ShowError(
                $"An error occurred while disbursing the loan request due to {ex.InnerException?.Message}");

            isWaiting = false;
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

    private async Task HandleRequestEditLoan()
    {
        var confirm = await MsgBox.ConfirmAsync("តើអ្នកពិតជាចង់ស្នើសុំកែប្រែការស្នើសុំឥណទាននេះ?", "បញ្ជាក់");

        if (confirm == MsgBoxResult.No) return;

        var command = new RequestEditLoanCommand(new RequestDeleteDtoCreate(LoanId, BranchId, memo));

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {

            _toastService.ShowSuccess("ស្នើសុំកែប្រែការស្នើសុំឥណទានបានផ្ញើរទៅអ្នកគ្រប់គ្រងដោយជោគជ័យ។");

            await GetLoan();

            StateHasChanged();
        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
        }
    }

    private async Task HandleApproveEditLoanRequest()
    {
        var confirm = await MsgBox.ConfirmAsync("តើអ្នកពិតជាចង់អនុម័តស្នើសុំកែប្រែការស្នើសុំឥណទាននេះ?", "បញ្ជាក់");

        if (confirm == MsgBoxResult.No) return;

        var command = new ApproveEditLoanRequestCommand(new RequestDeleteDtoCreate(LoanId, BranchId, memo));

        var rsp = await mediator.Send(command);

        if (rsp.IsSuccess())
        {

            _toastService.ShowSuccess("ស្នើសុំកែប្រែការស្នើសុំឥណទានត្រូវបានអនុម័តដោយជោគជ័យ។");

            await GetLoan();

            StateHasChanged();
        }
        else
        {
            _toastService.ShowError(rsp.GetMessage());
        }
    }
}