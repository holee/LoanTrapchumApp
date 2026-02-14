using Applications.Features.Currencies;
using Applications.Features.Currencies.Command.ActivateOrDeactivateNote;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencyCounter;
using Applications.Features.Currencies.Queries.GetCurrencyNotes;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Currencies;
using Applications.Responses.Paginations;
using TrapchumApp.Helpers.Extensions;

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
            await LoadPages(BranchId);

            await base.OnInitializedAsync();
        }

        async Task DisplayCurrencyDialogue()
        {
            await DialogService.OpenAsync<NoteModalCreate>("Note");
        }

        async Task LoadPages(int branchId)
        {
            try
            {
                isLoading = true;

                var query = new GetCurrencyCounterQuery(0, branchId);

                var rsp = await mediator.Send(query);  
                
                //_currencyService.GetCurrencyNotes(branchId, 0);

                CurrencyCounters = rsp.Items;

                isLoading = false;
            }
            catch
            {
                isLoading = false;
            }

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