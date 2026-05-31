using Applications.Commons.Extensions;
using Applications.Extensions.Maths;
using Applications.Extensions.NumberExtensions;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using Applications.Interfaces.setting;
using Applications.Interfaces.Transactions;
using Applications.Responses.ViewModels.Options;
using Domains.Enumerations;
using Domains.Models.Settings;
using Domains.Models.Transaction;
using Mapster;
using TrapchumApp.Helpers.Constants;
using TrapchumApp.Services;
using TrapchumApp.Shared;

namespace TrapchumApp.Pages.Transactions.Exchanges
{
    public partial class ExchangesPopup(IAppSettingService settingsServiceImpl, IForiegnCurrencyExchange exchangeServiceImpl)
    {
        [CascadingParameter] private CascadingAppState BState { get; set; } = default!;
        [CascadingParameter] private Task<AuthenticationState> AuthenticateState { get; set; } = default!;
        [Parameter] public int? CurrencyId { get; set; } = 0;
        [Parameter] public decimal? Amount { get; set; } = 0;
        [Parameter] public EventCallback ClosePopup { get; set; } = default!;

        ForeignExchangeDtoCreate c = new ForeignExchangeDtoCreate();

        private TrxForeignExchangeDtoCreate _model = new TrxForeignExchangeDtoCreate();

        private ExchangeDtoCreate _mx = new();

        EditForm form = default!;

        private string? _userId = string.Empty;

        private readonly IEnumerable<Items> _buyCurrencies = [new Items(2, "US Dollar")];

        private readonly IEnumerable<Items> _sellCurrencies = [new Items(1, "Khmer Riels")];

        private IEnumerable<AppSettings> _settings = [];

        private VCurrenciesList? _currency = null;

        private bool _isBuy = true;

        private bool _isBusy = false;

        protected override async Task OnInitializedAsync()
        {

            var auth = await AuthenticateState;

            _userId = auth.GetUserId();

            await branchService.GetCurrentBranchIdAsync();

            var resultSetting = await settingsServiceImpl.GetSysSettings();

            if (resultSetting.IsSuccess)
            {
                _settings = resultSetting.Data!;
            }

            if (Amount > 0)
            {
                _mx.Amount = Amount.Value;
            }

            _currency = await currencyService.GetExchangedCurrencies((int)CurrencyEnum.KHR);

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


            _mx.SellCurrencyId = 1;

            _mx.BuyCurrencyId = 2;

            _mx.ActionTypeId = 1;

            _isBuy = true;



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

            var selectedValue = ((Items)o).Value;

            switch (selectedValue)
            {
                case 1:
                    _isBuy = true;

                    _mx.ExchangedMode = Defaults.SellCurrency;

                    _mx.ExchangeRate = _currency!.BuyingRate!.Value;

                    break;
                case 2:

                    _isBuy = false;

                    _mx.ExchangedMode = Defaults.BuyCurrency;

                    _mx.ExchangeRate = _currency!.SellingRate;

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
            await Save();
        }

        async Task SaveAndClose()
        {
            await Save(false);
        }

        async Task Save(bool saveNew = true)
        {
            form.EditContext!.Validate();

            await BState.GetBranchIdByUser();

            await BState.GetUserId();

            _isBusy = true;

            var FXGAIN_KHR_ID = _settings.FirstOrDefault(p => p.Code == "FXGAIN-KHR")!.Value.ToNumer<int>();

            var FXLOSS_KHR_ID = _settings.FirstOrDefault(p => p.Code == "FXLOSS-KHR")!.Value.ToNumer<int>();

            var FXEPA_USD_ID = _settings.FirstOrDefault(p => p.Code == "FXEPA-USD")!.Value.ToNumer<int>();

            var FXPA_KHR_ID = _settings.FirstOrDefault(p => p.Code == "FXPA-KHR")!.Value.ToNumer<int>();

            if (_mx.Amount <= 0)
            {
                _toastService.ShowInfo("Exchange Amount is not valid");

                _isBusy = false;

                return;
            }

            if (_mx.TotalAmount <= 0)
            {

                _toastService.ShowInfo("Total Amount must be greater than 0");

                _isBusy = false;
                return;
            }
            if (_mx.TotalAmount < _mx.Amount)
            {

                _toastService.ShowInfo("Total Amount must be greater than Amount");

                _isBusy = false;

                return;
            }
            _model.UserRefId = _userId ?? BState.UserId;

            _model.TranTypeRefId = 67;

            _model.BranchRefId = branchService.BranchId > 0 ? branchService.BranchId.Value : BState.BranchId;

            _model.TransactionTypeName = TransactionTypes.CFX.GetDescription();//"Currency Foreign Exchange";

            c.StaffRefId = _userId ?? BState.UserId;

            c.StaffAuthRefId = _userId ?? BState.UserId;

            c.BranchRefId = branchService.BranchId > 0 ? branchService.BranchId.Value : BState.BranchId;

            c.BankRefId = 11;

            c.SpotRate = _mx.SpotRate;

            c.Remark = _mx.Description;

            c.NameRefId = BState.UserId;

            if (_isBuy)
            {
                c.BuyAmount = _mx.Amount;
                c.SellAmount = _mx.TotalAmount;
                c.BuyCurrencyRefId = 2;
                c.SellCurrencyRefId = 1;
                c.ExchangeRate = _mx.ExchangeRate;
                c.BuyLedgerRefId = 1;
                c.SellLedgerRefId = 2;
                c.GainAndLostAmount = (_mx.Amount * _mx.SpotRate) - _mx.TotalAmount;
                c.FXEPA_Amount = _mx.Amount;//FXEPA_Amount
                c.FXPA_Amount = _mx.Amount * _mx.SpotRate;//FXPA_Amount
                c.FxepaLedgerRefId = FXEPA_USD_ID;//FXEPA_LedgerRefId
                c.FxpaLedgerRefId = FXPA_KHR_ID;//FXPA_LedgerRefId
                c.SpotRate = _mx.SpotRate;
                if (c.GainAndLostAmount > 0)
                {
                    c.GainAndLostLedgerRefId = FXGAIN_KHR_ID;
                }
                else
                {
                    c.GainAndLostLedgerRefId = FXLOSS_KHR_ID;
                }
            }
            else
            {
                c.BuyAmount = _mx.TotalAmount;
                c.SellAmount = _mx.Amount;
                c.BuyCurrencyRefId = 1;
                c.SellCurrencyRefId = 2;
                c.ExchangeRate = _mx.ExchangeRate;
                c.BuyLedgerRefId = 2;
                c.SellLedgerRefId = 1;
                c.GainAndLostAmount = _mx.TotalAmount - (_mx.Amount * _mx.SpotRate);
                c.FXEPA_Amount = _mx.Amount;
                c.FXPA_Amount = _mx.Amount * _mx.SpotRate;
                c.SpotRate = _mx.SpotRate;
                c.FxepaLedgerRefId = FXEPA_USD_ID;
                c.FxpaLedgerRefId = FXPA_KHR_ID;
                c.GainAndLostLedgerRefId = c.GainAndLostAmount > 0 ? FXGAIN_KHR_ID : FXLOSS_KHR_ID;
            }

            _model.ForeignExchanges = c;

            var req = _model.Adapt<LoanTransaction>();


            var result = await exchangeServiceImpl.Exchange(req);

            if (!result.IsSuccess)
            {
                _toastService.ShowError(result.ErrMessage, "Error");

                _isBusy = false;

                return;
            }

            _mx.Amount = 0;

            _mx.TotalAmount = 0;

            _mx.Description = string.Empty;

            _isBusy = false;

            _toastService.ShowSuccess("Currency exchange transaction has been successfully saved.");

            if (!saveNew)
            {
                await ClosePopup.InvokeAsync();
            }

        }
        private async Task ClosingSidebar(MouseEventArgs args)
        {
            await ClosePopup.InvokeAsync();
        }
    }
}