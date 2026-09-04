using Applications.Features.Currencies.Command.SetSpotRates;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrenciesForSetSpotRates;
using Applications.Features.Currencies.Responses;

namespace Web.UI.Pages.Transactions.Exchanges;

public partial class SetDailySpotRate
{

    private IEnumerable<VCurrenciesList>? Currencies = new List<VCurrenciesList>();

    private List<CurrencySpotRateDtoCreate> model = new List<CurrencySpotRateDtoCreate>();

    private DateTime reqDate = DateTime.Now;
    protected override async Task OnInitializedAsync()
    {
        try
        {

            var rsp = await mediator.Send(new GetCurrenciesForSetSpotRatesQuery(true, new QueryParam(0, 15)));

            Currencies = rsp.Items;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        await base.OnInitializedAsync();
    }


    async Task SetSpotRate()
    {
        model = Currencies!.Select(x => new CurrencySpotRateDtoCreate
        {
            CurrencyRefId = x.CurrencyId,
            SpotRate = x.SpotRate,
            BuyingRate = x.BuyingRate,
            SellingRate = x.SellingRate,
            TransactionDate = reqDate
        }).ToList();

        var requestNo = DateTime.Now.Subtract(reqDate).Days;

        if (requestNo != 0)
        {
            _toastService.ShowWarning("Date Provided is not valid date.The Provide date must be current Date Set Daily SPot Rate", "Error");

            return;
        }

        var rsp = await mediator.Send(new SetSpotRatesCommand(model));

        if (rsp.IsSuccess)
        {
            _toastService.ShowSuccess("Spot Rate has been set successfully", "Success");

            NavigationManager.NavigateTo("/Account/Currencies");
        }
        else
        {

            _toastService.ShowError(rsp.GetMessage(), "Error");
        }

    }

    void CheckSellingRate(FocusEventArgs e)
    {
        for (int x = 0; x < Currencies!.Count(); x++)
        {
            if (Currencies!.ToList()[x].SellingRate < Currencies!.ToList()[x].BuyingRate
            || Currencies!.ToList()[x].SellingRate < Currencies!.ToList()[x].SpotRate)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Selling Rate must greater than others");
                Currencies!.ToList()[x].SellingRate = 0;
                break;
            }
        }
    }
}