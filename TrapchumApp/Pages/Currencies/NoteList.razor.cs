using Applications.Features.Currencies;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Currencies;

namespace TrapchumApp.Pages.Currencies
{
    public partial class NoteList
    {

        private readonly ICurrencyService _currencyService;

        public NoteList(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [Parameter] public int Id { get; set; } = 2;
        [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }
        private List<VCurrencyCounter> CurrencyCounters = new List<VCurrencyCounter>();
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

                var responseRsult = await _currencyService.GetCurrencyNotes(branchId);

                if (responseRsult.IsSuccess)
                {
                    CurrencyCounters = responseRsult.Data!.ToList();
                }
                else
                {
                    CurrencyCounters = [];
                }

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
            var req = new NoteActivateDto
            {
                IsActive = arg
            };
            var resultResponse = await _currencyService.ActivateOrDeactivateNote(id, req);

            if (resultResponse.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "");
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, resultResponse.ErrMessage);
            }
        }
    }
}