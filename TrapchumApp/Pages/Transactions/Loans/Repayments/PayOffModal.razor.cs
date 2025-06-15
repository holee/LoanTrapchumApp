using Applications.Calculations;
using Applications.Extensions.Maths;
using Applications.Extensions.NumberExtensions;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Payments;
using Applications.Requests.Transactions.ForiegnExchanges;
using Applications.Requests.Transactions.LoanTransactions;
using Applications.Responses.ViewModels.Currencies;
using Applications.Services.Settings;
using Domains.Models.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments
{
    public partial class PayOffModal
    {
        private readonly ISettingServiceImpl _settingService;
        private readonly ILoanPaymentService _repaymentService;
        public PayOffModal(ISettingServiceImpl settingService, ILoanPaymentService repaymentService)
        {
            _settingService = settingService;
            _repaymentService = repaymentService;
        }

        [Parameter] public int CurrencyId { get; set; }
        [Parameter] public string? CreditAgentId { get; set; }
        [Parameter] public string? UserId { get; set; }
        [Parameter] public string? LoanId { get; set; }
        [Parameter] public int BranchId { get; set; }
        [Parameter] public string CustomerName { get; set; } = string.Empty;
        [Parameter] public string ReceiptNo { get; set; } = string.Empty;
        [Parameter] public List<LoanPaymentLineDtoCreate> LoanRepaymentLines { get; set; } = [];
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        VCurrenciesList? selectedCurrency;
        TrxForeignExchangeDtoCreate model = new TrxForeignExchangeDtoCreate();
        ForeignExchangeDtoCreate c = new ForeignExchangeDtoCreate();
        LoanPaymentDtoCreate LoanRepayment { get; set; } = new();
        IEnumerable<VCurrenciesList> Currencies = [];
        IEnumerable<AppSettings> settings = [];
        decimal? receiveAmountUsd = 0;
        decimal? receiveAmountKhr = 0;
        decimal? changedAmountUSD = 0;
        decimal? changedAmountKHR = 0;
        decimal? penaltyAmount = 0;
        decimal exchangeRate = 0;
        decimal? buyAmount = 0;
        decimal? sellAmount = 0;
        decimal? spotRateAmount = 0;
        decimal? spotRateExhcangeAmount = 0;
        decimal? buyExchangeAmount = 0;
        decimal? sellExchangeAmount = 0;
        decimal? TotalPaidAmount = 0;
        decimal? originalExchangeAmountUSD = 0;
        decimal? originalExchangeAmountKHR = 0;
        decimal? gainOrLostAmount = 0;
        decimal? buyingRate = 0;
        decimal? sellingRate = 0;
        decimal? spotRate = 0;
        decimal? exchangedSpotRateAmount = 0;
        decimal? exchangeGainAndLostAmount = 0;
        decimal? originalPaidAmount = 0;
        decimal? originalPenaltyAmount = 0;
        decimal? FXEPA_Amount = 0;
        decimal? FXPA_Amount = 0;
        int gainLedgerRefId = 0;
        int lostLedgerRefId = 0;
        int buyLedgerRefId = 0;
        int sellLedgerRefId = 0;
        int buyCurrencyRefId = 0;
        int sellCurrencyRefId = 0;
        bool isChangeAvailable = false;
        bool isSelled = false;
        bool isBuy = true;
        bool isBusy = false;
        bool disableButton = true;
        bool isPaymentExchanged = false;
        bool isExchangedChanged = false;
        protected override async Task OnParametersSetAsync()
        {
            var currencyResponse = await _curService.GetCurrencyAsync(1);
            if (currencyResponse.IsSuccess)
            {
                selectedCurrency = currencyResponse.Data!;
                buyingRate = currencyResponse.Data!.BuyingRate;
                sellingRate = currencyResponse.Data!.SellingRate;
                spotRate = currencyResponse.Data!.SpotRate;
            }
            var resultSetting = await _settingService.GetSysSettings();

            if (resultSetting.IsSuccess)
            {
                settings = resultSetting.Data!;
                gainLedgerRefId = settings.FirstOrDefault(p => p.Code == "FXGAIN-KHR")!.Value.ToNumer<int>();
                lostLedgerRefId = settings.FirstOrDefault(p => p.Code == "FXLOSS-KHR")!.Value.ToNumer<int>();
                buyLedgerRefId = settings.FirstOrDefault(p => p.Code == "FXEPA-USD")!.Value.ToNumer<int>();
                sellLedgerRefId = settings.FirstOrDefault(p => p.Code == "FXPA-KHR")!.Value.ToNumer<int>();
            }
            await base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            if (LoanRepaymentLines.Any())
            {
                penaltyAmount = LoanRepaymentLines.Sum(x => x.PaidPenaltyAmount);
                originalPenaltyAmount = penaltyAmount;
                TotalPaidAmount = LoanRepaymentLines.Total();
                originalPaidAmount = LoanRepaymentLines.TotalAmountWithPenalty();
            }
            if (CurrencyId == 1)
            {
                receiveAmountKhr = TotalPaidAmount;
            }
            else
            {
                receiveAmountUsd = TotalPaidAmount;
            }
            await base.OnInitializedAsync();
        }
        void CloseModal()
        {
            DialogService.Close(false);
        }
        async Task OnSaveChanges()
        {
            try
            {
                if (receiveAmountKhr <= 0 && receiveAmountUsd <= 0)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Please provide payment amount.");
                    return;
                }
                var tran = new LoanPaymentTransactionDtoCreate
                {
                    TransactionDate = DateTime.Now,
                    TransactionTypeName = "Loan Principal Repayment",
                    TranTypeRefId = 20,
                    UserRefId = UserId
                };
                LoanRepayment.LoanRefId = LoanId;
                LoanRepayment.CreditAgentRefId = CreditAgentId;
                LoanRepayment.PayDate = DateTime.Now;
                LoanRepayment.Principle = LoanRepaymentLines.TotalPaidPrinciple();
                LoanRepayment.Interest = LoanRepaymentLines.TotalPaidInterest();
                LoanRepayment.CollectionFeeAmount = LoanRepaymentLines.TotalPaidCollectionFee();
                LoanRepayment.PenaltyAmount = LoanRepaymentLines.TotalPaidPenalty();
                LoanRepayment.SubTotal = LoanRepaymentLines.Total();
                LoanRepayment.OfficialReceiptNo = ReceiptNo;
                LoanRepayment.LoanPaymentLines = LoanRepaymentLines;
                //LoanRepayment.LoanTransaction = tran;
                LoanRepayment.BranchRefId = BranchId;
                LoanRepayment.UserRefId = UserId;
                LoanRepayment.CurrencyRefId = CurrencyId;
                if (TotalPaidAmount > 0)
                {
                    var tranxCashManagement = new TrxCashManagementDtoCreate()
                    {
                        CurrencyRefId = CurrencyId,
                        TransactionDate = DateTime.Now,
                        BusinessDate = DateTime.Now,
                        CashStatusRefId = 2,
                        BankRefId = 11,
                        //StaffRefId = UserId,
                        Amount = TotalPaidAmount,
                    };
                    //LoanRepayment.LoanTransaction!.CashManagements!.Add(tranxCashManagement);
                }
                if (isPaymentExchanged)
                {
                    var currencyExchanges = new ForeignExchangeDtoCreate
                    {
                        //StaffRefId = UserId,
                        //StaffAuthRefId = UserId,
                        BranchRefId = BranchId,
                        BankRefId = 11,
                        BuyAmount = buyAmount,
                        SellAmount = sellAmount,
                        BuyCurrencyRefId = buyCurrencyRefId,
                        SellCurrencyRefId = sellCurrencyRefId,
                        ExchangeRate = exchangeRate,
                        BuyLedgerRefId = buyLedgerRefId,
                        SellLedgerRefId = sellLedgerRefId,
                        GainAndLostAmount = gainOrLostAmount,
                        GainAndLostLedgerRefId = gainOrLostAmount < 0 ? lostLedgerRefId : gainLedgerRefId,
                        SpotRate = selectedCurrency!.SpotRate,
                    };
                    model.ForeignExchanges!.Add(currencyExchanges);
                }
                if (isExchangedChanged)
                {
                    var currencyExchanges = new ForeignExchangeDtoCreate
                    {
                        //StaffRefId = UserId,
                        //StaffAuthRefId = UserId,
                        BankRefId = 11,
                        BuyAmount = buyExchangeAmount,
                        BranchRefId = BranchId,
                        SellAmount = sellExchangeAmount,
                        BuyCurrencyRefId = 1,
                        SellCurrencyRefId = 2,
                        ExchangeRate = exchangeRate,
                        BuyLedgerRefId = buyLedgerRefId,
                        SellLedgerRefId = sellLedgerRefId,
                        GainAndLostAmount = gainOrLostAmount,
                        GainAndLostLedgerRefId = gainOrLostAmount < 0 ? lostLedgerRefId : gainLedgerRefId,
                        SpotRate = spotRate
                    };
                    model.ForeignExchanges!.Add(currencyExchanges);
                }
                var confirmOption = new ConfirmOptions
                {
                    CancelButtonText = "Cancel",
                    OkButtonText = "Yes",
                };

                string message = $"Receive money from {CustomerName} with amount {TotalPaidAmount}";
                var confirmPayment = await DialogService.Confirm(message, "Payment", confirmOption);

                if (confirmPayment == false) return;

                model.UserRefId = UserId;
                model.TranTypeRefId = 67;
                model.TransactionTypeName = "Currency Foreign Exchange";
                //LoanRepayment.ForeignExchangeTrx = Model;
                var result = await _repaymentService.RecievePayment(LoanRepayment);

                if (result.IsSuccess)
                {
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, result.Data);
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }

        void ReceivePaymentUSDChanged(decimal? amount)
        {
            if (amount < 0 && TotalPaidAmount <= 0)
            {
                receiveAmountUsd = 0;
                receiveAmountKhr = 0;
                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                return;
            }
            if ((CurrencyId == 1 || CurrencyId == 2) && amount < 0 && TotalPaidAmount > 0)
            {
                receiveAmountKhr = 0;
                changedAmountKHR = 0;
                receiveAmountUsd = 0;
                changedAmountUSD = 0;
                isPaymentExchanged = false;
                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                return;
            }
            ///no currency exchange occur
            if (CurrencyId == 2 && amount > 0 && amount >= TotalPaidAmount)
            {
                changedAmountUSD = amount - TotalPaidAmount;
                originalExchangeAmountUSD = changedAmountUSD;
                receiveAmountKhr = 0;
                changedAmountKHR = 0;
                originalExchangeAmountKHR = 0;
                isPaymentExchanged = false;
                buyExchangeAmount = 0;
                sellExchangeAmount = 0;
                gainOrLostAmount = 0;
                spotRateAmount = 0;
                buyAmount = 0;
                sellAmount = 0;
                isChangeAvailable = changedAmountUSD > 0 ? true : false;
                return;
            }


            //curency exchange occur
            if (CurrencyId == 2 && amount > 0 && amount < TotalPaidAmount)
            {
                //sell usd to customer
                var sellUsdAmount = TotalPaidAmount - amount;
                //sell dollar to customer and buy riels from  customer
                var totalKhrAmount = (sellUsdAmount * sellingRate).RoundUpKhr(2);
                //get spot rate amount
                spotRateAmount = sellUsdAmount * spotRate;
                //get gain or lost amount
                gainOrLostAmount = totalKhrAmount - spotRateAmount;
                sellAmount = sellUsdAmount;
                buyAmount = receiveAmountKhr = totalKhrAmount;
                exchangeRate = sellingRate!.Value;
                isPaymentExchanged = true;
                changedAmountUSD = 0;
                changedAmountKHR = 0;
                originalExchangeAmountKHR = 0;
                originalExchangeAmountUSD = 0;
                return;
            }

            //curency exchange occur
            if (CurrencyId == 2 && amount == 0 && TotalPaidAmount > 0)
            {

                var totalKhrAmount = (TotalPaidAmount * sellingRate).RoundUpKhr(2);
                //sell dollar to customer and buy riels from  customer
                sellAmount = TotalPaidAmount;
                sellCurrencyRefId = 2;
                //buy amount
                buyAmount = receiveAmountKhr = totalKhrAmount;
                buyCurrencyRefId = 1;
                //get spot rate amount
                spotRateAmount = TotalPaidAmount * spotRate;
                //get gain or lost amount
                gainOrLostAmount = totalKhrAmount - spotRateAmount;
                exchangeRate = sellingRate!.Value;
                isPaymentExchanged = true;
                changedAmountUSD = 0;
                changedAmountKHR = 0;
                originalExchangeAmountKHR = 0;
                originalExchangeAmountUSD = 0;
                return;
            }

            //Payment currency is riels

            if (CurrencyId == 1 && amount < 0)
            {
                RestoreDefault();
                return;
            }
            if (CurrencyId == 1 && (amount == 0 || receiveAmountUsd == 0))
            {
                receiveAmountKhr = TotalPaidAmount;
                changedAmountKHR = 0;
                changedAmountUSD = 0;
                buyAmount = 0;
                sellAmount = 0;
                gainOrLostAmount = 0;
                exchangeGainAndLostAmount = 0;
                receiveAmountUsd = 0;
                spotRate = 0;
                return;
            }

            if (CurrencyId == 1 && amount > 0 && TotalPaidAmount > 0)
            {
                //buy us dollar
                var totalKhrAmount = (amount * buyingRate).RoundDownKhr(2);
                if (totalKhrAmount < TotalPaidAmount)
                {
                    //amount is dollar
                    buyAmount = amount;
                    buyCurrencyRefId = 2;
                    //calculate left khmer riels  amount
                    receiveAmountKhr = TotalPaidAmount - totalKhrAmount;
                    //calculate selling amount
                    sellAmount = totalKhrAmount;
                    sellCurrencyRefId = 1;
                    //Calculate for spot rate amount
                    spotRateAmount = amount * spotRate;
                    //Calculate for gain and lost
                    gainOrLostAmount = spotRateAmount - totalKhrAmount;
                    //set exchange rate
                    exchangeRate = buyingRate!.Value;
                    changedAmountKHR = 0;
                    changedAmountUSD = 0;
                    isPaymentExchanged = true;
                    return;
                }
                else
                {

                    receiveAmountKhr = 0;
                    changedAmountKHR = totalKhrAmount - TotalPaidAmount;
                    originalExchangeAmountKHR = changedAmountKHR;
                    changedAmountUSD = 0;
                    //assign buy usd amount
                    buyAmount = amount;
                    buyCurrencyRefId = 2;
                    //assign khr amount.
                    sellAmount = totalKhrAmount;
                    sellCurrencyRefId = 1;
                    //calculate spot rate amount.
                    spotRateAmount = amount * spotRate;
                    //calculate gain and lost
                    gainOrLostAmount = spotRateAmount - TotalPaidAmount;
                    //set exchange rate
                    exchangeRate = buyingRate!.Value;
                    isPaymentExchanged = true;
                    return;
                }
            }
        }
        void ReceivePaymentUSDChanged(ChangeEventArgs e)
        {
            decimal amount = e.Value!.ToNumer<decimal>();

            if (amount < 0 && TotalPaidAmount <= 0)
            {
                receiveAmountUsd = 0;
                receiveAmountKhr = 0;
                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                return;
            }
            if ((CurrencyId == 1 || CurrencyId == 2) && amount < 0 && TotalPaidAmount > 0)
            {
                receiveAmountKhr = 0;
                changedAmountKHR = 0;
                receiveAmountUsd = 0;
                changedAmountUSD = 0;
                isPaymentExchanged = false;
                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                return;
            }
            ///no currency exchange occur
            if (CurrencyId == 2 && amount > 0 && amount >= TotalPaidAmount)
            {
                changedAmountUSD = amount - TotalPaidAmount;
                originalExchangeAmountUSD = changedAmountUSD;
                receiveAmountKhr = 0;
                changedAmountKHR = 0;
                originalExchangeAmountKHR = 0;
                isPaymentExchanged = false;
                buyExchangeAmount = 0;
                sellExchangeAmount = 0;
                gainOrLostAmount = 0;
                spotRateAmount = 0;
                buyAmount = 0;
                sellAmount = 0;
                isChangeAvailable = changedAmountUSD > 0 ? true : false;
                return;
            }


            //curency exchange occur
            if (CurrencyId == 2 && amount > 0 && amount < TotalPaidAmount)
            {
                //sell usd to customer
                var sellUsdAmount = TotalPaidAmount - amount;
                //sell dollar to customer and buy riels from  customer
                var totalKhrAmount = (sellUsdAmount * sellingRate).RoundUpKhr(2);
                //get spot rate amount
                spotRateAmount = sellUsdAmount * spotRate;
                //get gain or lost amount
                gainOrLostAmount = totalKhrAmount - spotRateAmount;
                sellAmount = sellUsdAmount;
                buyAmount = receiveAmountKhr = totalKhrAmount;
                exchangeRate = sellingRate!.Value;
                isPaymentExchanged = true;
                changedAmountUSD = 0;
                changedAmountKHR = 0;
                originalExchangeAmountKHR = 0;
                originalExchangeAmountUSD = 0;
                return;
            }

            //curency exchange occur
            if (CurrencyId == 2 && amount == 0 && TotalPaidAmount > 0)
            {

                var totalKhrAmount = (TotalPaidAmount * sellingRate).RoundUpKhr(2);
                //sell dollar to customer and buy riels from  customer
                sellAmount = TotalPaidAmount;
                sellCurrencyRefId = 2;
                //buy amount
                buyAmount = receiveAmountKhr = totalKhrAmount;
                buyCurrencyRefId = 1;
                //get spot rate amount
                spotRateAmount = TotalPaidAmount * spotRate;
                //get gain or lost amount
                gainOrLostAmount = totalKhrAmount - spotRateAmount;
                exchangeRate = sellingRate!.Value;
                isPaymentExchanged = true;
                changedAmountUSD = 0;
                changedAmountKHR = 0;
                originalExchangeAmountKHR = 0;
                originalExchangeAmountUSD = 0;
                return;
            }

            //Payment currency is riels

            if (CurrencyId == 1 && amount < 0)
            {
                RestoreDefault();
                return;
            }
            if (CurrencyId == 1 && (amount == 0 || receiveAmountUsd == 0))
            {
                receiveAmountKhr = TotalPaidAmount;
                changedAmountKHR = 0;
                changedAmountUSD = 0;
                buyAmount = 0;
                sellAmount = 0;
                gainOrLostAmount = 0;
                exchangeGainAndLostAmount = 0;
                receiveAmountUsd = 0;
                spotRate = 0;
                return;
            }

            if (CurrencyId == 1 && amount > 0 && TotalPaidAmount > 0)
            {
                //buy us dollar
                var totalKhrAmount = (amount * buyingRate).RoundDownKhr(2);
                if (totalKhrAmount < TotalPaidAmount)
                {
                    //amount is dollar
                    buyAmount = amount;
                    buyCurrencyRefId = 2;
                    //calculate left khmer riels  amount
                    receiveAmountKhr = TotalPaidAmount - totalKhrAmount;
                    //calculate selling amount
                    sellAmount = totalKhrAmount;
                    sellCurrencyRefId = 1;
                    //Calculate for spot rate amount
                    spotRateAmount = amount * spotRate;
                    //Calculate for gain and lost
                    gainOrLostAmount = spotRateAmount - totalKhrAmount;
                    //set exchange rate
                    exchangeRate = buyingRate!.Value;
                    changedAmountKHR = 0;
                    changedAmountUSD = 0;
                    isPaymentExchanged = true;
                    return;
                }
                else
                {

                    receiveAmountKhr = 0;
                    changedAmountKHR = totalKhrAmount - TotalPaidAmount;
                    originalExchangeAmountKHR = changedAmountKHR;
                    changedAmountUSD = 0;
                    //assign buy usd amount
                    buyAmount = amount;
                    buyCurrencyRefId = 2;
                    //assign khr amount.
                    sellAmount = totalKhrAmount;
                    sellCurrencyRefId = 1;
                    //calculate spot rate amount.
                    spotRateAmount = amount * spotRate;
                    //calculate gain and lost
                    gainOrLostAmount = spotRateAmount - TotalPaidAmount;
                    //set exchange rate
                    exchangeRate = buyingRate!.Value;
                    isPaymentExchanged = true;
                    return;
                }
            }
        }
        void ReceivePaymentKHRChanged(decimal? amount)
        {
            if (amount < 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                SetDefualt();
                return;
            }
            //no exchange occure
            if (CurrencyId == 1 && amount >= 0 && amount >= TotalPaidAmount)
            {

                changedAmountKHR = amount - TotalPaidAmount;
                originalExchangeAmountKHR = amount - TotalPaidAmount;
                isPaymentExchanged = false;
                receiveAmountUsd = 0;
                changedAmountUSD = 0;
                gainOrLostAmount = 0;
                exchangeGainAndLostAmount = 0;
                return;
            }

            if (CurrencyId == 1 && amount >= 0 && amount < TotalPaidAmount)
            {
                var leftKhrAmount = TotalPaidAmount - amount;
                //buy dollar from customer
                var buyAmount = (leftKhrAmount / buyingRate).RoundUsd(2);
                receiveAmountUsd = buyAmount;
                buyCurrencyRefId = 2;
                //set sell amount
                sellAmount = leftKhrAmount;
                sellCurrencyRefId = 1;
                //set spot rate amoun
                spotRateAmount = buyAmount * spotRate;
                //set gain or lost amount
                gainOrLostAmount = spotRateAmount - leftKhrAmount;
                changedAmountUSD = 0;
                changedAmountKHR = 0;
                isPaymentExchanged = true;
                return;
            }

        }
        void ReceivePaymentKHRChanged(ChangeEventArgs e)
        {
            var amount = e.Value!.ToNumer<decimal>();

            if (amount < 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                SetDefualt();
                return;
            }
            //no exchange occure
            if (CurrencyId == 1 && amount >= 0 && amount >= TotalPaidAmount)
            {

                changedAmountKHR = amount - TotalPaidAmount;
                originalExchangeAmountKHR = amount - TotalPaidAmount;
                isPaymentExchanged = false;
                receiveAmountUsd = 0;
                changedAmountUSD = 0;
                gainOrLostAmount = 0;
                exchangeGainAndLostAmount = 0;
                return;
            }

            if (CurrencyId == 1 && amount >= 0 && amount < TotalPaidAmount)
            {
                var leftKhrAmount = TotalPaidAmount - amount;
                //buy dollar from customer
                var buyAmount = (leftKhrAmount / buyingRate).RoundUsd(2);
                receiveAmountUsd = buyAmount;
                buyCurrencyRefId = 2;
                //set sell amount
                sellAmount = leftKhrAmount;
                sellCurrencyRefId = 1;
                //set spot rate amoun
                spotRateAmount = buyAmount * spotRate;
                //set gain or lost amount
                gainOrLostAmount = spotRateAmount - leftKhrAmount;
                changedAmountUSD = 0;
                changedAmountKHR = 0;
                isPaymentExchanged = true;
                return;
            }

        }
        void TextUsdAmountChanged(decimal? amount)
        {

            if (amount > 0 && originalExchangeAmountUSD == 0 && originalExchangeAmountKHR == 0)
            {
                changedAmountUSD = 0;
                changedAmountKHR = 0;
                amount = 0;
                isExchangedChanged = false;
                return;
            }
            if ((amount < 0 && originalExchangeAmountUSD > 0 && originalExchangeAmountKHR <= 0) ||
               (amount < 0 && originalExchangeAmountKHR > 0 && originalExchangeAmountUSD <= 0) ||
                (originalExchangeAmountUSD > 0 && amount > originalExchangeAmountUSD) ||
                (originalExchangeAmountKHR > 0 && amount > originalExchangeAmountKHR))
            {
                changedAmountUSD = originalExchangeAmountUSD;
                changedAmountKHR = originalExchangeAmountKHR;
                isExchangedChanged = false;
                amount = 0;
                NotificationService.Notify(NotificationSeverity.Info, "please verify amount again.");
                return;
            }
            if (originalExchangeAmountKHR > 0 && originalExchangeAmountKHR < (10 * sellingRate))
            {
                changedAmountUSD = 0;
                changedAmountKHR = originalExchangeAmountKHR;
                NotificationService.Notify(NotificationSeverity.Info, "You dont have enough money to exchange.");
                return;
            }

            if (originalExchangeAmountKHR > 0 && originalExchangeAmountUSD == 0)
            {
                //Sell dollar to customer
                var KhrAmount = (amount * sellingRate).RoundUpKhr(2);

                if (KhrAmount > originalExchangeAmountKHR)
                {
                    changedAmountKHR = originalExchangeAmountKHR;
                    changedAmountUSD = 0;
                    sellExchangeAmount = 0;
                    buyExchangeAmount = 0;
                    exchangedSpotRateAmount = 0;
                    exchangeGainAndLostAmount = 0;
                    amount = 0;
                    isExchangedChanged = false;
                    return;
                }
                else
                {
                    changedAmountKHR = originalExchangeAmountKHR - KhrAmount;
                    //set sell currency amount
                    sellExchangeAmount = amount;
                    sellCurrencyRefId = 2;
                    //set buy khr amount from customer
                    buyExchangeAmount = KhrAmount;
                    buyCurrencyRefId = 1;
                    //calculate spot amount
                    exchangedSpotRateAmount = amount * spotRate;
                    //set exchange gain and lost
                    exchangeGainAndLostAmount = KhrAmount - exchangedSpotRateAmount;
                    isExchangedChanged = true;
                    return;
                }
            }
            if (originalExchangeAmountUSD > 0 && originalExchangeAmountKHR == 0)
            {
                //buy dollar from customer
                changedAmountKHR = ((originalExchangeAmountUSD - amount) * buyingRate).RoundDownKhr(2);
                sellExchangeAmount = changedAmountKHR;
                sellCurrencyRefId = 1;
                //buy dollar
                buyExchangeAmount = originalExchangeAmountUSD - amount;
                buyCurrencyRefId = 2;
                //Spot amount
                spotRateExhcangeAmount = buyExchangeAmount * spotRate;
                exchangeGainAndLostAmount = changedAmountKHR - spotRateExhcangeAmount;
                isExchangedChanged = true;
                return;
            }


        }
        void TextUsdAmountChanged(ChangeEventArgs e)
        {
            var amount = e.Value!.ToNumer<decimal>();

            if (amount > 0 && originalExchangeAmountUSD == 0 && originalExchangeAmountKHR == 0)
            {
                changedAmountUSD = 0;
                changedAmountKHR = 0;
                amount = 0;
                isExchangedChanged = false;
                return;
            }
            if ((amount < 0 && originalExchangeAmountUSD > 0 && originalExchangeAmountKHR <= 0) ||
               (amount < 0 && originalExchangeAmountKHR > 0 && originalExchangeAmountUSD <= 0) ||
                (originalExchangeAmountUSD > 0 && amount > originalExchangeAmountUSD) ||
                (originalExchangeAmountKHR > 0 && amount > originalExchangeAmountKHR))
            {
                changedAmountUSD = originalExchangeAmountUSD;
                changedAmountKHR = originalExchangeAmountKHR;
                isExchangedChanged = false;
                amount = 0;
                NotificationService.Notify(NotificationSeverity.Info, "please verify amount again.");
                return;
            }
            if (originalExchangeAmountKHR > 0 && originalExchangeAmountKHR < (10 * sellingRate))
            {
                changedAmountUSD = 0;
                changedAmountKHR = originalExchangeAmountKHR;
                NotificationService.Notify(NotificationSeverity.Info, "You dont have enough money to exchange.");
                return;
            }

            if (originalExchangeAmountKHR > 0 && originalExchangeAmountUSD == 0)
            {
                //Sell dollar to customer
                var KhrAmount = (amount * sellingRate).RoundUpKhr(2);

                if (KhrAmount > originalExchangeAmountKHR)
                {
                    changedAmountKHR = originalExchangeAmountKHR;
                    changedAmountUSD = 0;
                    sellExchangeAmount = 0;
                    buyExchangeAmount = 0;
                    exchangedSpotRateAmount = 0;
                    exchangeGainAndLostAmount = 0;
                    amount = 0;
                    isExchangedChanged = false;
                    return;
                }
                else
                {
                    changedAmountKHR = originalExchangeAmountKHR - KhrAmount;
                    //set sell currency amount
                    sellExchangeAmount = amount;
                    sellCurrencyRefId = 2;
                    //set buy khr amount from customer
                    buyExchangeAmount = KhrAmount;
                    buyCurrencyRefId = 1;
                    //calculate spot amount
                    exchangedSpotRateAmount = amount * spotRate;
                    //set exchange gain and lost
                    exchangeGainAndLostAmount = KhrAmount - exchangedSpotRateAmount;
                    isExchangedChanged = true;
                    return;
                }
            }
            if (originalExchangeAmountUSD > 0 && originalExchangeAmountKHR == 0)
            {
                //buy dollar from customer
                changedAmountKHR = ((originalExchangeAmountUSD - amount) * buyingRate).RoundDownKhr(2);
                sellExchangeAmount = changedAmountKHR;
                sellCurrencyRefId = 1;
                //buy dollar
                buyExchangeAmount = originalExchangeAmountUSD - amount;
                buyCurrencyRefId = 2;
                //Spot amount
                spotRateExhcangeAmount = buyExchangeAmount * spotRate;
                exchangeGainAndLostAmount = changedAmountKHR - spotRateExhcangeAmount;
                isExchangedChanged = true;
                return;
            }


        }
        void CalculatePenaltyAmount(decimal? amount)
        {
            if (amount < 0)
            {
                penaltyAmount = originalPenaltyAmount;
                return;
            }
            if (amount > originalPenaltyAmount)
            {
                NotificationService.Notify(NotificationSeverity.Error, "input amount must be less than penalty amount.");
                RestoreDefault();
                return;
            }
            else
            {
                TotalPaidAmount = originalPaidAmount + penaltyAmount;
                SetDefualt();
            }

        }
        void CalculatePenaltyAmount(ChangeEventArgs e)
        {

            var amount = e.Value?.ToNumer<decimal>();

            if (amount < 0)
            {
                penaltyAmount = originalPenaltyAmount;
                return;
            }
            if (amount > originalPenaltyAmount)
            {
                NotificationService.Notify(NotificationSeverity.Error, "input amount must be less than penalty amount.");
                RestoreDefault();
                return;
            }
            else
            {
                TotalPaidAmount = originalPaidAmount + penaltyAmount;
                SetDefualt();
            }

        }
        void SetDefualt()
        {
            changedAmountKHR = 0.00M;
            changedAmountUSD = 0.00M;
            receiveAmountKhr = 0.00M;
            receiveAmountUsd = 0.00M;
            changedAmountKHR = 0.00M;
            changedAmountUSD = 0.00M;
            receiveAmountKhr = 0.00M;
            receiveAmountUsd = 0.00M;
            buyAmount = 0;
            sellAmount = 0;
            buyExchangeAmount = 0;
            sellExchangeAmount = 0;
            spotRateAmount = 0;
        }
        void RestoreDefault()
        {
            changedAmountKHR = 0.00M;
            changedAmountUSD = 0.00M;
            changedAmountKHR = 0.00M;
            changedAmountUSD = 0.00M;
            buyAmount = 0;
            sellAmount = 0;
            buyExchangeAmount = 0;
            sellExchangeAmount = 0;
            spotRateAmount = 0;
            TotalPaidAmount = originalPaidAmount + originalPenaltyAmount;
            penaltyAmount = originalPenaltyAmount;
            gainOrLostAmount = 0;
            exchangeGainAndLostAmount = 0;
            if (CurrencyId == 1)
            {
                receiveAmountKhr = TotalPaidAmount;
                receiveAmountUsd = 0.00M;
            }
            else
            {
                receiveAmountUsd = TotalPaidAmount;
                receiveAmountKhr = 0.00M;
            }
        }
    }
}