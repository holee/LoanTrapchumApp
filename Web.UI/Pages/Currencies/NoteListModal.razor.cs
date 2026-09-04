using Applications.Features.Currencies.Responses;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Currencies
{
    public partial class NoteListModal
    {
        [Inject] private ICurrencyApi CurrencyApi { get; set; } = default!;

        [Parameter] public int Id { get; set; }

        private List<VCurrencyCounter> CurrencyCounters = [];

        private bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                isLoading = true;

                var resp = await CurrencyApi.GetNotesAsync(0, Id);

                CurrencyCounters = resp.Items ?? [];
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
            finally
            {
                isLoading = false;
            }

            await base.OnInitializedAsync();
        }

        void Close()
        {
            DialogService.Close();
        }
    }
}
