using AppilcationServices.Interfaces;
using ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Components;
using TCLMS.Shared.Entities.Dtos.Lists.Currency;
using TCLMS.Shared.Entities.Dtos.Loans;
using TCLMS.Shared.Entities.ViewModels.Currencies;

namespace TCLMS.Front.Pages.Transactions.Loans.LoanProducts
{
    public partial class LoanProductModalCreate
    {
        [Inject] private ILoanProductService? Service { get; set; }

        protected LoanProductDTOCreate model =new LoanProductDTOCreate();

        protected IEnumerable<CurrencyDto> _currencies = [];

        protected bool IsActive = false;

        private bool isValid = true;
        protected override async Task OnInitializedAsync()
        {
            var currenciesRsponse = await _currencyService.GetCurrencies(0);

            if (currenciesRsponse.IsSuccess)
            {
                _currencies = currenciesRsponse.Data!;
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

            }catch (Exception ex)
            {
                NotificationService.Notify(Radzen.NotificationSeverity.Error, ex.Message);
            }
        }
    }
}