using Applications.Interfaces.Currencies;
using Applications.Interfaces.Transactions;
using Applications.Requests.Lists.Currency;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Currencies;
using Mapster;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Exchanges
{
    public partial class SetDailySpotRate
    {
        private readonly ICurrencyService currencyService;
        private readonly ISpotRateService spotRateService;
        private IEnumerable<VCurrenciesList>? Currencies = new List<VCurrenciesList>();
        private List<CurrencyDtoCreateSpotRate> model = new List<CurrencyDtoCreateSpotRate>();
        private int pageIndex = 1;
        private int pageSize = 10;
        private bool includeInactive = false;
        private DateTime reqDate = DateTime.Now;
        private bool isValid = true;

        public SetDailySpotRate(ICurrencyService currencyService, ISpotRateService spotRateService)
        {
            this.currencyService = currencyService;
            this.spotRateService = spotRateService;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var spotRateResult = await spotRateService.GetCurrenciesForSetSpotRate();

                if (spotRateResult.Any())
                {
                    Currencies = spotRateResult;
                }
                else
                {
                    Currencies = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            await base.OnInitializedAsync();
        }


        async Task SetSpotRate()
        {
            model = Currencies!.Select(x => new CurrencyDtoCreateSpotRate
            {
                CurrencyRefId = x.CurrencyId,
                SpotRate = x.SpotRate,
                BuyingRate = x.BuyingRate,
                SellingRate = x.SellingRate,
                TransactionDate = reqDate
            }).ToList();

            var requestNo = DateTime.Now.Subtract(reqDate).Days;

            if (requestNo != 0)
            {
                NotificationService.Notify(NotificationSeverity.Success, @"Date Provided is not valid date.The Provide date must be current Date Set Daily SPot Rate");
                return;
            }

            var req = model.Adapt<List<CurrencySpotRate>>();

            var setSpotRateResponse = await spotRateService.SetSpotRates(req);

            if (setSpotRateResponse.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, setSpotRateResponse.Data);

                NavigationManager.NavigateTo("/Account/Currencies");
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Success, setSpotRateResponse.ErrMessage);
            }

        }

        void CheckSellingRate(FocusEventArgs e)
        {
            for (int x = 0; x < Currencies!.Count(); x++)
            {
                if (Currencies!.ToList()[x].SellingRate < Currencies!.ToList()[x].BuyingRate
                || Currencies!.ToList()[x].SellingRate < Currencies!.ToList()[x].SpotRate)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Selling Rate must greater than others");
                    Currencies!.ToList()[x].SellingRate = 0;
                    break;
                }
            }
        }
    }
}