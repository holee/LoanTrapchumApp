using Applications.Calculations;
using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.Customers.Response;
using Applications.Features.LoanRequests.Queryies.GetApprovedLoanRequest;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Mappers;
using Applications.Features.Loans.Requests;
using Applications.Features.Loans.Services;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Holidays;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Lists;
using Domains.Enumerations;
using Radzen.Blazor;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class LoanCreate(ILoanServiceImpl loanService, IHolidayServiceImpl holidayService)
{
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
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
    protected IEnumerable<VBranchResponse> Branches = [];

    DateTime firstPayment;

    EditForm editForm = default!;

    decimal penaltyRate = 0.0M;

    IEnumerable<HolidayVm> holidays = [];

    bool disburseStatus = false;

    private string memo = string.Empty;

    int RequestStatusId = 0;

    [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? CustomerRequestId { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public string? LoanRequestId { get; set; } = string.Empty;

    VLoanRequests req = new VLoanRequests();

    VCustomerDetails _customer = new VCustomerDetails();

    async Task GetLoanRequest()
    {
        IsLoading = true;

        var query = new GetApprovedLoanRequestQuery(BranchId, CustomerRequestId, LoanRequestId, true);

        var request = await mediator.Send(query);// _loanRequestService.GetApprovedLoanRequestAsync(BranchId, CustomerRequestId, LoanRequestId, true);

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

            if (CustomerRequestId.IsNotNullOrEmpty() && LoanRequestId.IsNotNullOrEmpty())
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

        model.UserRefId = UserId;

        model.PenaltyRate = penaltyRate;

        previewSchedule = model.CreateSchedules().MapToLoanSchedules();

        selectedTabIndex = 3;

    }

    async Task SendRechecked()
    {

        var req = new RecheckedRequestDtoCreate(BranchId, CustomerRequestId, LoanRequestId, DateTime.Now, RequestStatus.Rechecked, memo);

        var rsp = await _loanRequestService.RecheckedLoanRequestAsync(req);

        if (rsp.IsSuccess)
        {
            RequestStatusId = rsp.Data!.StatusId!.Value;

            var description = new LoanDescriptionsDtoCreate(memo, UserId, LoanRequestId);

            await _loanRequestService.AddDescriptionAsync(description);

            NotificationService.Notify(NotificationSeverity.Success, "Request was sent successfully.");

            StateHasChanged();
        }
        else
        {
            NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
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
        model.LoanStatusRefId = (int)LoanStatuses.Active;
        model.LoanSubTypeRefId = 1;
        model.BranchId = BranchId;
        model.UserRefId = UserId;
        model.PenaltyRate = penaltyRate;

        var scheduleDto = model.GenerateSchedules();  //.CreateSchedules().MapToLoanSchedules();

        var lastDate = scheduleDto!.LastOrDefault()!.PaymentDate;

        model.LastPaymentDate = lastDate;

        try
        {
            editForm.EditContext!.Validate();

            model.LoanId = $"dis-{Guid.CreateVersion7()}";

            model.LoanSchedules = scheduleDto.ToList();

            var rsp = await loanService.CreateLoanAsync(model);

            if (!rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);

                return;
            }

            disburseStatus = rsp.Data;

            StateHasChanged();

        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }
}