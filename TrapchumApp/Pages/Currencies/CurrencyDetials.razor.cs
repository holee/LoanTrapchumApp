using Applications.Features.Currencies;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Domains.Models.Currencies;
using Mapster;

namespace TrapchumApp.Pages.Currencies
{
    public partial class CurrencyDetials
    {
        [Inject] ICurrencyService CurrencyServiceImpl { get; set; } = default!;
        [Inject] ISpotRateService spotRateService { get; set; } = default!;

        [Parameter] public int Id { get; set; }

        private CurrencyDto Currency { get; set; } = new();

        private CurrencyDtoSpotRateCreate SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
        private List<CurrencyDtoSpotRate> currencySpotRates { get; set; } = default!;
        private PaginatedList<CurrencyDtoSpotRate> CurrencyPaginatedList { get; set; } = default!;
        private int pageIndex = 1;
        private DateTime? reqDate = DateTime.UtcNow;
        bool isActive = false;
        private CurrencyDtoSpotRate selectedSpotRate = new CurrencyDtoSpotRate();

        void Close()
        {
            NavigationManager.NavigateTo("/Account/Currencies");
        }
        protected override async Task OnInitializedAsync()
        {

            SpotRateDtoCreate.TransactionDate = DateTime.Now;

            var command = new GetCurrencyByIdQuery(Id);

            var curResult = await mediator.Send(command); //CurrencyServiceImpl.GetCurrencyById(Id);

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
                SpotRateDtoCreate = new CurrencyDtoSpotRateCreate();
                reqDate = DateTime.Now;
            }
            StateHasChanged();
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
                Page = pageIndex,
                Take = pageSize
            };
            CurrencyPaginatedList = await spotRateService.GetSpotRates(Id, qparam);
            currencySpotRates = CurrencyPaginatedList.Items;
        }




    }
}