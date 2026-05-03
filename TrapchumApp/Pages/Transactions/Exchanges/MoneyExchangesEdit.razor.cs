using Applications.Extensions.Enums;
using Applications.Extensions.Maths;
using Applications.Extensions.NumberExtensions;
using Applications.Features.Currencies.Command.EditMoneyExchange;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Queries.GetEditMoneyExchange;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.setting;
using Applications.Responses.ViewModels.Options;
using Domains.Enumerations;
using Domains.Models.Settings;
using TrapchumApp.Helpers.Constants;

namespace TrapchumApp.Pages.Transactions.Exchanges;

public partial class MoneyExchangesEdit
{
    [Parameter] public int BranchId { get; set; } = 0;
    [Parameter] public string TransactionId { get; set; } = string.Empty;
    [Parameter] public bool ShowCloseButton { get; set; } = false;
    [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    ForeignExchangeDtoUpdate _cfx = new();
    ExchangeDtoEdit mx = new();
    EditForm form = default!;
    IEnumerable<Items> buyCurrencies = [new Items(2, "US Dollar")];
    IEnumerable<Items> sellCurrencies = [new Items(1, "Khmer Riels")];
    IEnumerable<AppSettings> settings = [];
    VCurrenciesList Currency = new();
    bool isBuy = true;
    bool isBusy = false;
    private readonly ICurrencyService cuService;
    private readonly IAppSettingService settingService;
    private string? _userId { get; set; }
    public MoneyExchangesEdit(ICurrencyService currencyService, IAppSettingService settingsServiceImpl)
    {
        cuService = currencyService;
        settingService = settingsServiceImpl;
    }

    protected override async Task OnInitializedAsync()
    {

        var authState = await AuthenticationState;

        _userId = authState.GetUserId();

        var resultSetting = await settingService.GetSysSettings();

        if (resultSetting.IsSuccess)
        {
            settings = resultSetting.Data!;
        }

        await GetEditMoneyExchangeResponse();

        var rsp = await mediator.Send(new GetCurrencyDetailsByIdQuery(1));

        if (rsp.IsSuccess())
        {
            Currency = rsp.Value;
            mx.SpotRate = Currency.SpotRate;
        }
        else
        {
            mx.SpotRate = Defaults.SpotRate;
        }
        await base.OnInitializedAsync();
    }

    private async Task GetEditMoneyExchangeResponse()
    {
        try
        {
            var query = new GetEditMoneyExchangeQuery(BranchId, TransactionId, DateTimeOffset.UtcNow);

            var result = await mediator.Send(query);

            if (result.IsSuccess())
            {
                mx = result.Value;

                if (mx.IsBuy)
                {
                    isBuy = mx.IsBuy;

                    mx.ActionTypeId = (int)ExchangeAction.BuyCurrency;// (int)CurrencyEnum.KHR;

                    mx.ExchangedMode = "Sell Currency";

                    mx.Amount = mx.BuyAmount;

                    mx.TotalAmount = mx.SellAmount;
                }
                else
                {
                    isBuy = mx.IsBuy;

                    mx.ActionTypeId = (int)ExchangeAction.SellCurrency;// (int)CurrencyEnum.USD;

                    mx.ExchangedMode = " Buy Currency";

                    mx.Amount = mx.SellAmount;

                    mx.BuyCurrencyId = (int)CurrencyEnum.USD;

                    mx.SellCurrencyId = (int)CurrencyEnum.KHR;

                    mx.TotalAmount = mx.BuyAmount;

                }


            }
        }
        catch (Exception ex)
        {

            isBusy = false;

            NotificationService.Notify(NotificationSeverity.Error, ex.Message);

        }

    }





    void MouseOut(FocusEventArgs e)
    {
        if (mx.Amount > 0 && mx.TotalAmount > 0)
        {
            mx.ExchangeRate = MathExtension.Round((mx.TotalAmount / mx.Amount), 2);

            return;
        }

    }


    void CurrencyModeSelected(object o)
    {

        var selectedValue = ((Items)o).Value;

        if (selectedValue == (int)ExchangeAction.BuyCurrency) //1
        {
            isBuy = true;

            mx.IsBuy = true;

            mx.ExchangedMode = "Sell Currency";

            mx.ExchangeRate = Currency!.BuyingRate;
        }
        if (selectedValue == (int)ExchangeAction.SellCurrency)//2
        {
            isBuy = false;

            mx.IsBuy = false;

            mx.ExchangedMode = "Buy Currency";

            mx.ExchangeRate = Currency!.SellingRate;
        }

        var temp = mx.ExchangeRate * mx.Amount;

        if (isBuy)
        {
            mx.TotalAmount = temp.RoundDown(-2);
        }
        else
        {
            mx.TotalAmount = temp.RoundUp(-2);
        }
    }

    void AmountTextChanged(decimal? amount)
    {

        if (amount <= 0)
        {
            mx.Amount = 0;

            mx.TotalAmount = 0;

            NotificationService.Notify(NotificationSeverity.Warning, "Invalid input amount");

            return;
        }

        if (mx.ExchangeRate == 0)
        {
            mx.TotalAmount = 0;

            return;
        }


        var temp = mx.ExchangeRate * amount;

        if (isBuy)
        {
            mx.TotalAmount = temp.RoundDown(-2);
        }
        else
        {
            mx.TotalAmount = temp.RoundUp(-2);
        }


    }



    void ExchangedTextChanged(decimal? rate)
    {

        if (rate <= 0)
        {
            mx.ExchangeRate = 0;
            mx.TotalAmount = 0;
            NotificationService.Notify(NotificationSeverity.Warning, "Invalid input Exchange Rate.");
            return;
        }

        if (mx.Amount == 0)
        {
            mx.TotalAmount = 0;

            return;
        }

        var temp = rate * mx.Amount;

        if (isBuy)
        {
            mx.TotalAmount = temp.RoundDown(-2);
        }
        else
        {
            mx.TotalAmount = temp.RoundUp(-2);
        }
    }


    void TextTotalChanged(decimal? total)
    {
        if (total <= 0)
        {
            mx.TotalAmount = 0;

            NotificationService.Notify(NotificationSeverity.Warning, "Invalid input Total Amount.");

            return;
        }

        if (mx.Amount == 0)
        {
            mx.TotalAmount = 0;
            return;
        }

        mx.ExchangeRate = MathExtension.Round((total / mx.Amount), 2);
    }
    async Task SaveAndNew()
    {
        await Save();
    }

    async Task SaveAndClose()
    {
        await Save(false);
    }

    async Task Save(bool saveNew = true)
    {
        form.EditContext!.Validate();
        isBusy = true;
        var FXGAIN_KHR_ID = settings.FirstOrDefault(p => p.Code == "FXGAIN-KHR")!.Value.ToNumer<int>();

        var FXLOSS_KHR_ID = settings.FirstOrDefault(p => p.Code == "FXLOSS-KHR")!.Value.ToNumer<int>();

        var FXEPA_USD_ID = settings.FirstOrDefault(p => p.Code == "FXEPA-USD")!.Value.ToNumer<int>();

        var FXPA_KHR_ID = settings.FirstOrDefault(p => p.Code == "FXPA-KHR")!.Value.ToNumer<int>();

        if (mx.Amount <= 0)
        {
            NotificationService.Notify(NotificationSeverity.Info, "Exchange Amount is not valid");
            isBusy = false;
            return;
        }

        if (mx.TotalAmount <= 0)
        {
            NotificationService.Notify(NotificationSeverity.Info, "Total Amount must be greater than 0");
            isBusy = false;
            return;
        }
        if (mx.TotalAmount < mx.Amount)
        {
            NotificationService.Notify(NotificationSeverity.Info, "Total Amount must be greater than Amount");
            isBusy = false;
            return;
        }

        _cfx.Transaction = new TransactionBase
        {
            TransactionId = mx.TransactionId,
            UserRefId = _userId,
            TranTypeRefId = (int)TransactionTypes.CFX,
            TransactionTypeName = TransactionTypes.CFX.Description(),
            TransactionDate = DateTimeOffset.UtcNow,
            BranchRefId = BranchId
        };
        _cfx.TranRefId = mx.TransactionId;
        _cfx.Id = mx.Id;
        _cfx.StaffRefId = _userId;
        _cfx.StaffAuthRefId = _userId;
        _cfx.BranchRefId = BranchId;
        _cfx.BankRefId = 11;
        _cfx.SpotRate = mx.SpotRate;
        _cfx.Remark = mx.Description;
        if (isBuy)
        {
            _cfx.IsBusy = true;
            _cfx.BuyAmount = mx.Amount;
            _cfx.SellAmount = mx.TotalAmount;
            _cfx.BuyCurrencyRefId = (int)CurrencyEnum.USD;// 2;
            _cfx.SellCurrencyRefId = (int)CurrencyEnum.KHR;// 1;
            _cfx.ExchangeRate = mx.ExchangeRate;
            _cfx.BuyLedgerRefId = 1;
            _cfx.SellLedgerRefId = 2;
            _cfx.GainAndLostAmount = (mx.Amount * mx.SpotRate) - mx.TotalAmount;
            _cfx.FXEPA_Amount = mx.Amount;//FXEPA_Amount
            _cfx.FXPA_Amount = mx.Amount * mx.SpotRate;//FXPA_Amount
            _cfx.FXEPALedgerRefId = FXEPA_USD_ID;//FXEPA_LedgerRefId
            _cfx.FXPALedgerRefId = FXPA_KHR_ID;//FXPA_LedgerRefId
            _cfx.SpotRate = mx.SpotRate;
        }
        else
        {
            _cfx.IsBusy = false;
            _cfx.BuyAmount = mx.TotalAmount;
            _cfx.SellAmount = mx.Amount;
            _cfx.BuyCurrencyRefId = (int)CurrencyEnum.KHR;// 1;
            _cfx.SellCurrencyRefId = (int)CurrencyEnum.USD;// 2;
            _cfx.ExchangeRate = mx.ExchangeRate;
            _cfx.BuyLedgerRefId = 2;
            _cfx.SellLedgerRefId = 1;
            _cfx.GainAndLostAmount = mx.TotalAmount - (mx.Amount * mx.SpotRate);
            _cfx.FXEPA_Amount = mx.Amount;
            _cfx.FXPA_Amount = mx.Amount * mx.SpotRate;
            _cfx.SpotRate = mx.SpotRate;
            _cfx.FXEPALedgerRefId = FXEPA_USD_ID;
            _cfx.FXPALedgerRefId = FXPA_KHR_ID;
        }

        if (_cfx.GainAndLostAmount > 0)
        {
            _cfx.GainAndLostLedgerRefId = FXGAIN_KHR_ID;
        }
        else
        {
            _cfx.GainAndLostLedgerRefId = FXLOSS_KHR_ID;
        }
        var command = new EditMoneyExchangeCommand(_cfx);

        var response = await mediator.Send(command);

        if (!response.IsSuccess())
        {
            NotificationService.Notify(NotificationSeverity.Error, response.GetMessage());

            isBusy = false;

            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Money Exchange was changed successfully.");

        mx.Amount = 0;

        mx.TotalAmount = 0;

        mx.Description = string.Empty;

        isBusy = false;

        DialogService.Close(response.Value);
    }

    void Close() => DialogService.Close(null);
}
