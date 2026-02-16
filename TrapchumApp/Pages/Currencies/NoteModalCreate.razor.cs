using Applications.Extensions.mappers;
using Applications.Features.Currencies.Command.AddCurrencyNote;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Responses.Paginations;
using Domains.Models.Currencies;
using Mapster;
using TrapchumApp.Helpers.Extensions;

namespace TrapchumApp.Pages.Currencies
{
    public partial class NoteModalCreate
    {
        [Parameter] public int Id { get; set; }
        
        private bool isValid = true;
        CurrencyNoteDtoCreate model = new CurrencyNoteDtoCreate();
        EditForm? editForm;
        IEnumerable<CurrencyDto> Currencies = new List<CurrencyDto>();
        void Close() => DialogService.Close(false);
        protected override async Task OnInitializedAsync()
        {
            model.CurrencyRefId = Id == 0 ? 1 : Id;

            model.Quantity = 0;

            var command = new GetCurrenciesQuery(0, new QueryParam(0, 0));

            var currenciesResponse = await mediator.Send(command);   //_currencyService.GetCurrencies();

            if (currenciesResponse.IsSuccess)
            {
                Currencies = currenciesResponse.Items.Maps<CurrencyDto>();
            }

            await base.OnInitializedAsync();
        }

        async Task FormSubmit()
        {

            editForm?.EditContext?.Validate();

            var req = model.Adapt<CurrencyCounter>();

            var command = new AddCurrencyNoteCommand(model);

            var response = await mediator.Send(command);  //_currencyService.AddNote(req);

            if (response.IsSuccess())
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