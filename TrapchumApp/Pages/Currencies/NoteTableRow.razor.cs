using Applications.Interfaces.Currencies;
using Applications.Requests.Lists.Currency;
using Applications.Responses.ViewModels.Currencies;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Currencies
{
    public partial class NoteTableRow
    {

        [Parameter] public int? RowNumber { get; set; } = 0;
        [Parameter] public VCurrencyNotes Item { get; set; } = default!;
        [Parameter] public EventCallback<VCurrencyNotes> AllowRemovedRow { get; set; }
        [Inject] ICurrencyService cuService { get; set; } = default!;
        async Task makeInactiveOrActive()
        {
            var req = new NoteActivateDto
            {
                IsActive = Item.IsActive
            };

            var message = await cuService.ActivateOrDeactivateNote(Item.Id, req);

            if (message.IsSuccess)
            {
                await AllowRemovedRow.InvokeAsync(Item);
            }
            else
            {
                Item.IsActive = !Item.IsActive;
            }
        }
    }
}