using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses.Paginations;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Transactions.Exchanges
{
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
        private int pageSize = 10;
        private int pageIndex = 1;
        private bool isLoading = false;
        private bool sortDescending = false;
        private DateTime? transactionDate = DateTime.UtcNow;
        string sortField = "TransactionDate";

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
            isLoading = true;

            var exchangeResp = await _exService.GetCurrenciesExchange(BranchId, null, new QueryParam());

            counts = exchangeResp.Count;

            ExchangeHostories = exchangeResp.Items;

            isLoading = false;

            StateHasChanged();
        }


        async void PageSizeChange(int page)
        {
            pageSize = page;

            pageIndex = 0;

            await GetExchangeHistories(transactionDate);
        }

        async Task CurrentDateChanged(DateTime tranxDate)
        {
            await GetExchangeHistories(tranxDate);
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

            await DialogService.OpenAsync<MoneyExchanges>("Currency Exchange", param, options);
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

            await DialogService.OpenAsync<MoneyExchangesEdit>("Currency Exchange", param, options);
        }


    }
}