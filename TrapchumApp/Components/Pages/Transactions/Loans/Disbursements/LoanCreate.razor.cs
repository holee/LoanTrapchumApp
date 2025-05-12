using Applications.Calculations;
using Applications.Extensions;
using Applications.Requests.Loans.Disbursements;
using Applications.Responses.ViewModels.Branches;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Services.Loans;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Loans.Disbursements;

public partial class LoanCreate
{
    private readonly ILoanServiceImpl _loanService;

    public LoanCreate(ILoanServiceImpl loanService)
    {
        _loanService = loanService;
    }

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
    [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }
    [Parameter, SupplyParameterFromQuery] public string? CustomerRequestId { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public string? LoanRequestId { get; set; } = string.Empty;

    VLoanRequests req = new VLoanRequests();
    DateTime firstPayment;
    EditForm editForm = default!;
    decimal penaltyRate = 0.0M;
    List<DateTimeOffset> Holidays = [];

    async Task GetLoanRequest()
    {
        IsLoading = true;

        var request = await _loanRequestService.GetLoanRequestAsync(BranchId, CustomerRequestId, LoanRequestId, true);

        if (request.IsSuccess)
        {
            req = request.Data!;
        }

        IsLoading = false;
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (CustomerRequestId.IsNotNullOrEmpty() && LoanRequestId.IsNotNullOrEmpty())
            {
                await GetLoanRequest();

                model = req.ToLoanDtoCreate();
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

        model.LoanStatusRefId = 4;

        model.LoanSubTypeRefId = 1;

        model.UserRefId = UserId;

        model.PenaltyRate = penaltyRate;

        var param = model.ToLoanInfo();

        var builder = Calculators.CreateBuilder(model);
        {
            var app = builder.Build(param);

            previewSchedule = app.SetHoliday(Holidays)
                .CreateSchedules(model)
                .MapTo(model);
            selectedTabIndex = 2;
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
        model.LoanStatusRefId = 4;
        model.LoanSubTypeRefId = 1;
        model.BranchId = BranchId;
        model.UserRefId = UserId;
        model.PenaltyRate = penaltyRate;

        var param = model.ToLoanInfo();

        var builder = Calculators.CreateBuilder(model); //.CreateBuilder(model.RepaymentModeRefId, model.CurrencyRefId);

        var app = builder.Build(model);

        var scheduleDto = app.SetHoliday(Holidays)
            .CreateSchedules(model)
            .MapTo(model);

        var lastDate = scheduleDto!.LastOrDefault()!.PaymentDate;

        model.LastPaymentDate = lastDate;

        try
        {
            editForm.EditContext!.Validate();

            model.LoanId = $"dis-{Guid.CreateVersion7()}";

            model.LoanSchedules = scheduleDto.ToList();

            var rsp = await _loanService.CreateLoanAsync(model);

            if (!rsp.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.ErrMessage);
                return;
            }

            NotificationService.Notify(NotificationSeverity.Success, rsp.Data);
            
            
            
            
            
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }
}