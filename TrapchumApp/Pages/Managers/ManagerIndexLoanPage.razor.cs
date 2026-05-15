using Applications.Features.Branches.Dtos;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Features.Loans.Queries.GetLoanByBranchId;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Managers;

public partial class ManagerIndexLoanPage
{
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    [CascadingParameter] CascadingAppState bState { get; set; } = default!;
    [Parameter] public int BranchId { get; set; } = 0;

    private int loanCount = 0;

    void GetLoanCount(int count)
    {
        loanCount = count;
    }

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
        Tracker.Show();

        var param = new QueryParam(pageIndex, pageSize, sortField, false, sortDescending, filter);

        var query = new GetLoanByBranchIdQuery(0, null, LoanStatuses.Active, from, to, param);

        var res = await mediator.Send(query);

        LoanItems = res.Items;

        counts = res.Count;

        Tracker.Hide();

        StateHasChanged();

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
        //await GetLoanChanged.InvokeAsync(counts);
    }
}
