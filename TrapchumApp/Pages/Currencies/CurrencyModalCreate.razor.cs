using Applications.Features.Currencies;
using Applications.Interfaces.Currencies;
using Microsoft.AspNetCore.Components.Forms;

namespace TrapchumApp.Pages.Currencies
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