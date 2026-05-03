using Applications.Features.Currencies.Command.ActivateOrDeactivateNote;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;
using Applications.Features.Currencies.Queries.GetCurrencyCounter;
using Applications.Features.Currencies.Responses;
using Domains.Models.Currencies;

namespace TrapchumApp.Pages.Currencies
{
    public partial class NoteList
    {
        [Parameter] public int Id { get; set; } = 2;
        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        private List<VCurrencyCounter> CurrencyCounters = [];
        private int pageIndex = 1;
        private int pageSize = 15;
        private string sortField = "CurrencyName";
        private string sortDir = "ASC";
        private bool includeInactive = false;
        private string searctText = string.Empty;
        private bool isSuccess = false;
        private bool isLoading = false;
        protected override async Task OnInitializedAsync()
        {

            await GetCurrencies();

            await LoadPages(BranchId);

            await base.OnInitializedAsync();
        }


        private async Task GetCurrencies()
        {
            if (Currencies is null)
            {
                var query = new GetCurrenciesQuery(0, new QueryParam(0, 0));

                var rsp = await mediator.Send(query);

                var temp = rsp.Items;

                temp.Add(new Currency { CurrencyId = 0, CurrencyCode = "All" });

                Currencies = temp.OrderBy(x => x.CurrencyId).ToList();
            }

        }

        async Task SelectedItemChanged(object obj)
        {
            if (obj is not null)
            {
                var selectedResult = (Currency)obj;

                await LoadPages(BranchId, selectedResult.CurrencyId);
            }

        }


        async Task DisplayCurrencyDialogue()
        {
            await DialogService.OpenAsync<NoteModalCreate>("Note");
        }

        async Task LoadPages(int branchId, int? currencyId = 0)
        {
            isLoading = true;

            var query = new GetCurrencyCounterQuery(currencyId, branchId);

            var rsp = await mediator.Send(query);

            CurrencyCounters = rsp.Items;

            isLoading = false;

            StateHasChanged();
        }

        async Task SelectPageChanged(int pageIndex)
        {
            this.pageIndex = pageIndex;

            await LoadPages(BranchId);
        }

        async Task SelectPageSizeChanged()
        {
            await LoadPages(BranchId);
        }

        async Task AllowRemovedRow(VCurrencyCounter note)
        {
            if (!includeInactive && !note.IsActive)
            {
                CurrencyCounters.Remove(note);
            }
            StateHasChanged();

            await Task.CompletedTask;

        }
        protected Task<bool> ValidateSuccess(bool success) => Task.FromResult(success);

        protected async Task CreateSuccess(bool param)
        {
            if (param)
            {
                await OnInitializedAsync();
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task ProcessSuccess(bool param)
        {
            if (param)
            {
                await OnInitializedAsync();
            }
            await InvokeAsync(StateHasChanged);
        }


        async Task makeInactiveOrActive(bool arg, int id)
        {
            var req = new NoteActivateDto(arg);

            var command = new ToggleNoteCommand(id, req);

            var rsp = await mediator.Send(command);  //_currencyService.ActivateOrDeactivateNote(id, req);

            if (rsp.IsSuccess())
            {
                NotificationService.Notify(NotificationSeverity.Success, rsp.Value);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, rsp.GetMessage());
            }
        }
    }
}