using Applications.Features.Currencies;
using Applications.Features.Currencies.Command.SetSpotRate;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Queries.GetSpotRates;
using Mapster;
using Radzen.Blazor;

namespace Web.UI.Pages.Transactions.Exchanges
{
    public partial class SpotRate
    {
        [Parameter] public int Id { get; set; }

        private CurrencyDto? Currency { get; set; } = new();

        private CurrencySpotRateDtoCreate SpotRateDtoCreate = new();

        private IEnumerable<CurrencyDtoSpotRate> currencySpotRates = [];

        IList<CurrencyDtoSpotRate>? SelectedCurrencySpotRates = null;

        private int pageIndex = 1;

        private DateTime? reqDate = DateTime.UtcNow;

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
                SpotRateDtoCreate = new();
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

            var command = new GetCurrencyByIdQuery(Id);

            var curResult = await mediator.Send(command);  //CurrencyServiceImpl.GetCurrencyById(Id);

            if (curResult.IsSuccess())
            {
                Currency = curResult.Value.Adapt<CurrencyDto>();
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
                SpotRateDtoCreate = new();

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

                var message = await mediator.Send(new SetSpotRateCommand(Id, SpotRateDtoCreate));

                if (message.IsSuccess)
                {

                    try
                    {
                        await ChangePageIndex(pageIndex);

                        SpotRateDtoCreate = new();
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
        }


        private async Task ChangePageIndex(int pageIndex, int pageSize = 10)
        {
            var qparam = new QueryParam
            {
                Page = pageIndex,
                Take = pageSize
            };

            var rsp = await mediator.Send(new GetSpotRatesQuery(Id, qparam));

            currencySpotRates = rsp.Items;
        }


    }
}