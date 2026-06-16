using Applications.Features.Branches.Dtos;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Loans.Queries.GetLoanByBranchId;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Loans.Disbursements;

public partial class TabAllLoans
{
    [Parameter] public int BranchId { get; set; } = 0;
    [Parameter] public string? UserId { get; set; }
    [Parameter] public EventCallback<int> GetLoanChanged { get; set; }
    [Parameter] public EventCallback<List<LoanCurrencySummary>> SummaryChanged { get; set; }
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [CascadingParameter] CascadingAppState bState { get; set; } = default!;

    private List<VLoanDisbursments> LoanItems = [];

    private IList<VLoanDisbursments> SelectedLoans = [];

    private string? filter = string.Empty;

    protected IEnumerable<VBranch> Branches = [];

    int counts = 0;

    int pageSize = 15;

    bool isLoading = false;

    bool isProgess = false;

    bool includeInactive = false;

    string sortField = "RegisterDate";

    bool sortDescending = true;

    int pageIndex = 0;

    private DateTime? _from = null;

    private DateTime? _to = null;
    async Task ChangedPageSize(int page)
    {
        pageSize = page;
        pageIndex = 0;
        await GetLoanDisbursements();
    }
    async void TextLoanSearchChanged(string filterText)
    {
        filter = filterText;
        await GetLoanDisbursements();
    }
    async void CheckedIncludeChanged()
    {
        await GetLoanDisbursements();
    }

    protected override async Task OnInitializedAsync()
    {

        var auth = await AuthenticationState;

        //await bState.CheckedIsCreditAgent(auth.User);

        await bState.GetAuthenticatedUser(auth.User);

        await GetLoanDisbursements();

        await GetLoanCount();

        await base.OnInitializedAsync();
    }

    private async Task LoadData(LoadDataArgs args)
    {
        pageIndex = args.Skip ?? 0;

        (sortField, sortDescending) = args.PageInfo();

        await GetLoanDisbursements();
    }

    private async Task GetLoanDisbursements(DateTime? from = null, DateTime? to = null)
    {

        isLoading = true;

        var param = new QueryParam(pageIndex, pageSize, sortField, false, sortDescending, filter);

        var userId = bState.IsCreditAgent ? UserId : string.Empty;

        var query = new GetLoanByBranchIdQuery(BranchId, userId, LoanStatuses.Active, from, to, param);

        var res = await mediator.Send(query); 

        LoanItems = res.Items;

        counts = res.Count;

        isLoading = false;

        await InvokeSummaryChanged();

        StateHasChanged();

    }

    private async Task InvokeSummaryChanged()
    {
        var summaries = LoanItems
            .GroupBy(x => new { x.CurrencyId, x.CurrencyName })
            .Select(g => new LoanCurrencySummary(
                g.Key.CurrencyId,
                g.Key.CurrencyName ?? string.Empty,
                g.Count(),
                g.Sum(x => x.Principle ?? 0m),
                g.Sum(x => x.PaidPrinciple ?? 0m),
                g.Sum(x => x.OutStanding ?? 0m)
            ))
            .ToList();

        await SummaryChanged.InvokeAsync(summaries);
    }


    private async Task DateFromChanged(DateTime? from)
    {
        if (from.HasValue)
        {
            await GetLoanDisbursements(from, _to);
        }
        else
        {
            await GetLoanDisbursements(null, _to);
        }
    }


    private async Task DateToChanged(DateTime? to)
    {
        if (to.HasValue)
        {
            await GetLoanDisbursements(_from, to);
        }
        else
        {
            await GetLoanDisbursements(_from, null);
        }
    }

    async Task GetLoanCount()
    {
        await GetLoanChanged.InvokeAsync(counts);
    }

}



//async Task DownLoadScheduleAsExcel(string? loanRefId, int? branchId)
//{
//    ContextMenuService.Close();

//    if (loanRefId.IsNotNullOrEmpty() && branchId > 0)
//    {
//        await js.InvokeVoidAsync("open", $"/Reports/Schedules/{loanRefId}/{branchId}/Exel", "_blank");
//    }
//}

//async Task DownLoadScheduleAsPdf(string? loanRefId, int? branchId)
//{

//    ContextMenuService.Close();

//    if (loanRefId.IsNotNullOrEmpty() && BranchId > 0)
//    {
//        await js.InvokeVoidAsync("open", $"/Reports/Schedules/{loanRefId}/{branchId}", "_blank");
//    }
//}
//async void ShowPaidSchedules(long loanId, int branchId)
//{
//    ContextMenuService.Close();

//    var response = await _dService.GetPaymentSchedulesByLoanId(loanId, branchId);

//    if (response.IsSuccess)
//    {
//        PaidSchedules = response.Data!;
//    }
//    else
//    {

//        PaidSchedules = [];

//        var error = response.Errors!.FirstOrDefault();

//        NotificationService.Notify(NotificationSeverity.Error, error);
//    }
//    StateHasChanged();
//}


//void GetLoanDetails(VLoanDisbursments p)
//{
//    NavigationManager.NavigateTo($"/App/Loan/{p.LoanId}/Branches/{BranchId}/Currencies/{p.CurrencyId}");
//}
//void GetPaymentSchedules(VLoanDisbursments p)
//{
//    NavigationManager.NavigateTo($"/App/Loans/{p.BranchId}/Branches/{p.LoanId}/Schedules?CreditAgentId={p.CreditAgentRefId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
//}

//void ChangeCreditAgent(VLoanDisbursments p)
//{
//    NavigationManager.NavigateTo($"/App/Loan/{p.LoanId}/CreditAgent/{p.CreditAgentRefId}?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
//}
//void CustomerPayOff(VLoanDisbursments p)
//{
//    NavigationManager.NavigateTo($"/Loan/CreditAgent/{p.LoanId}/{p.CreditAgentRefId}?CustomerId={p.CustomerId}&BranchId={p.BranchId}&CurrencyId={p.CurrencyId}&UserId={UserId}");
//    return;
//}