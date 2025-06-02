using Applications.Extensions.Maths;
using Applications.Extensions.NumberExtensions;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.setting;
using Applications.Interfaces.Transactions;
using Applications.Requests.Lists.Currency;
using Applications.Requests.Transactions.ForiegnExchanges;
using Applications.Responses.ViewModels.Currencies;
using Applications.Responses.ViewModels.Options;
using Domains.Models.Settings;
using Domains.Models.Transaction;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Exchanges
{
    public partial class MoneyExchanges
    {
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public string? UserId { get; set; }
        [Parameter] public bool ShowCloseButton { get; set; } = false;
        ForeignExchangeDtoCreate c = new ForeignExchangeDtoCreate();
        TrxForeignExchangeDtoCreate model = new TrxForeignExchangeDtoCreate();
        ExchangeDtoCreate mx = new();
        EditForm form = default!;
        IEnumerable<Items> buyCurrencies = [new Items(2, "US Dollar")];
        IEnumerable<Items> sellCurrencies = [new Items(1, "Khmer Riels")];
        IEnumerable<AppSettings> settings = [];
        VCurrenciesList Currency = new();
        bool isBuy = true;
        bool isBusy = false;
        private readonly ICurrencyService cuService;
        private readonly IAppSettingService settingService;
        private readonly IForiegnCurrencyExchange _exchangeService;

        public MoneyExchanges(ICurrencyService currencyService, IAppSettingService settingsServiceImpl, IForiegnCurrencyExchange exchangeServiceImpl)
        {
            cuService = currencyService;
            settingService = settingsServiceImpl;
            _exchangeService = exchangeServiceImpl;
        }

        protected override async Task OnInitializedAsync()
        {
            var resultSetting = await settingService.GetSysSettings();

            if (resultSetting.IsSuccess)
            {
                settings = resultSetting.Data!;
            }
            mx.SellCurrencyId = 1;

            mx.BuyCurrencyId = 2;

            mx.CurrencyTypeId = 1;

            var currencyResp = await cuService.GetCurrencyAsync(1);

            if (currencyResp.IsSuccess)
            {
                Currency = currencyResp.Data!;
                mx.ExchangeRate = Currency.BuyingRate!.Value;
                mx.SpotRate = Currency.SpotRate!.Value;
            }
            else
            {
                mx.ExchangeRate = 4000;
                mx.SpotRate = 3900;
            }
            isBuy = true;
            await base.OnInitializedAsync();
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

            if (selectedValue == 1)
            {
                isBuy = true;

                mx.ExchangedMode = "Sell Currency";

                mx.ExchangeRate = Currency!.BuyingRate!.Value;
            }
            if (selectedValue == 2)
            {
                isBuy = false;

                mx.ExchangedMode = "Buy Currency";

                mx.ExchangeRate = Currency!.SellingRate!.Value;
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
            model.UserRefId = UserId;
            model.TranTypeRefId = 67;
            model.TransactionTypeName = "Currency Foreign Exchange";
            //c.StaffRefId = UserId;
            //c.StaffAuthRefId = UserId;
            c.BranchRefId = BranchId;
            c.BankRefId = 11;
            c.SpotRate = mx.SpotRate;
            c.Remark = mx.Description;
            if (isBuy)
            {
                c.BuyAmount = mx.Amount;
                c.SellAmount = mx.TotalAmount;
                c.BuyCurrencyRefId = 2;
                c.SellCurrencyRefId = 1;
                c.ExchangeRate = mx.ExchangeRate;
                c.BuyLedgerRefId = 1;
                c.SellLedgerRefId = 2;
                c.GainAndLostAmount = (mx.Amount * mx.SpotRate) - mx.TotalAmount;
                c.FXEPA_Amount = mx.Amount;//FXEPA_Amount
                c.FXPA_Amount = mx.Amount * mx.SpotRate;//FXPA_Amount
                c.FxepaLedgerRefId = FXEPA_USD_ID;//FXEPA_LedgerRefId
                c.FxpaLedgerRefId = FXPA_KHR_ID;//FXPA_LedgerRefId
                c.SpotRate = mx.SpotRate;
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
                c.BuyAmount = mx.TotalAmount;
                c.SellAmount = mx.Amount;
                c.BuyCurrencyRefId = 1;
                c.SellCurrencyRefId = 2;
                c.ExchangeRate = mx.ExchangeRate;
                c.BuyLedgerRefId = 2;
                c.SellLedgerRefId = 1;
                c.GainAndLostAmount = mx.TotalAmount - (mx.Amount * mx.SpotRate);
                c.FXEPA_Amount = mx.Amount;
                c.FXPA_Amount = mx.Amount * mx.SpotRate;
                c.SpotRate = mx.SpotRate;
                c.FxepaLedgerRefId = FXEPA_USD_ID;
                c.FxpaLedgerRefId = FXPA_KHR_ID;
                if (c.GainAndLostAmount > 0)
                {
                    c.GainAndLostLedgerRefId = FXGAIN_KHR_ID;
                }
                else
                {
                    c.GainAndLostLedgerRefId = FXLOSS_KHR_ID;
                }
            }

            model.ForeignExchanges!.Add(c);

            var req = model.Adapt<LoanTransaction>();


            var result = await _exchangeService.Exchange(req);

            if (!result.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
                isBusy = false;
                return;
            }

            mx.Amount = 0;

            mx.TotalAmount = 0;

            mx.Description = string.Empty;

            isBusy = false;

            NotificationService.Notify(NotificationSeverity.Success, result.Data);

            if (!saveNew)
            {
                DialogService.Close(true);
            }

        }

        void Close() => DialogService.Close(false);

    }
}