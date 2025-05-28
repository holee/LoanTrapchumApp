using Applications.Interfaces.Currencies;
using Applications.Requests.Lists.Currency;
using Applications.Responses.ViewModels.Currencies;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TrapchumApp.Components.Pages.Currencies
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
        private List<VCurrencyNotes> CurrencyCounters = new List<VCurrencyNotes>();
        private int pageIndex = 1;
        private int pageSize = 10;
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

        //async Task LoadPages(int pageIndex = 1, int pageSize = 10, string sortField = "CurrencyName", string sortDir = "ASC")
        //{
        //    var param = new QueryParam
        //    {
        //        PageIndex = pageIndex,
        //        PageSize = pageSize,
        //        SortDescending = true,
        //        SortColumn = sortField
        //    };

        //    //var data = await _currencyService.GetCurrencies(param, includeInactive, searctText);

        //    // StateHasChanged();
        //}

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

        async Task AllowRemovedRow(VCurrencyNotes note)
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