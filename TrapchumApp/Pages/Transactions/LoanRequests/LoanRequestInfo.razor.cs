using Applications.Commons.Mappers;
using Applications.Extensions;
using Applications.Extensions.Maths;
using Applications.Features.Customers.Queries.GetCustomerAsPaging;
using Applications.Features.Customers.Response;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.LoanPurposes;
using Applications.Features.LoanPurposes.Queries.GetLoanPurposes;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Dtos.Requests;
using Applications.Features.Periods.Dto;
using Applications.Features.Periods.Queries.GetPeriods;
using Applications.Features.Settings.Dto;
using Applications.Features.Settings.Queries.GetApprovedUsers;
using Applications.Features.Settings.Queries.GetLoanCategories;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Responses.ViewModels.Options;
using Applications.Services.Loans;

namespace TrapchumApp.Pages.Transactions.LoanRequests;

public partial class LoanRequestInfo(ILogger<LoanRequestCreate> logger)
{
    private readonly ILogger<LoanRequestCreate> _logger = logger;
    [Parameter] public LoanRequestDtoCreate Model { get; set; } = new();
    [Parameter] public string? UserId { get; set; }
    [Parameter] public int? BranchId { get; set; } = 0;
    [Parameter] public string? CustomerId { get; set; }
    [Parameter] public EventCallback<string?> SelectCustomer { get; set; }
    [Parameter] public string? LoanRequestId { get; set; }

    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

    private IEnumerable<LoanScheduleDtoCreate> previewSchedule = [];

    private IEnumerable<VUserLoanSizes> AprovedUsers = [];
    private string? defaultUserId;
    private IEnumerable<VCustomerList> Customers = [];

    private IEnumerable<LoanRepaymentModeDto>? LoanRepaymentModes = [];

    private IEnumerable<LoanScheduleStatusDto>? LoanScheduleStatus = [];

    private IEnumerable<LoanStatusDto>? LoanStatuses = [];

    private IEnumerable<LoanCategoryDto>? loanCategories = [];

    private IEnumerable<VCustomerDetails>? CustomersList = [];

    private IEnumerable<LoanInstallmentTypeDto>? LoanInstallments = [];

    private IEnumerable<PeriodDto>? _periods = [];

    private List<VLoanRequestGuarantors> Guarantors = [];

    private List<VLoanRequestCollateralLines> CollateralLines = [];

    private List<VLoanRequestsDocuments> Documents = [];

    private IEnumerable<LoanRequestDocumentsDtoCreate> _documents = [];

    private VCustomerDetails? selectedCustomer = new();

    private DateTime firstPayment;

    private LoanCategoryDto? SelectedLoanCategory;
    long? size = 0;
    int counts = 0;
    bool isVible = false;
    bool isCustomerSelected = false;
    bool isLoading = false;
    bool isProgressing = false;
    int SelectedCollectionFee = 1;
    decimal? TotalAdminFee = 0;
    int SelectedPeriodName = 1;
    int installmentType = 1;
    int selectedTabIndex = 0;
    string? filterText = "";
    private List<LoanPurposeDto> _loanPurposes = [];
    protected override async Task OnInitializedAsync()
    {


        await GetApprovedUser(BranchId, null, null);

        await GetLoanPurposes();

        await GetLoanCategories();

        await GetPeriods();

        if (string.IsNullOrEmpty(Model.AprovedUserId))
        {
            Model.AprovedUserId = defaultUserId;
        }

        if (CustomerId.IsNotNullOrEmpty() || Model.CustomerRefId.IsNotNullOrEmpty())
        {
            Model.CustomerRefId = CustomerId;

            await GetCustomers(CustomerId);

            Model.FullName = Customers.FirstOrDefault(x => x.CustomerId == CustomerId)?.AltName ?? string.Empty;
        }


        await base.OnInitializedAsync();
    }

    private async Task GetLoanCategories()
    {
        var cat = await mediator.Send(new GetLoanCategoriesQuery(null, null)); //loanSettingService.GetLoanCategories();

        if (cat.Items.Count > 0)
        {
            loanCategories = cat.Items;

            SelectedLoanCategory = loanCategories!.FirstOrDefault();
        }
    }


    private async Task GetPeriods()
    {
        var command = new GetPeriodsQuery(new QueryParam(0, 0, string.Empty, filter: filterText));

        var rsp = await mediator.Send(command);

        _periods = rsp.Items;

    }

    async Task GetLoanPurposes()
    {
        var command = new GetLoanPurposesQuery(new QueryParam(0, 0, string.Empty, filter: filterText));

        var rsp = await mediator.Send(command);

        _loanPurposes = rsp.Items;

    }



    protected override async Task OnParametersSetAsync()
    {
        if (LoanRequestId.IsNotNullOrEmpty())
        {
            await GetApprovedUser(BranchId, Model.CurrencyRefId, Model.RequestAmount);
        }
        await base.OnParametersSetAsync();
    }


    private async Task GetApprovedUser(int? branchId, int? currencyId, decimal? amount)
    {

        var command = new GetApprovedUserQuery(branchId, currencyId, amount, null);

        var rsp = await mediator.Send(command);

        AprovedUsers = rsp.Value ?? [];

        defaultUserId = AprovedUsers
                            .Where(x => x.IsDefault == true)
                            .Select(x => x.UserRefId)
                            .FirstOrDefault();

    }

    private async Task GetCustomers(string? customerId, int take = 25, string? filter = null)
    {
        var queryParam = new QueryParam(0, take);

        var command = new GetCustomerAsPagingQuery(null, null, customerId, queryParam);

        var rsp = await mediator.Send(command);

        Customers = rsp.Items;

    }

    void ChangedDate(DateTime? date)
    {
        if (installmentType == 1)
        {
            firstPayment = date!.Value.AddMonths(1);
        }

        if (installmentType == 2)
        {
            firstPayment = date!.Value.AddDays(14);
        }

        if (installmentType == 3)
        {
            firstPayment = date!.Value.AddDays(7);
        }

        if (installmentType == 4)
        {
            firstPayment = date!.Value.AddDays(1);
        }
    }

    void CalculateAdminFeeAmount(decimal? value)
    {
        if (Model.CurrencyRefId == 1)
        {
            Model.AdminFeeAmount = (Model.RequestAmount * (value / 100)).RoundUpKhr(2);
        }
        else
        {
            Model.AdminFeeAmount = (Model.RequestAmount * (value / 100)).RoundUp(2);
        }
    }

    async Task PrincipleValueChanged(decimal? principle)
    {
        if (!principle.HasValue || principle < 0)
        {
            NotificationService.Notify(NotificationSeverity.Warning, "Invalid principle input");

            Model.RequestAmount = 0.00M;

            return;
        }

        if (principle <= 50000)
        {
            Model.CurrencyRefId = 2;

            Model.AdminFeeAmount = (principle * (Model.AdminFeeRate / 100)).RoundUp(2);

        }
        else
        {
            Model.CurrencyRefId = 1;

            Model.AdminFeeAmount = (principle * (Model.AdminFeeRate / 100)).RoundUp(-2);
        }

        StateHasChanged();
    }

    void SelectedInstallmentChanged(object? arg)
    {
        if (arg is not null)
        {
            var installType = (Items)arg;
            installmentType = installType.Value;
            Model.InstallmentType = installType.Label;
        }
    }

    void CurrencySelectedChanged(object e)
    {
        var selectedValue = (Items)e;
        StateHasChanged();
    }

    void SelectedChargeModeChanged(object? e)
    {
        if (e is not null)
        {
            var selectedValue = (Items)e;

            Model.CollectionFeeModeName = selectedValue.Label;
        }
        else
        {
            Model.CollectionFeeModeId = 1;

            Model.CollectionFeeModeName = "Flat";
        }
    }

    void RepaymentModeSelectedChanged(object e)
    {
        var selectedValue = (Items)e;

        Model.RepaymentMode = selectedValue.Label;

        if (selectedValue.Value == 1)
        {
            Model.PayEvery = 4;
        }
        else
        {
            Model.PayEvery = 1;
        }
    }
}
