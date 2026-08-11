using Applications.Features.Currencies.Command.AddNewCurrency;
using Applications.Features.Currencies.Dto;

namespace TrapchumApp.Pages.Currencies
{
    public partial class CurrencyModalCreate
    {
        private CurrencyDtoCreate model = new CurrencyDtoCreate();

        private EditForm? editForm;

        private void Close() => DialogService.Close(false);

        private async Task FormSubmit()
        {
            var response = await mediator.Send(new AddNewCurrencyCommand(model));

            if (response.IsSuccess())
            {
                _toastService.ShowSuccess("Currency record was created successfully!", "Success");

                DialogService.Close(response.Value);
            }
            else
            {
                _toastService.ShowError(response.GetMessage(), "Error");
            }
        }
    }
}