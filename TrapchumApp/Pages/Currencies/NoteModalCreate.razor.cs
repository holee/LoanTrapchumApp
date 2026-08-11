using Applications.Exceptions;
using Applications.Features.Currencies.Command.AddCurrencyNote;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;

namespace TrapchumApp.Pages.Currencies
{
    public partial class NoteModalCreate
    {
        [Parameter] public int Id { get; set; }

        CurrencyNoteDtoCreate model = new CurrencyNoteDtoCreate();

        //EditForm? editForm;

        IEnumerable<CurrencyDto> Currencies = new List<CurrencyDto>();
        void Close() => DialogService.Close(false);
        protected override async Task OnInitializedAsync()
        {
            model.CurrencyRefId = Id == 0 ? 1 : Id;

            model.Quantity = 0;

            var command = new GetCurrenciesDtoQuery(0, new QueryParam(0, 0));

            var cur = await mediator.Send(command);

            Currencies = cur.Items;

            await base.OnInitializedAsync();
        }

        async Task FormSubmit()
        {
            try
            {
                var command = new AddCurrencyNoteCommand(model);

                var response = await mediator.Send(command);

                if (response.IsSuccess())
                {
                    Close();
                }
                else
                {
                    _toastService.ShowError("Not Success.", "Error");
                }
            }
            catch (MediatorException ex)
            {
                _toastService.ShowError(ex.ErrorMessage, "Error");
            }
            catch (Exception ex)
            {
                _toastService.ShowError(ex.Message, "Error");
            }


        }
    }
}