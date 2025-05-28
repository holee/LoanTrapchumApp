using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Applications.Requests.Lists.Currency;
using Applications.Responses.Lists.Currency;
using Applications.Responses.Paginations;
using Domains.Models.Currencies;
using Mapster;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace TrapchumApp.Components.Pages.Transactions.Exchanges
{
    public partial class SpotRate
    {
        [Inject] ICurrencyService CurrencyServiceImpl { get; set; } = default!;
        [Inject] ISpotRateService spotRateService { get; set; } = default!;
        private CurrencyResponse? Currency { get; set; } = new();
        private CurrencyDtoSpotRateCreate SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
        private List<CurrencyDtoSpotRate> currencySpotRates { get; set; } = default!;
        private PaginatedList<CurrencyDtoSpotRate> CurrencyPaginatedList { get; set; } = default!;
        IList<CurrencyDtoSpotRate>? SelectedCurrencySpotRates = null;
        private int pageIndex = 1;
        private DateTime? reqDate = DateTime.UtcNow;
        [Parameter] public int Id { get; set; }
        bool isActive = false;
        private CurrencyDtoSpotRate selectedSpotRate = new CurrencyDtoSpotRate();

        RadzenDataGrid<CurrencyDtoSpotRate> gridRef = default!;

        void SelectedRowChanged(CurrencyDtoSpotRate e)
        {
            if (e is not null)
            {
                selectedSpotRate = e;
                reqDate = e.TransactionDate;
                SpotRateDtoCreate.SpotRate = e.SpotRate;
                SpotRateDtoCreate.BuyingRate = e.BuyingRate;
                SpotRateDtoCreate.SellingRate = e.SellingRate;
                SpotRateDtoCreate.TransactionDate = reqDate;
                SpotRateDtoCreate.EditSeq = e.EditSeq;
            }
            else
            {
                SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
                reqDate = DateTime.Now;
            }
        }


        void DisplayCurrencyDialogue()
        {
            //var resp = await _client.GetAsync("https://www.nbc.gov.kh/english/economic_research/exchange_rate.php");
            //Console.WriteLine(resp.StatusCode);

        }

        Task DisplayCurrencyNoteDialogue()
        {
            return Task.CompletedTask;
        }


        void Close()
        {
            NavigationManager.NavigateTo("/App/Currencies");
        }
        protected override async Task OnInitializedAsync()
        {
            SpotRateDtoCreate.TransactionDate = DateTime.Now;
            var curResult = await CurrencyServiceImpl.GetCurrencyById(Id);
            if (curResult.IsSuccess)
            {
                Currency = curResult.Adapt<CurrencyResponse>();
            }
            await ChangePageIndex(pageIndex);
            await base.OnInitializedAsync();
        }

        private void SelectedRow(CurrencyDtoSpotRate param)
        {
            selectedSpotRate = param;
            if (param is not null)
            {
                reqDate = param.TransactionDate;
                SpotRateDtoCreate.SpotRate = param.SpotRate;
                SpotRateDtoCreate.BuyingRate = param.BuyingRate;
                SpotRateDtoCreate.SellingRate = param.SellingRate;
                SpotRateDtoCreate.TransactionDate = reqDate;
                SpotRateDtoCreate.EditSeq = param.EditSeq;
            }
            else
            {
                SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
                reqDate = DateTime.Now;
            }
            StateHasChanged();
        }

        async void SelectedValueChanged()
        {

            await Task.CompletedTask;
            //var result = await spotRateService.GetSpotRate(Id, reqDate!.Value);

            //if (result.IsSuccess)
            //{
            //    SpotRateDtoCreate.SpotRate = result.Data!.SpotRate;
            //    SpotRateDtoCreate.BuyingRate = result.Data.BuyingRate;
            //    SpotRateDtoCreate.SellingRate = result.Data.SellingRate;
            //    SpotRateDtoCreate.TransactionDate = result.Data.TransactionDate;
            //    SpotRateDtoCreate.EditSeq = result.Data.EditSeq;
            //    var selectedSpotRate = result.Data.Adapt<CurrencyDtoSpotRate>();
            //    SelectedCurrencySpotRates = currencySpotRates.Where(x => x.TransactionDate == result.Data.TransactionDate)
            //                                                 .Take(1).ToList();
            //}
            //else
            //{
            //    SelectedCurrencySpotRates = null;
            //    SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
            //    SpotRateDtoCreate.TransactionDate = reqDate;
            //}

            //StateHasChanged();
        }

        async Task SelectPageChanged(int page)
        {
            try
            {
                pageIndex = page;
                await ChangePageIndex(page);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            StateHasChanged();
        }

        async Task FormSubmit()
        {
            try
            {

                var req = SpotRateDtoCreate.Adapt<CurrencySpotRate>();

                var message = await spotRateService.SetSpotRate(Id, req);

                if (message.IsSuccess)
                {

                    try
                    {
                        await ChangePageIndex(pageIndex);
                        SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //StateHasChanged();
        }


        private async Task ChangePageIndex(int pageIndex, int pageSize = 10)
        {
            var qparam = new QueryParam
            {
                Skip = pageIndex,
                PageSize = pageSize
            };
            CurrencyPaginatedList = await spotRateService.GetSpotRates(Id, qparam);
            currencySpotRates = CurrencyPaginatedList.Items;
        }


    }
}