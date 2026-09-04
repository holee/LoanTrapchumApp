using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Radzen.Blazor;

namespace Web.UI.Pages.Transactions.Exchanges;

public partial class ExchangeHistory
{
    private readonly IForiegnCurrencyExchange _exService;

    public ExchangeHistory(IForiegnCurrencyExchange exService)
    {
        _exService = exService;
    }

    [Parameter, EditorRequired] public int BranchId { get; set; }
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    private string? _userId = string.Empty;
    private IEnumerable<VttCurrencyExchanges> ExchangeHostories = [];
    private int counts = 0;
    private int pageSize = 15;
    private int pageIndex = 0;
    private bool isLoading = false;
    private bool sortDescending = false;
    private DateTime? transactionDate = null;
    string sortField = nameof(VttCurrencyExchanges.TransactionDate);
    private RadzenDataGrid<VttCurrencyExchanges> dataGrid = default!;
    protected override async Task OnInitializedAsync()
    {

        _userId = (await AuthenticationState).GetUserId();

        await GetExchangeHistories();

        await base.OnInitializedAsync();
    }
    async Task LoadData(LoadDataArgs args)
    {
        pageIndex = args.Skip ?? 0;

        (sortField, sortDescending) = args.PageInfo();

        await GetExchangeHistories();
    }

    async Task GetExchangeHistories(DateTime? traxDate = null)
    {

        Tracker.Show();

        var param = new QueryParam
        {
            Page = pageIndex,
            Take = pageSize,
            SortColumn = sortField,
            SortDescending = sortDescending
        };

        var exchangeResp = await _exService.GetCurrenciesExchange(BranchId, transactionDate, param);

        counts = exchangeResp.Count;

        ExchangeHostories = exchangeResp.Items;

        Tracker.Hide();

        StateHasChanged();
    }


    async void PageSizeChange(int page)
    {
        pageSize = page;

        pageIndex = 0;

        await GetExchangeHistories(transactionDate);
    }

    async Task CurrentDateChanged()
    {
        await GetExchangeHistories();
    }

    async Task ShowCurrencyDialogue()
    {

        var param = new Dictionary<string, object?> { { "BranchId", BranchId }, { "ShowCloseButton", true } };

        var options = new DialogOptions
        {
            Draggable = true,
            AutoFocusFirstElement = false,
            Resizable = false,
            Width = "50rem"
        };

        var dia = await DialogService.OpenAsync<MoneyExchanges>("Currency Exchange", param, options);

        if (dia is VttCurrencyExchanges result && result is not null)
        {
            await dataGrid.InsertRow(result);

            await dataGrid.Reload();
        }
    }

    async Task ShowEditCurrencyDialogue(VttCurrencyExchanges cur)
    {

        var param = new Dictionary<string, object?> { { "BranchId", BranchId }, { "TransactionId", cur.TransactionsId }, { "ShowCloseButton", true } };

        var options = new DialogOptions
        {
            Draggable = true,
            AutoFocusFirstElement = false,
            Resizable = false,
            Width = "50rem"
        };

        var dia = await DialogService.OpenAsync<MoneyExchangesEdit>("Currency Exchange", param, options);

        if (dia is VttCurrencyExchanges result && result is not null)
        {
            await dataGrid.UpdateRow(result);

            await dataGrid.Reload();
        }

    }


}