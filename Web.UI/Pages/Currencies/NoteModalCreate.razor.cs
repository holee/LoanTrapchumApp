using Applications.Features.Currencies.Dto;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Currencies
{
    public partial class NoteModalCreate
    {
        [Inject] private ICurrencyApi CurrencyApi { get; set; } = default!;

        [Parameter] public int Id { get; set; }

        CurrencyNoteDtoCreate model = new CurrencyNoteDtoCreate();

        IEnumerable<CurrencyDto> Currencies = new List<CurrencyDto>();

        void Close() => DialogService.Close(false);

        protected override async Task OnInitializedAsync()
        {
            model.CurrencyRefId = Id == 0 ? 1 : Id;

            model.Quantity = 0;

            try
            {
                var cur = await CurrencyApi.GetCurrencyListAsync(0, 0, 0);

                Currencies = cur.Items;
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

            await base.OnInitializedAsync();
        }

        async Task FormSubmit()
        {
            try
            {
                var created = await CurrencyApi.CreateNoteAsync(model);

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Success",
                    Detail = "Currency note was created successfully!"
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
