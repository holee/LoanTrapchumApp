using Applications.Features.Currencies.Dto;
using Applications.Interfaces.Currencies;

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