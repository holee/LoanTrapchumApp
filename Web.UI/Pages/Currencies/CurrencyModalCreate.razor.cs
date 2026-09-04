using Applications.Features.Currencies.Dto;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Currencies
{
    public partial class CurrencyModalCreate
    {
        [Inject] private ICurrencyApi CurrencyApi { get; set; } = default!;

        private CurrencyDtoCreate model = new CurrencyDtoCreate();

        private void Close() => DialogService.Close(false);

        private async Task FormSubmit()
        {
            try
            {
                var created = await CurrencyApi.CreateCurrencyAsync(model);

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Success",
                    Detail = "Currency record was created successfully!"
                });

                DialogService.Close(created);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = ex.Message
                });
            }
        }
    }
}
