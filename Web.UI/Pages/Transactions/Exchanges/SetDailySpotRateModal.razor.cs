using Applications.Features.Currencies.Command.SetSpotRates;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrenciesForSetSpotRates;
using Applications.Features.Currencies.Responses;

namespace Web.UI.Pages.Transactions.Exchanges
{
    public partial class SetDailySpotRateModal(ILogger<SetDailySpotRateModal> logger)
    {
        private IEnumerable<VCurrenciesList>? Currencies = new List<VCurrenciesList>();

        private List<CurrencySpotRateDtoCreate> model = new List<CurrencySpotRateDtoCreate>();

        private int pageIndex = 0;

        private int pageSize = 15;

        private bool includeInactive = false;

        private DateTime reqDate = DateTime.UtcNow;

        protected override async Task OnInitializedAsync()
        {

            logger.LogInformation("Getting...");

            try
            {

                var result = await mediator.Send(new GetCurrenciesForSetSpotRatesQuery(includeInactive, new QueryParam(pageIndex, pageSize)));

                Currencies = result.Items;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
            await base.OnInitializedAsync();
        }


        async Task SetSpotRate()
        {
            model = Currencies!.Select(x => new CurrencySpotRateDtoCreate
            {
                CurrencyRefId = x.CurrencyId,
                SpotRate = x.SpotRate,
                BuyingRate = x.BuyingRate,
                SellingRate = x.SellingRate,
                TransactionDate = reqDate
            }).ToList();

            var intervalDay = DateTime.UtcNow.Subtract(reqDate).Days;

            if (intervalDay != 0)
            {
                return;
            }

            var setSpotRate = await mediator.Send(new SetSpotRatesCommand(model));

            if (setSpotRate.IsSuccess)
            {
                _toastService.ShowSuccess("Spot Rate has been set successfully.", "Success");
            }
            else
            {
                _toastService.ShowError(setSpotRate.GetMessage(), "Error");
            }

        }

        string selectedTab = "profile";

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            return Task.CompletedTask;
        }
    }
}