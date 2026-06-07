using Applications.Exceptions;
using Applications.Extensions.Enums;
using Applications.Extensions.Maths;
using Applications.Extensions.NumberExtensions;
using Applications.Features.Currencies.Command;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using Applications.Responses.ViewModels.Options;
using Domains.Enumerations;
using Domains.Models.Settings;
using TrapchumApp.Helpers.Constants;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Exchanges;

//IForiegnCurrencyExchange exchangeServiceImpl
public partial class ExchangesPopup
{
    [CascadingParameter] private CascadingAppState BState { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState> AuthenticateState { get; set; } = default!;
    [Parameter] public int? CurrencyId { get; set; } = 0;
    [Parameter] public decimal? Amount { get; set; } = 0;
    [Parameter] public EventCallback ClosePopup { get; set; } = default!;

    ForeignExchangeDtoCreate fcx = new ForeignExchangeDtoCreate();

    private ExchangeDtoCreate _mx = new();

    EditForm form = default!;

    private string? _userId = string.Empty;

    private readonly IEnumerable<Items> _buyCurrencies = [new Items(2, "US Dollar")];

    private readonly IEnumerable<Items> _sellCurrencies = [new Items(1, "Khmer Riels")];

    private IEnumerable<AppSettings> _settings = [];

    private VCurrenciesList? _currency = null;

    private bool _isBuy = true;

    private bool _isBusy = false;

    private int? _branchId = 0;
    protected override async Task OnInitializedAsync()
    {

        var auth = await AuthenticateState;

        _userId = auth.GetUserId();

        try
        {

            await branchService.GetCurrentBranchIdAsync();

            var rsp = await appSettingService.GetSystemSettings();

            _settings = rsp.Items;

            _currency = await currencyService.GetExchangedCurrencies((int)CurrencyEnum.KHR);

            await branchService.GetCurrentBranchIdAsync();

            _branchId = branchService.BranchId;

            if (_currency is not null)
            {
                _mx.ExchangeRate = _currency.BuyingRate;

                _mx.SpotRate = _currency.SpotRate;
            }
            else
            {
                _mx.ExchangeRate = Defaults.ExchangeRate;

                _mx.SpotRate = Defaults.SpotRate;

            }

            if (Amount > 0)
            {
                _mx.Amount = Amount.Value;

                var temp = _mx.ExchangeRate * _mx.Amount;

                _mx.TotalAmount = _isBuy ? temp.RoundDown(-2) : temp.RoundUp(-2);

            }


            _mx.SellCurrencyId = (int)CurrencyEnum.KHR;

            _mx.BuyCurrencyId = (int)CurrencyEnum.USD;

            if (CurrencyId == (int)CurrencyEnum.KHR)
            {
                _mx.ActionTypeId = 1;

                _isBuy = true;
            }
            else
            {
                _mx.ActionTypeId = 2;//1

                _isBuy = false;
            }



        }
        catch (MediatorException ex)
        {

            _isBuy = false;

            _toastService.ShowError(ex.GetErrorMessage(), "Error");
        }
        catch (Exception ex)
        {
            _isBusy = false;

            _toastService.ShowError(ex.Message, "Error");
        }

        await base.OnInitializedAsync();
    }


    void MouseOut(FocusEventArgs e)
    {
        if (_mx.Amount > 0 && _mx.TotalAmount > 0)
        {
            _mx.ExchangeRate = MathExtension.Round((_mx.TotalAmount / _mx.Amount), 2);
        }

    }


    void CurrencyModeSelected(object o)
    {

        if (_currency is null) return;

        var selectedValue = ((Items)o).Value;

        switch (selectedValue)
        {
            case 1:

                _isBuy = true;

                _mx.ExchangedMode = Defaults.SellCurrency;

                _mx.ExchangeRate = _currency.BuyingRate;

                break;

            case 2:

                _isBuy = false;

                _mx.ExchangedMode = Defaults.BuyCurrency;

                _mx.ExchangeRate = _currency.SellingRate;

                break;
        }

        var temp = _mx.ExchangeRate * _mx.Amount;

        _mx.TotalAmount = _isBuy ? temp.RoundDown(-2) : temp.RoundUp(-2);
    }

    void AmountTextChanged(decimal? amount)
    {

        if (amount <= 0)
        {
            _mx.Amount = 0;

            _mx.TotalAmount = 0;

            _toastService.ShowWarning("Invalid input amount");

            return;
        }

        if (_mx.ExchangeRate == 0)
        {
            _mx.TotalAmount = 0;

            return;
        }

        var temp = _mx.ExchangeRate * amount;

        _mx.TotalAmount = _isBuy ? temp.RoundDown(-2) : temp.RoundUp(-2);


    }



    void ExchangedTextChanged(decimal? rate)
    {

        if (rate <= 0)
        {
            _mx.ExchangeRate = 0;

            _mx.TotalAmount = 0;

            _toastService.ShowWarning("Invalid input Exchange Rate.");

            return;
        }

        if (_mx.Amount == 0)
        {
            _mx.TotalAmount = 0;

            return;
        }

        var temp = rate * _mx.Amount;

        _mx.TotalAmount = _isBuy ? temp.RoundDown(-2) : temp.RoundUp(-2);
    }


    void TextTotalChanged(decimal? total)
    {
        if (total <= 0)
        {
            _mx.TotalAmount = 0;

            _toastService.ShowWarning("Invalid input Total Amount.");

            return;
        }

        if (_mx.Amount == 0)
        {
            _mx.TotalAmount = 0;

            return;
        }

        _mx.ExchangeRate = MathExtension.Round((total / _mx.Amount), 2);
    }
    async Task SaveAndNew()
    {
        await Save2();
    }

    async Task SaveAndClose()
    {
        await Save2(false);
    }

    async Task Save2(bool saveNew = true)
    {
        form.EditContext!.Validate();

        _isBusy = true;

        var FXGAIN_KHR_ID = _settings.FirstOrDefault(p => p.Code == "FXGAIN-KHR")!.Value.ToNumer<int>();

        var FXLOSS_KHR_ID = _settings.FirstOrDefault(p => p.Code == "FXLOSS-KHR")!.Value.ToNumer<int>();

        var FXEPA_USD_ID = _settings.FirstOrDefault(p => p.Code == "FXEPA-USD")!.Value.ToNumer<int>();

        var FXPA_KHR_ID = _settings.FirstOrDefault(p => p.Code == "FXPA-KHR")!.Value.ToNumer<int>();

        if (_mx.Amount <= 0)
        {
            NotificationService.Notify(NotificationSeverity.Info, "Exchange Amount is not valid");



            _isBusy = false;

            return;
        }

        if (_mx.TotalAmount <= 0)
        {

            _toastService.ShowInfo("Total Amount must be greater than 0", "Validation Error");

            _isBusy = false;

            return;
        }
        if (_mx.TotalAmount < _mx.Amount)
        {

            _toastService.ShowInfo("Total Amount must be greater than Amount", "Validation Error");

            _isBusy = false;

            return;
        }

        fcx.Transaction = new TransactionBase
        {
            UserRefId = _userId,
            TranTypeRefId = (int)TransactionTypes.CFX,
            TransactionTypeName = TransactionTypes.CFX.Description(),
            BranchRefId = _branchId ?? 1,
        };

        fcx.StaffRefId = _userId;
        fcx.StaffAuthRefId = _userId;
        fcx.BranchRefId = _branchId ?? 1;
        fcx.BankRefId = 11;
        fcx.SpotRate = _mx.SpotRate;
        fcx.Remark = _mx.Description;

        if (_isBusy)
        {
            fcx.BuyAmount = _mx.Amount;
            fcx.IsBuy = _mx.IsBuy;
            fcx.SellAmount = _mx.TotalAmount;
            fcx.BuyCurrencyRefId = (int)CurrencyEnum.USD; //2;
            fcx.SellCurrencyRefId = (int)CurrencyEnum.KHR;//1;
            fcx.ExchangeRate = _mx.ExchangeRate;
            fcx.BuyLedgerRefId = (int)CurrencyEnum.KHR;// 1;
            fcx.SellLedgerRefId = (int)CurrencyEnum.USD;//2
            fcx.GainAndLostAmount = (_mx.Amount * _mx.SpotRate) - _mx.TotalAmount;
            fcx.FXEPA_Amount = _mx.Amount;//FXEPA_Amount
            fcx.FXPA_Amount = _mx.Amount * _mx.SpotRate;//FXPA_Amount
            fcx.FxepaLedgerRefId = FXEPA_USD_ID;//FXEPA_LedgerRefId
            fcx.FxpaLedgerRefId = FXPA_KHR_ID;//FXPA_LedgerRefId
            fcx.SpotRate = _mx.SpotRate;
            if (fcx.GainAndLostAmount > 0)
            {
                fcx.GainAndLostLedgerRefId = FXGAIN_KHR_ID;
            }
            else
            {
                fcx.GainAndLostLedgerRefId = FXLOSS_KHR_ID;
            }
        }
        else
        {
            fcx.IsBuy = _mx.IsBuy;
            fcx.BuyAmount = _mx.TotalAmount;
            fcx.SellAmount = _mx.Amount;
            fcx.BuyCurrencyRefId = (int)CurrencyEnum.KHR; //1;
            fcx.SellCurrencyRefId = (int)CurrencyEnum.USD;//2;
            fcx.ExchangeRate = _mx.ExchangeRate;
            fcx.BuyLedgerRefId = (int)CurrencyEnum.USD;//2;
            fcx.SellLedgerRefId = (int)CurrencyEnum.KHR;//1;
            fcx.GainAndLostAmount = _mx.TotalAmount - (_mx.Amount * _mx.SpotRate);
            fcx.FXEPA_Amount = _mx.Amount;
            fcx.FXPA_Amount = _mx.Amount * _mx.SpotRate;
            fcx.SpotRate = _mx.SpotRate;
            fcx.FxepaLedgerRefId = FXEPA_USD_ID;
            fcx.FxpaLedgerRefId = FXPA_KHR_ID;
            if (fcx.GainAndLostAmount > 0)
            {
                fcx.GainAndLostLedgerRefId = FXGAIN_KHR_ID;
            }
            else
            {
                fcx.GainAndLostLedgerRefId = FXLOSS_KHR_ID;
            }
        }

        var command = new CreateMoneyExchangeCommand(fcx);

        var result = await mediator.Send(command);

        if (!result.IsSuccess())
        {

            _toastService.ShowError(result.GetMessage(), "Failed to create exchange");

            _isBusy = false;

            return;
        }

        _mx.Amount = 0;

        _mx.TotalAmount = 0;

        _mx.Description = string.Empty;

        _isBusy = false;

        _toastService.ShowSuccess("Money exchange created successfully.", "Success");

        DialogService.Close(result.Value);

    }



    private async Task ClosingSidebar(MouseEventArgs args)
    {
        await ClosePopup.InvokeAsync();
    }
}


/// <summary>
/// The model for the foreign exchange transaction.
/// </summary>
//private TrxForeignExchangeDtoCreate _model = new TrxForeignExchangeDtoCreate();

//async Task Save(bool saveNew = true)
//{
//    form.EditContext!.Validate();

//    await BState.GetBranchIdByUser();

//    await BState.GetUserId();

//    _isBusy = true;

//    var FXGAIN_KHR_ID = _settings.FirstOrDefault(p => p.Code == "FXGAIN-KHR")!.Value.ToNumer<int>();

//    var FXLOSS_KHR_ID = _settings.FirstOrDefault(p => p.Code == "FXLOSS-KHR")!.Value.ToNumer<int>();

//    var FXEPA_USD_ID = _settings.FirstOrDefault(p => p.Code == "FXEPA-USD")!.Value.ToNumer<int>();

//    var FXPA_KHR_ID = _settings.FirstOrDefault(p => p.Code == "FXPA-KHR")!.Value.ToNumer<int>();

//    if (_mx.Amount <= 0)
//    {
//        _toastService.ShowInfo("Exchange Amount is not valid");

//        _isBusy = false;

//        return;
//    }

//    if (_mx.TotalAmount <= 0)
//    {

//        _toastService.ShowInfo("Total Amount must be greater than 0");

//        _isBusy = false;
//        return;
//    }
//    if (_mx.TotalAmount < _mx.Amount)
//    {

//        _toastService.ShowInfo("Total Amount must be greater than Amount");

//        _isBusy = false;

//        return;
//    }
//    _model.UserRefId = _userId ?? BState.UserId;

//    _model.TranTypeRefId = 67;

//    _model.BranchRefId = branchService.BranchId > 0 ? branchService.BranchId.Value : BState.BranchId;

//    _model.TransactionTypeName = TransactionTypes.CFX.GetDescription();//"Currency Foreign Exchange";

//    fcx.StaffRefId = _userId ?? BState.UserId;

//    fcx.StaffAuthRefId = _userId ?? BState.UserId;

//    fcx.BranchRefId = branchService.BranchId > 0 ? branchService.BranchId.Value : BState.BranchId;

//    fcx.BankRefId = 11;

//    fcx.SpotRate = _mx.SpotRate;

//    fcx.Remark = _mx.Description;

//    fcx.NameRefId = BState.UserId;

//    if (_isBuy)
//    {
//        fcx.BuyAmount = _mx.Amount;
//        fcx.SellAmount = _mx.TotalAmount;
//        fcx.BuyCurrencyRefId = 2;
//        fcx.SellCurrencyRefId = 1;
//        fcx.ExchangeRate = _mx.ExchangeRate;
//        fcx.BuyLedgerRefId = 1;
//        fcx.SellLedgerRefId = 2;
//        fcx.GainAndLostAmount = (_mx.Amount * _mx.SpotRate) - _mx.TotalAmount;
//        fcx.FXEPA_Amount = _mx.Amount;//FXEPA_Amount
//        fcx.FXPA_Amount = _mx.Amount * _mx.SpotRate;//FXPA_Amount
//        fcx.FxepaLedgerRefId = FXEPA_USD_ID;//FXEPA_LedgerRefId
//        fcx.FxpaLedgerRefId = FXPA_KHR_ID;//FXPA_LedgerRefId
//        fcx.SpotRate = _mx.SpotRate;
//        if (fcx.GainAndLostAmount > 0)
//        {
//            fcx.GainAndLostLedgerRefId = FXGAIN_KHR_ID;
//        }
//        else
//        {
//            fcx.GainAndLostLedgerRefId = FXLOSS_KHR_ID;
//        }
//    }
//    else
//    {
//        fcx.BuyAmount = _mx.TotalAmount;
//        fcx.SellAmount = _mx.Amount;
//        fcx.BuyCurrencyRefId = 1;
//        fcx.SellCurrencyRefId = 2;
//        fcx.ExchangeRate = _mx.ExchangeRate;
//        fcx.BuyLedgerRefId = 2;
//        fcx.SellLedgerRefId = 1;
//        fcx.GainAndLostAmount = _mx.TotalAmount - (_mx.Amount * _mx.SpotRate);
//        fcx.FXEPA_Amount = _mx.Amount;
//        fcx.FXPA_Amount = _mx.Amount * _mx.SpotRate;
//        fcx.SpotRate = _mx.SpotRate;
//        fcx.FxepaLedgerRefId = FXEPA_USD_ID;
//        fcx.FxpaLedgerRefId = FXPA_KHR_ID;
//        fcx.GainAndLostLedgerRefId = fcx.GainAndLostAmount > 0 ? FXGAIN_KHR_ID : FXLOSS_KHR_ID;
//    }

//    _model.ForeignExchanges = fcx;

//    var req = _model.Adapt<LoanTransaction>();


//    var result = await exchangeServiceImpl.Exchange(req);

//    if (!result.IsSuccess)
//    {
//        _toastService.ShowError(result.ErrMessage, "Error");

//        _isBusy = false;

//        return;
//    }

//    _mx.Amount = 0;

//    _mx.TotalAmount = 0;

//    _mx.Description = string.Empty;

//    _isBusy = false;

//    _toastService.ShowSuccess("Currency exchange transaction has been successfully saved.");

//    if (!saveNew)
//    {
//        await ClosePopup.InvokeAsync();
//    }

//}
