using Applications.Interfaces.Currencies;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Currencies
{
    public partial class NoteListModal
    {
        [Inject] ICurrencyService CurrencyService { get; set; } = default!;
        [Parameter] public int Id { get; set; }
        private List<VCurrencyNotes> CurrencyCounters = [];
        private int pageIndex = 1;
        private int pageSize = 10;
        private bool includeInactive = false;
        private string searctText = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var param = new QueryParam(pageIndex, pageSize, "CurrencyValue");

            var resp = await CurrencyService.GetCurrencyNotes(Id, param);

            if (resp.IsSuccess)
            {
                CurrencyCounters = resp.Data.Items;
            }

            await base.OnInitializedAsync();
        }

        void Close()
        {
            DialogService.Close();
        }
    }
}