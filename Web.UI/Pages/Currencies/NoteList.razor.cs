using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Currencies;

public partial class NoteList
{
    [Inject] private ICurrencyApi CurrencyApi { get; set; } = default!;

    [Parameter] public int Id { get; set; } = 2;
    [Parameter, SupplyParameterFromQuery] public int BranchId { get; set; }

    private List<VCurrencyCounter> CurrencyCounters = [];
    private int pageIndex = 1;
    private int pageSize = 15;
    private bool includeInactive = false;
    private bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _currencyId = 0;

        await GetCurrencies();

        await LoadPages(BranchId);

        await base.OnInitializedAsync();
    }

    private async Task GetCurrencies()
    {
        if (Currencies is null)
        {
            try
            {
                var rsp = await CurrencyApi.GetCurrencyListAsync(0, 0, 0);

                var temp = rsp.Items.ToList();

                temp.Add(new CurrencyDto { CurrencyId = 0, CurrencyCode = "All" });

                Currencies = temp.OrderBy(x => x.CurrencyId).ToList();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }
    }

    async Task SelectedItemChanged(object obj)
    {
        if (obj is not null)
        {
            var selectedResult = (CurrencyDto)obj;

            _currencyId = selectedResult.CurrencyId;

            await LoadPages(BranchId, selectedResult.CurrencyId);
        }
    }

    async Task DisplayCurrencyDialogue()
    {
        var parameters = new Dictionary<string, object?>
        {
            { "Id", _currencyId }
        };

        await DialogService.OpenAsync<NoteModalCreate>("Note", parameters);

        await LoadPages(BranchId, _currencyId);
    }

    async Task LoadPages(int branchId, int? currencyId = 0)
    {
        try
        {
            isLoading = true;

            var rsp = await CurrencyApi.GetNotesAsync(currencyId, branchId);

            CurrencyCounters = rsp.Items ?? [];
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
        finally
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

    async Task makeInactiveOrActive(bool arg, int id)
    {
        try
        {
            var rsp = await CurrencyApi.ToggleNoteAsync(id, new NoteToggleRequest(arg));

            NotificationService.Notify(NotificationSeverity.Success, rsp.Message);

            await LoadPages(BranchId, _currencyId);
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }
}
