using Applications.Extensions.mappers;
using Applications.Interfaces.Currencies;
using Applications.Requests.Loans.Products;
using Applications.Responses.Lists.Currency;
using Applications.Services.Loans;

namespace TrapchumApp.Components.Pages.Transactions.Loans.LoanProducts
{
    public partial class LoanProductModalCreate
    {
        private readonly ILoanProductServiceImpl _loanProductService;
        private readonly ICurrencyService _currencyService;

        public LoanProductModalCreate(ICurrencyService currencyService, ILoanProductServiceImpl loanProductService)
        {
            _currencyService = currencyService;
            _loanProductService = loanProductService;
        }

        protected LoanProductDtoCreate model = new LoanProductDtoCreate();

        protected IEnumerable<CurrencyResponse> _currencies = [];

        protected bool IsActive = false;

        private bool isValid = true;
        protected override async Task OnInitializedAsync()
        {
            var currenciesRsponse = await _currencyService.GetCurrencies(0);

            if (currenciesRsponse.IsSuccess)
            {
                _currencies = currenciesRsponse.Data!.Maps<CurrencyResponse>();
            }
            await base.OnInitializedAsync();
        }
        protected async Task OnSaveNew()
        {
            try
            {
                if (model.CurrencyRefId is null)
                    model.CurrencyRefId = 2;

                var result = await _loanProductService.CreateAsync(model);

                if (result.IsSuccess)
                {
                    //Toaster.Success("Loan Product created successfully!", "Creating", config =>
                    //{
                    //    config.ShowProgressBar = false;
                    //    config.ShowCloseIcon = true;
                    //});

                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
            }
        }
    }
}