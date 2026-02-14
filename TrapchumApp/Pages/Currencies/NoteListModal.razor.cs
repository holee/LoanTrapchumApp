using Applications.Features.Currencies.Queries.GetCurrencyCounter;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Currencies;
using Applications.Responses.Paginations;
using Microsoft.AspNetCore.Components;

namespace TrapchumApp.Pages.Currencies
{
    public partial class NoteListModal
    {
        [Inject] ICurrencyService CurrencyService { get; set; } = default!;
        [Parameter] public int Id { get; set; }
        private List<VCurrencyCounter> CurrencyCounters = [];
        private int pageIndex = 1;
        private int pageSize = 10;
        private bool includeInactive = false;
        private string searctText = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var param = new QueryParam(pageIndex, pageSize, "CurrencyValue");

            var command = new GetCurrencyCounterQuery(Id, param);

            var resp = await mediator.Send(command);  //CurrencyService.GetCurrencyCounter(Id, param);

            CurrencyCounters = resp.Items;

            await base.OnInitializedAsync();
        }

        void Close()
        {
            DialogService.Close();
        }
    }
}