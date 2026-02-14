using Applications.Enumerations;
using Applications.Extensions.Enums;
using Applications.Extensions.Maths;
using Applications.Extensions.NumberExtensions;
using Applications.Features.Currencies.Command;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.setting;
using Applications.Responses.ViewModels.Options;
using Domains.Enumerations;
using Domains.Models.Settings;
using TrapchumApp.Helpers.Constants;
using TrapchumApp.Helpers.Extensions;
namespace TrapchumApp.Pages.Transactions.Exchanges
{
    public partial class MoneyExchanges
    {
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter] public bool ShowCloseButton { get; set; } = false;
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        ForeignExchangeDtoCreate fcx = new ForeignExchangeDtoCreate();
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
        private string? _userId = string.Empty;
        public MoneyExchanges(ICurrencyService currencyService, IAppSettingService settingsServiceImpl)
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

            mx.SellCurrencyId = (int)CurrencyEnum.KHR;// 1;

            mx.BuyCurrencyId = (int)CurrencyEnum.USD;// 2;

            mx.ActionTypeId = (int)ExchangeAction.BuyCurrency;

            mx.IsBuy = true;

            var rsp = await mediator.Send(new GetCurrencyDetailsByIdQuery(1));

            if (rsp.IsSuccess())
            {
                Currency = rsp.Value;

                mx.ExchangeRate = Currency.BuyingRate;

                mx.SpotRate = Currency.SpotRate;
            }
            else
            {
                mx.ExchangeRate = Defaults.ExchangeRate;

                mx.SpotRate = Defaults.SpotRate;

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

            if (selectedValue == (int)ExchangeAction.BuyCurrency) //1
            {
                isBuy = true;

                mx.IsBuy = true;

                mx.ExchangedMode = "Sell Currency";

                mx.ExchangeRate = Currency!.BuyingRate;
            }

            if (selectedValue == (int) ExchangeAction.SellCurrency)//2
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

            fcx.Transaction = new TransactionBase
            {
                UserRefId = _userId,
                TranTypeRefId = (int)TransactionTypes.CFX,
                TransactionTypeName = TransactionTypes.CFX.Description(),
                BranchRefId = BranchId
            };

            fcx.StaffRefId = _userId;
            fcx.StaffAuthRefId = _userId;
            fcx.BranchRefId = BranchId;
            fcx.BankRefId = 11;
            fcx.SpotRate = mx.SpotRate;
            fcx.Remark = mx.Description;
            if (isBuy)
            {
                fcx.BuyAmount = mx.Amount;
                fcx.IsBuy = mx.IsBuy;
                fcx.SellAmount = mx.TotalAmount;
                fcx.BuyCurrencyRefId = (int)CurrencyEnum.USD; //2;
                fcx.SellCurrencyRefId = (int)CurrencyEnum.KHR;//1;
                fcx.ExchangeRate = mx.ExchangeRate;
                fcx.BuyLedgerRefId = (int)CurrencyEnum.KHR;// 1;
                fcx.SellLedgerRefId = (int)CurrencyEnum.USD;//2
                fcx.GainAndLostAmount = (mx.Amount * mx.SpotRate) - mx.TotalAmount;
                fcx.FXEPA_Amount = mx.Amount;//FXEPA_Amount
                fcx.FXPA_Amount = mx.Amount * mx.SpotRate;//FXPA_Amount
                fcx.FxepaLedgerRefId = FXEPA_USD_ID;//FXEPA_LedgerRefId
                fcx.FxpaLedgerRefId = FXPA_KHR_ID;//FXPA_LedgerRefId
                fcx.SpotRate = mx.SpotRate;
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
                fcx.IsBuy = mx.IsBuy;
                fcx.BuyAmount = mx.TotalAmount;
                fcx.SellAmount = mx.Amount;
                fcx.BuyCurrencyRefId = (int)CurrencyEnum.KHR; //1;
                fcx.SellCurrencyRefId = (int)CurrencyEnum.USD;//2;
                fcx.ExchangeRate = mx.ExchangeRate;
                fcx.BuyLedgerRefId = (int)CurrencyEnum.USD;//2;
                fcx.SellLedgerRefId = (int)CurrencyEnum.KHR;//1;
                fcx.GainAndLostAmount = mx.TotalAmount - (mx.Amount * mx.SpotRate);
                fcx.FXEPA_Amount = mx.Amount;
                fcx.FXPA_Amount = mx.Amount * mx.SpotRate;
                fcx.SpotRate = mx.SpotRate;
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
                NotificationService.Notify(NotificationSeverity.Error, result.GetMessage());

                isBusy = false;

                return;
            }

            mx.Amount = 0;

            mx.TotalAmount = 0;

            mx.Description = string.Empty;

            isBusy = false;

            NotificationService.Notify(NotificationSeverity.Success, "Exchange created successfully.");

            DialogService.Close(result.Value);

        }

        void Close() => DialogService.Close(null);

    }
}