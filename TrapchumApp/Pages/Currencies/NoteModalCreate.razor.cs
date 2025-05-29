using Applications.Extensions.mappers;
using Applications.Interfaces.Currencies;
using Applications.Requests.Lists.Currency;
using Applications.Responses.Lists.Currency;
using Domains.Models.Currencies;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace TrapchumApp.Pages.Currencies
{
    public partial class NoteModalCreate
    {
        private readonly ICurrencyService _currencyService;

        public NoteModalCreate(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [Parameter] public int Id { get; set; }
        private bool isValid = true;

        CurrencyNoteDtoCreate model = new CurrencyNoteDtoCreate();
        EditForm? editForm;
        IEnumerable<CurrencyResponse> Currencies = new List<CurrencyResponse>();
        void Close() => DialogService.Close(false);
        protected override async Task OnInitializedAsync()
        {
            model.CurrencyRefId = Id == 0 ? 1 : Id;
            model.Quantity = 0;
            var currenciesResponse = await _currencyService.GetCurrencies(0);
            if (currenciesResponse.IsSuccess)
            {
                Currencies = currenciesResponse.Data!.Maps<CurrencyResponse>();
            }

            await base.OnInitializedAsync();
        }

        async Task FormSubmit()
        {

            editForm?.EditContext?.Validate();

            var req = model.Adapt<CurrencyCounter>();


            var response = await _currencyService.AddNote(req);

            if (response.IsSuccess)
            {
                Close();
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Not Success.");
            }
        }
    }
}