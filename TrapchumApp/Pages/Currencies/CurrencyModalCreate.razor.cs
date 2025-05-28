using Applications.Interfaces.Currencies;
using Applications.Requests.Lists.Currency;
using Microsoft.AspNetCore.Components.Forms;

namespace TrapchumApp.Components.Pages.Currencies
{
    public partial class CurrencyModalCreate
    {
        private readonly ICurrencyService curService;

        public CurrencyModalCreate(ICurrencyService currencyService)
        {
            curService = currencyService;
        }

        private CurrencyDtoCreate model = new CurrencyDtoCreate();
        private EditForm? editForm;


      
    }
}