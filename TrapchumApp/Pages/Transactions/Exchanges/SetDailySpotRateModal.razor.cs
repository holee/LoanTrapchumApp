using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Domains.Models.Currencies;
using Mapster;

namespace TrapchumApp.Pages.Transactions.Exchanges
{
    public partial class SetDailySpotRateModal
    {
        private readonly ILogger<SetDailySpotRateModal> _logger;

        private readonly ISpotRateService _spotRateService;

        public SetDailySpotRateModal(ILogger<SetDailySpotRateModal> logger, ISpotRateService spotRateService)
        {
            _logger = logger;
            _spotRateService = spotRateService;
        }

        private IEnumerable<VCurrenciesList>? Currencies = new List<VCurrenciesList>();

        private List<CurrencyDtoCreateSpotRate> model = new List<CurrencyDtoCreateSpotRate>();

        private int pageIndex = 1;

        private int pageSize = 10;

        private bool includeInactive = false;

        private DateTime reqDate = DateTime.UtcNow;

        private bool isValid = true;

        protected override async Task OnInitializedAsync()
        {

            _logger.LogInformation("Getting...");

            try
            {
                var result = await _spotRateService.GetCurrenciesForSetSpotRate();

                Currencies = result.Any() ? result : [];

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
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
            var intervalDay = DateTime.UtcNow.Subtract(reqDate).Days;
            if (intervalDay != 0)
            {
                return;
            }

            var req = model.Adapt<List<CurrencySpotRate>>();

            var setSpotRate = await _spotRateService.SetSpotRates(req);

            if (setSpotRate.IsSuccess)
            {

            }
            else
            {

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