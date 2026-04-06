using Applications.Calculations;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Branches.Dtos;
using Applications.Features.Customers.Response;
using Applications.Features.LoanRequests.Command.AddDescription;
using Applications.Features.LoanRequests.Command.RecheckedLoanRequest;
using Applications.Features.LoanRequests.Queryies.GetApprovedLoanRequest;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Command.CreateLoan;
using Applications.Features.Loans.Command.SaveLoanDisbursemenent;
using Applications.Features.Loans.Mappers;
using Applications.Features.Loans.Requests;
using Applications.Features.Loans.Services;
using Applications.Responses.ViewModels.Holidays;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Lists;
using Domains.Enumerations;
using Domains.Models.Identity;
using Domains.Models.Loans;
using Radzen.Blazor;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class LoanCreate(IHolidayServiceImpl holidayService)
{

    bool collapse = false;
    string collapseIcon = "expand_more";
    bool isVible = false;
    int selectedTabIndex = 0;
    bool isProgressing = false;
    bool isCollateralTab = false;
    bool isPreviewScheduleTab = false;
    bool IsLoading = false;
    RadzenTabs tb = default!;
    List<int> tabCounts = [0];
    IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];
    protected LoanDtoCreate model = new LoanDtoCreate();
    protected DateTime? RegisterDate = DateTime.Now;
    protected int SelectedCollectionFee = 1;
    protected decimal? TotalAdminFee = 0;
    protected int SelectedPeriodName = 1;
    protected IEnumerable<VBranch> Branches = [];

    DateTime firstPayment;

    EditForm editForm = default!;

    decimal penaltyRate = 0.0M;

    private IEnumerable<HolidayVm> holidays = [];

    private bool disburseStatus = false;

    private string memo = string.Empty;

    private int RequestStatusId = 0;

    private string? _userId = string.Empty;
    [Parameter] public int BranchId { get; set; }
    [Parameter] public string? LoanRequestId { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public string? CustomerId { get; set; } = string.Empty;
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    VLoanRequests req = new VLoanRequests();

    VCustomerDetails _customer = new VCustomerDetails();

    async Task GetLoanRequest()
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
    private async Task GetHolidayes(DateTimeOffset? date, int? installment = 12)
    {
        var yearAdded = (installment / 12) + 1;

        var selectedYear = date!.Value.DateTime;

        holidays = await holidayService.GetActiveHolidays(selectedYear, yearAdded);

    }
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

                await GetHolidayes(model.FirstPaymentDate, model.Installments);

                model.Holidays = holidays.ToList();
            }


        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }

        await base.OnInitializedAsync();
    }

    void ShowLoanPreviews()
    {
        if (model.Principle == 0) return;

        model.BankRefId = 11;

        model.TermTypeId = 11;

        model.LoanStatusRefId = (int)LoanStatuses.Active;

        model.LoanSubTypeRefId = 1;

        model.UserRefId = _userId;

        model.PenaltyRate = penaltyRate;

        previewSchedule = model.CreateSchedules().MapToLoanSchedules();

        ShowPreviewTab(previewSchedule, model.CurrencyRefId);

    }

    async Task SendRechecked()
    {

        var req = new RecheckedRequestDtoCreate(BranchId, CustomerId, LoanRequestId, DateTime.Now, RequestStatus.Rechecked, memo);

        var command = new RecheckedLoanRequestCommand(req);

        var rsp = await mediator.Send(command);

        if (!rsp.IsError)
        {

            var description = new LoanDescriptionsDtoCreate(memo, _userId, LoanRequestId);

            var cmd = new AddDescriptionCommand(description);

            await mediator.Send(cmd);

            NotificationService.Notify(NotificationSeverity.Success, "Request was sent successfully.");

            StateHasChanged();
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
        }

    }


    protected async Task HandleFormSubmit()
    {
        var confirm = await DialogService.Confirm("Do you want to disburse this loan request?", "Confirm");

        if (confirm == false)
        {
            NotificationService.Notify(NotificationSeverity.Info, "loan disburse cancel.");

            return;
        }

        model.BankRefId = 11;
        model.TermTypeId = 11;
        model.BranchId = BranchId;
        model.UserRefId = _userId;
        model.PenaltyRate = penaltyRate;
        model.LoanStatusRefId = (int)LoanStatuses.Active;
        model.LoanSubTypeRefId = (int)LoanSubTypeOption.Standard;
        model.LoanProductRefId = (int)ProductTypeOption.Mortgage;
        var scheduleDto = model.GenerateSchedules();
        var lastDate = scheduleDto!.LastOrDefault()!.PaymentDate;
        model.LastPaymentDate = lastDate;
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


    protected async Task SaveLoanDisbursement()
    {
        var confirm = await DialogService.Confirm("Do you want to save this loan request?", "Confirm");

        if (confirm == false)
        {
            NotificationService.Notify(NotificationSeverity.Info, "loan save cancel.");
            return;
        }

        var command = new SaveLoanDisbursementCommand(new SaveLoanDisbursementDtoCreate(BranchId, CustomerId, LoanRequestId, memo));

        try
        {
            editForm.EditContext!.Validate();

            var rsp = await mediator.Send(command);

            if (!rsp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());

                return;
            }

            StateHasChanged();

        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
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

}