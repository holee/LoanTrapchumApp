using Applications.Features.Currencies;
using Applications.Features.Currencies.Dto;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Currencies;

public partial class CurrencyDetials
{
    [Inject] private ICurrencyApi CurrencyApi { get; set; } = default!;

    [Parameter] public int Id { get; set; }

    private CurrencyDto _currency = new();

    private CurrencySpotRateDtoCreate SpotRateDtoCreate = new();

    private IEnumerable<CurrencyDtoSpotRate> currencySpotRates = [];

    private int pageIndex = 0;

    private int pagSize = 15;

    private DateTime? reqDate = DateTime.UtcNow;

    private bool isLoading = false;

    private CurrencyDtoSpotRate selectedSpotRate = new CurrencyDtoSpotRate();

    void Close()
    {
        NavigationManager.NavigateTo("/App/Currencies");
    }

    protected override async Task OnInitializedAsync()
    {
        SpotRateDtoCreate.TransactionDate = DateTime.Now;

        try
        {
            _currency = await CurrencyApi.GetCurrencyByIdAsync(Id);
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }

        await ChangePageIndex(pageIndex);

        await base.OnInitializedAsync();
    }

    private void SelectedRow(CurrencyDtoSpotRate param)
    {
        selectedSpotRate = param;

        if (param is not null)
        {
            reqDate = param.TransactionDate;
            SpotRateDtoCreate.SpotRate = param.SpotRate;
            SpotRateDtoCreate.BuyingRate = param.BuyingRate;
            SpotRateDtoCreate.SellingRate = param.SellingRate;
            SpotRateDtoCreate.TransactionDate = reqDate;
            SpotRateDtoCreate.EditSeq = param.EditSeq;
        }
        else
        {
            SpotRateDtoCreate = new();

            reqDate = DateTime.Now;
        }

        StateHasChanged();
    }

    async Task SelectPageChanged(int page)
    {
        pageIndex = page;

        await ChangePageIndex(page);

        StateHasChanged();
    }

    async Task FormSubmit()
    {
        try
        {
            await CurrencyApi.SetSpotRateAsync(Id, SpotRateDtoCreate);

            await ChangePageIndex(pageIndex);

            SpotRateDtoCreate = new();
        }
        catch (Exception ex)
        {
            NotificationService.Notify(NotificationSeverity.Error, ex.Message);
        }
    }

    private async Task ChangePageIndex(int pageIndex, int pageSize = 15)
    {
        try
        {
            isLoading = true;

            var result = await CurrencyApi.GetSpotRatesAsync(Id, pageIndex, pageSize);

            currencySpotRates = result.Items;
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
}
