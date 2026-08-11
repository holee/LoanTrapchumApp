using Applications.Features.Currencies;
using Applications.Features.Currencies.Command.SetSpotRate;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Queries.GetSpotRates;
using Mapster;

namespace TrapchumApp.Pages.Currencies;

public partial class CurrencyDetials
{
    [Parameter] public int Id { get; set; }

    private CurrencyDto _currency = new();

    private CurrencySpotRateDtoCreate SpotRateDtoCreate = new();

    private IEnumerable<CurrencyDtoSpotRate> currencySpotRates = [];

    private int pageIndex = 0;

    private int pagSize = 15;

    private DateTime? reqDate = DateTime.UtcNow;

    bool isActive = false;

    private CurrencyDtoSpotRate selectedSpotRate = new CurrencyDtoSpotRate();

    void Close()
    {
        NavigationManager.NavigateTo("/App/Currencies");
    }
    protected override async Task OnInitializedAsync()
    {

        SpotRateDtoCreate.TransactionDate = DateTime.Now;

        var command = new GetCurrencyByIdQuery(Id);

        var curResult = await mediator.Send(command);

        if (curResult.IsSuccess())
        {
            _currency = curResult.Value;
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
        try
        {
            pageIndex = page;

            await ChangePageIndex(page);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        StateHasChanged();
    }

    async Task FormSubmit()
    {
        try
        {

            var message = await mediator.Send(new SetSpotRateCommand(Id, SpotRateDtoCreate));

            if (message.IsSuccess)
            {

                try
                {
                    await ChangePageIndex(pageIndex);

                    SpotRateDtoCreate = new();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        //StateHasChanged();
    }


    private async Task ChangePageIndex(int pageIndex, int pageSize = 15)
    {
        Tracker.Show();

        var qparam = new QueryParam(pageIndex, pageSize);

        var result = await mediator.Send(new GetSpotRatesQuery(Id, qparam)); //

        currencySpotRates = result.Items;

        Tracker.Hide();

        StateHasChanged();
    }




}