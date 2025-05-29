using Applications.Interfaces.Transactions;
using Applications.Responses.ViewModels.Currencies;
using Microsoft.AspNetCore.Components;
using Radzen;

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
        [Parameter, EditorRequired] public string? UserId { get; set; }

        IEnumerable<VttCurrencyExchanges> ExchangeHostories = [];
        int counts = 0;
        int pageSize = 10;
        int pageIndex = 1;
        bool isLoading = false;
        bool sortDescending = false;
        DateTime? transactionDate;
        protected override async Task OnInitializedAsync()
        {
            await GetExchangeHistories();

            await base.OnInitializedAsync();
        }
        async Task LoadData(LoadDataArgs args)
        {
            string sortField = "TransactionDate";
            bool sortDescending = false;
            pageIndex = args.Skip!.Value;
            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                sortField = args.OrderBy.Split(" ")[0];
                string dir = args.OrderBy.Split(" ")[1];
                sortDescending = dir.Contains("asc") ? false : true;
            }

            await GetExchangeHistories();
        }

        async Task GetExchangeHistories(DateTime? traxDate = null)
        {
            isLoading = true;

            var exchangeResp = await _exService.GetCurrencyExchange(BranchId, pageIndex, pageSize, traxDate);

            counts = exchangeResp.Count;
            ExchangeHostories = exchangeResp.Items;
            isLoading = false;

            StateHasChanged();
        }


        async void PageSizeChange(int page)
        {
            pageSize = page;
            await GetExchangeHistories(transactionDate);
        }

        async Task CurrentDateChanged(DateTime tranxDate)
        {
            await GetExchangeHistories(tranxDate);
        }

        async Task ShowCurrencyDialogue()
        {

            var param = new Dictionary<string, object?> { { "BranchId", BranchId }, { "UserId", UserId }, { "ShowCloseButton", true } };

            var options = new DialogOptions
            {
                Draggable = true,
                AutoFocusFirstElement = false,
                Resizable = false,
                Width = "50rem"
            };

            await DialogService.OpenAsync<MoneyExchanges>("Currency Exchange", param, options);
        }

    }
}