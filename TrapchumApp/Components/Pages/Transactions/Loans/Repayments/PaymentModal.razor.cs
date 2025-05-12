using Applications.Calculations;
using Applications.Extensions.NumberExtensions;
using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Payments;
using Applications.Requests.Transactions.ForiegnExchanges;
using Applications.Requests.Transactions.LoanTransactions;
using Applications.Responses.ViewModels.Currencies;
using Applications.Responses.ViewModels.Settings;
using Applications.Services.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
namespace TrapchumApp.Components.Pages.Transactions.Loans.Repayments
{
    public partial class PaymentModal
    {
        private readonly ISettingServiceImpl _settingService;

        private readonly ILoanPaymentService _loanPaymentService;
        public PaymentModal(ISettingServiceImpl settingService, ILoanPaymentService loanPaymentService)
        {
            _settingService = settingService;
            _loanPaymentService = loanPaymentService;
        }

        [Parameter] public int CurrencyId { get; set; }
        [Parameter] public string? CreditAgentId { get; set; }
        [Parameter] public string? UserId { get; set; }
        [Parameter] public string? LoanId { get; set; }
        [Parameter] public int BranchId { get; set; }
        [Parameter] public string CustomerName { get; set; } = string.Empty;
        [Parameter] public string ReceiptNo { get; set; } = string.Empty;
        [Parameter] public PaymentCreate PaymentCreate { get; set; } = new();
        [Parameter] public List<LoanRepaymentLineDtoCreate> LoanRepaymentLines { get; set; } = [];
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;

        VCurrenciesList? selectedCurrency;

        TrxForeignExchangeDtoCreate model = new TrxForeignExchangeDtoCreate();

        ForeignExchangeDtoCreate c = new ForeignExchangeDtoCreate();
        LoanRepaymentDtoCreate LoanRepayment { get; set; } = new();

        IEnumerable<VCurrenciesList> Currencies = [];

        IEnumerable<AppSettingVms> settings = [];

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
        bool isBusy = false; 
        protected override async Task OnInitializedAsync()
        {
            var currencyResponse = await _curService.GetCurrencyAsync(1);

            if (currencyResponse.IsSuccess)
            {
                selectedCurrency = currencyResponse.Data!;
                //buyingRate = currencyResponse.Data!.BuyingRate;
                //sellingRate = currencyResponse.Data!.SellingRate;
                //spotRate = currencyResponse.Data!.SpotRate;
            }

            
            await base.OnInitializedAsync();
        }


        void CloseModal()
        {
            DialogService.Close(false);
        }


        async Task OnSaveChanges()
        {
            var confirmOption = new ConfirmOptions
            {
                CancelButtonText = "Cancel",
                OkButtonText = "Yes",
            };
            try
            {
                isBusy = true;

                if (PaymentCreate.RecieveKhrAmount <= 0 && PaymentCreate.RecieveAmount <= 0)
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Please provide payment amount.");
                    return;
                }
                var tran = new LoanTransactionDtoCreate
                {
                    TransactionDate = DateTime.Now,
                    TransactionTypeName = "Loan Principal Repayment",
                    TranTypeRefId = 20,
                    UserRefId = UserId
                };
                LoanRepayment.LoanRefId = LoanId;
                LoanRepayment.CreditAgentRefId = CreditAgentId;
                LoanRepayment.PayDate = DateTime.Now;
                LoanRepayment.Principle = LoanRepaymentLines.TotalPaidPrincipleAmount();
                LoanRepayment.Interest = LoanRepaymentLines.TotalPaidInterestAmount();
                LoanRepayment.CollectionFeeAmount = LoanRepaymentLines.TotalPaidCollectionFeeAmount();
                LoanRepayment.PenaltyAmount = LoanRepaymentLines.TotalPaidPenaltyAmount();
                LoanRepayment.SubTotal = LoanRepaymentLines.Total();
                LoanRepayment.OfficialReceiptNo = ReceiptNo;
                LoanRepayment.LoanRepaymentLines = LoanRepaymentLines;
                LoanRepayment.LoanTransaction = tran;
                LoanRepayment.BranchRefId = BranchId;
                LoanRepayment.UserRefId = UserId;
                LoanRepayment.CurrencyRefId = CurrencyId;
                if (PaymentCreate.TotalAmountWithPenalty > 0)
                {
                    var tranxCashManagement = new TrxCashManagementDtoCreate()
                    {
                        Id = $"trx-{Guid.CreateVersion7()}",
                        CurrencyRefId = CurrencyId,
                        TransactionDate = DateTime.Now,
                        BusinessDate = DateTime.Now,
                        CashStatusRefId = 2,
                        BankRefId = 11,
                        Amount = PaymentCreate.TotalAmountWithPenalty,
                    };
                    LoanRepayment.LoanTransaction!.CashManagements!.Add(tranxCashManagement);
                }

                string message = $"Receive money from {CustomerName} with amount {PaymentCreate.TotalAmountWithPenalty}";

                var confirmPayment = await DialogService.Confirm(message, "Payment", confirmOption);

                if (confirmPayment == false) return;

                //model.UserRefId = UserId;

                //model.TranTypeRefId = 67;

                //model.TransactionTypeName = "Currency Foreign Exchange";

                //LoanRepayment.ForeignExchangeTrx = model;

                var result = await _loanPaymentService.RecievePayment(LoanRepayment);

                if (result.IsSuccess)
                {
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, result.ErrMessage);
                }
                isBusy = false;
            }
            catch (Exception ex)
            {
                isBusy = false;
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }

        void ReceivePaymentFocus(FocusEventArgs e)
        {
            if (PaymentCreate.RecieveAmount < PaymentCreate.TotalAmountWithPenalty)
            {
                PaymentCreate.RecieveAmount = 0;

                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                
                return;
            }
        }


        void ReceivePaymentUSDChanged(ChangeEventArgs e)
        {
            decimal amount = e.Value!.ToNumer<decimal>();

            if (amount < 0 && PaymentCreate.TotalAmountWithPenalty <= 0)
            {
                PaymentCreate.RecieveKhrAmount = 0;

                PaymentCreate.RecieveAmount = 0;

                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                return;
            }

            if (amount >= PaymentCreate.TotalAmountWithPenalty)
            {
                PaymentCreate.ChangeAmount= amount - PaymentCreate.TotalAmountWithPenalty;

                return;
            }
        }


        void CalculatePenaltyAmount(ChangeEventArgs e)
        {

            var amount = e.Value?.ToNumer<decimal>();

            if (amount < 0)
            {
                PaymentCreate.PenaltyAmount = PaymentCreate.OriginalPenaltyAmount;
                return;
            }
            if (amount > PaymentCreate.OriginalPenaltyAmount)
            {
                amount = PaymentCreate.OriginalPenaltyAmount;
                NotificationService.Notify(NotificationSeverity.Error, "input amount must be less than penalty amount.");
                RestoreDefault();
            }
            else
            {
                PaymentCreate.TotalAmountWithPenalty = PaymentCreate.OriginalPaidAmount + amount;
                PaymentCreate.RecieveAmount = PaymentCreate.OriginalPaidAmount + amount;
            }

        }
       
        void SetDefualt()
        {
            PaymentCreate.ChangeKhrAmount = 0.00M;
            PaymentCreate.ChangeAmount = 0.00M;
            PaymentCreate.RecieveKhrAmount = 0.00M;
            PaymentCreate.RecieveAmount = 0.00M;
            buyAmount = 0;
            sellAmount = 0;
            buyExchangeAmount = 0;
            sellExchangeAmount = 0;
            spotRateAmount = 0;
        }
        void RestoreDefault()
        {
            PaymentCreate.ChangeKhrAmount = 0.00M;
            PaymentCreate.ChangeAmount = 0.00M;
            buyAmount = 0;
            sellAmount = 0;
            buyExchangeAmount = 0;
            sellExchangeAmount = 0;
            spotRateAmount = 0;
            PaymentCreate.TotalAmountWithPenalty = PaymentCreate.OriginalPaidAmount + PaymentCreate.OriginalPenaltyAmount;
            PaymentCreate.PenaltyAmount = PaymentCreate.OriginalPenaltyAmount;
            gainOrLostAmount = 0;
            PaymentCreate.RecieveAmount = PaymentCreate.TotalAmountWithPenalty;
        }
    }
}




//if (CurrencyId == 1)
//{
//    PaymentCreate.RecieveKhrAmount = PaymentCreate.OriginalPaidAmount + amount;
//}
//else
//{

//}
//exchangeGainAndLostAmount = 0;

//if (CurrencyId == 1)
//{
//    PaymentCreate.RecieveKhrAmount = PaymentCreate.TotalAmountWithPenalty;
//    PaymentCreate.RecieveAmount = 0;
//}
//else
//{

//PaymentCreate.RecieveKhrAmount = 0.00M;
//}


//var resultSetting = await _settingService.GetSysSettings<AppSettingVms>();

//if (resultSetting.IsSuccess)
//{
//    settings = resultSetting.Data!;

//    gainLedgerRefId = settings!.FirstOrDefault(p => p.Code == "FXGAIN-KHR")!.Value!.ToNumer<int>();

//    lostLedgerRefId = settings!.FirstOrDefault(p => p.Code == "FXLOSS-KHR")!.Value!.ToNumer<int>();

//    buyLedgerRefId = settings!.FirstOrDefault(p => p.Code == "FXEPA-USD")!.Value!.ToNumer<int>();

//    sellLedgerRefId = settings!.FirstOrDefault(p => p.Code == "FXPA-KHR")!.Value!.ToNumer<int>();
//}
//decimal? exchangedSpotRateAmount = 0;
//decimal? exchangeGainAndLostAmount = 0;
//decimal? FXEPA_Amount = 0;
//decimal? FXPA_Amount = 0;
//int gainLedgerRefId = 0;
//int lostLedgerRefId = 0;
//int buyLedgerRefId = 0;
//int sellLedgerRefId = 0;
//int buyCurrencyRefId = 0;
//int sellCurrencyRefId = 0;
//bool isChangeAvailable = false;
//bool isSelled = false;
//bool isBuy = true;
//bool isBusy = false;
//bool disableButton = true;
//bool isPaymentExchanged = false;
//bool isExchangedChanged = false;

//if (isPaymentExchanged)
//{
//    var currencyExchanges = new ForeignExchangeDtoCreate
//    {
//        BranchRefId = BranchId,
//        BankRefId = 11,
//        BuyAmount = buyAmount,
//        SellAmount = sellAmount,
//        BuyCurrencyRefId = buyCurrencyRefId,
//        SellCurrencyRefId = sellCurrencyRefId,
//        ExchangeRate = exchangeRate,
//        BuyLedgerRefId = buyLedgerRefId,
//        SellLedgerRefId = sellLedgerRefId,
//        GainAndLostAmount = gainOrLostAmount,
//        GainAndLostLedgerRefId = gainOrLostAmount < 0 ? lostLedgerRefId : gainLedgerRefId,
//        SpotRate = selectedCurrency!.SpotRate,
//    };
//    model.ForeignExchanges!.Add(currencyExchanges);
//}
//if (isExchangedChanged)
//{
//    var currencyExchanges = new ForeignExchangeDtoCreate
//    {
//        BankRefId = 11,
//        BuyAmount = buyExchangeAmount,
//        BranchRefId = BranchId,
//        SellAmount = sellExchangeAmount,
//        BuyCurrencyRefId = 1,
//        SellCurrencyRefId = 2,
//        ExchangeRate = exchangeRate,
//        BuyLedgerRefId = buyLedgerRefId,
//        SellLedgerRefId = sellLedgerRefId,
//        GainAndLostAmount = gainOrLostAmount,
//        GainAndLostLedgerRefId = gainOrLostAmount < 0 ? lostLedgerRefId : gainLedgerRefId,
//        SpotRate = spotRate
//    };
//    model.ForeignExchanges!.Add(currencyExchanges);
//}

//void TextUsdAmountChanged(ChangeEventArgs e)
//{
//    var amount = e.Value!.ToNumer<decimal>();

//    if (amount > 0 && originalExchangeAmountUSD == 0 && originalExchangeAmountKHR == 0)
//    {
//        PaymentCreate.ChangeKhrAmount = 0;
//        PaymentCreate.ChangeUsdAmount = 0;
//        amount = 0;
//        isExchangedChanged = false;
//        return;
//    }
//    if (amount < 0 && originalExchangeAmountUSD > 0 && originalExchangeAmountKHR <= 0 ||
//       amount < 0 && originalExchangeAmountKHR > 0 && originalExchangeAmountUSD <= 0 ||
//        originalExchangeAmountUSD > 0 && amount > originalExchangeAmountUSD ||
//        originalExchangeAmountKHR > 0 && amount > originalExchangeAmountKHR)
//    {
//        PaymentCreate.ChangeUsdAmount = originalExchangeAmountUSD;
//        PaymentCreate.ChangeKhrAmount = originalExchangeAmountKHR;
//        isExchangedChanged = false;
//        amount = 0;
//        NotificationService.Notify(NotificationSeverity.Info, "please verify amount again.");
//        return;
//    }
//    if (originalExchangeAmountKHR > 0 && originalExchangeAmountKHR < 10 * sellingRate)
//    {
//        PaymentCreate.ChangeUsdAmount = 0;
//        PaymentCreate.ChangeKhrAmount = originalExchangeAmountKHR;
//        NotificationService.Notify(NotificationSeverity.Info, "You dont have enough money to exchange.");
//        return;
//    }

//    if (originalExchangeAmountKHR > 0 && originalExchangeAmountUSD == 0)
//    {
//        //Sell dollar to customer
//        var KhrAmount = (amount * sellingRate).RoundUpKhr(2);

//        if (KhrAmount > originalExchangeAmountKHR)
//        {
//            PaymentCreate.ChangeKhrAmount = originalExchangeAmountKHR;
//            PaymentCreate.ChangeUsdAmount = 0;
//            sellExchangeAmount = 0;
//            buyExchangeAmount = 0;
//            exchangedSpotRateAmount = 0;
//            exchangeGainAndLostAmount = 0;
//            amount = 0;
//            isExchangedChanged = false;
//            return;
//        }
//        else
//        {
//            PaymentCreate.ChangeKhrAmount = originalExchangeAmountKHR - KhrAmount;
//            //set sell currency amount
//            sellExchangeAmount = amount;
//            sellCurrencyRefId = 2;
//            //set buy khr amount from customer
//            buyExchangeAmount = KhrAmount;
//            buyCurrencyRefId = 1;
//            //calculate spot amount
//            exchangedSpotRateAmount = amount * spotRate;
//            //set exchange gain and lost
//            exchangeGainAndLostAmount = KhrAmount - exchangedSpotRateAmount;
//            isExchangedChanged = true;
//            return;
//        }
//    }
//    if (originalExchangeAmountUSD > 0 && originalExchangeAmountKHR == 0)
//    {
//        //buy dollar from customer
//        PaymentCreate.ChangeKhrAmount = ((originalExchangeAmountUSD - amount) * buyingRate).RoundDownKhr(2);
//        sellExchangeAmount = PaymentCreate.ChangeKhrAmount;
//        sellCurrencyRefId = 1;
//        //buy dollar
//        buyExchangeAmount = originalExchangeAmountUSD - amount;
//        buyCurrencyRefId = 2;
//        //Spot amount
//        spotRateExhcangeAmount = buyExchangeAmount * spotRate;
//        exchangeGainAndLostAmount = PaymentCreate.ChangeKhrAmount - spotRateExhcangeAmount;
//        isExchangedChanged = true;
//        return;
//    }


//}

//void ReceivePaymentKHRChanged(ChangeEventArgs e)
//{
//    var amount = e.Value!.ToNumer<decimal>();

//    if (amount < 0)
//    {
//        NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
//        SetDefualt();
//        return;
//    }
//    //no exchange occure
//    if (CurrencyId == 1 && amount >= 0 && amount >= PaymentCreate.TotalAmountWithPenalty)
//    {

//        PaymentCreate.ChangeKhrAmount = amount - PaymentCreate.TotalAmountWithPenalty;
//        originalExchangeAmountKHR = amount - TotalPaidAmount;
//        isPaymentExchanged = false;
//        PaymentCreate.RecieveUsdAmount = 0;
//        PaymentCreate.ChangeUsdAmount = 0;
//        gainOrLostAmount = 0;
//        exchangeGainAndLostAmount = 0;
//        return;
//    }

//    if (CurrencyId == 1 && amount >= 0 && amount < PaymentCreate.TotalAmountWithPenalty)
//    {
//        var leftKhrAmount = PaymentCreate.TotalAmountWithPenalty - amount;
//        //buy dollar from customer
//        var buyAmount = (leftKhrAmount / buyingRate).RoundUsd(2);
//        PaymentCreate.RecieveUsdAmount = buyAmount;
//        buyCurrencyRefId = 2;
//        //set sell amount
//        sellAmount = leftKhrAmount;
//        sellCurrencyRefId = 1;
//        //set spot rate amoun
//        spotRateAmount = buyAmount * spotRate;
//        //set gain or lost amount
//        gainOrLostAmount = spotRateAmount - leftKhrAmount;
//        PaymentCreate.ChangeUsdAmount = 0;
//        PaymentCreate.ChangeKhrAmount = 0;
//        isPaymentExchanged = true;
//        return;
//    }

//}

//void ReceivePaymentKHRChanged(decimal? amount)
//{
//    if (amount < 0)
//    {
//        NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
//        SetDefualt();
//        return;
//    }
//    //no exchange occure
//    if (CurrencyId == 1 && amount >= 0 && amount >= PaymentCreate.TotalAmount)
//    {

//        PaymentCreate.ChangeKhrAmount = amount - PaymentCreate.TotalAmount;
//        originalExchangeAmountKHR = amount - TotalPaidAmount;
//        isPaymentExchanged = false;
//        PaymentCreate.RecieveUsdAmount = 0;
//        PaymentCreate.ChangeUsdAmount = 0;
//        gainOrLostAmount = 0;
//        exchangeGainAndLostAmount = 0;
//        return;
//    }

//    if (CurrencyId == 1 && amount >= 0 && amount < PaymentCreate.TotalAmount)
//    {
//        var leftKhrAmount = TotalPaidAmount - amount;
//        //buy dollar from customer
//        var buyAmount = (leftKhrAmount / buyingRate).RoundUsd(2);
//        PaymentCreate.RecieveUsdAmount = buyAmount;
//        buyCurrencyRefId = 2;
//        //set sell amount
//        sellAmount = leftKhrAmount;
//        sellCurrencyRefId = 1;
//        //set spot rate amoun
//        spotRateAmount = buyAmount * spotRate;
//        //set gain or lost amount
//        gainOrLostAmount = spotRateAmount - leftKhrAmount;
//        PaymentCreate.ChangeUsdAmount = 0;
//        PaymentCreate.ChangeKhrAmount = 0;
//        isPaymentExchanged = true;
//        return;
//    }

//}


//void ReceivePaymentUSDChanged(decimal? amount)
//{
//    if (amount < 0 && PaymentCreate.TotalAmount <= 0)
//    {
//        PaymentCreate.RecieveUsdAmount = 0;
//        PaymentCreate.RecieveKhrAmount = 0;
//        NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
//        return;
//    }
//    if ((CurrencyId == 1 || CurrencyId == 2) && amount < 0 && TotalPaidAmount > 0)
//    {
//        PaymentCreate.RecieveUsdAmount = 0;
//        PaymentCreate.RecieveKhrAmount = 0;
//        PaymentCreate.ChangeKhrAmount = 0;
//        PaymentCreate.ChangeUsdAmount = 0;
//        isPaymentExchanged = false;
//        NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
//        return;
//    }
//    ///no currency exchange occur
//    if (CurrencyId == 2 && amount > 0 && amount >= PaymentCreate.TotalAmount)
//    {
//        PaymentCreate.ChangeUsdAmount = amount - TotalPaidAmount;
//        originalExchangeAmountUSD = PaymentCreate.ChangeUsdAmount;
//        PaymentCreate.RecieveKhrAmount = 0;
//        PaymentCreate.ChangeKhrAmount = 0;
//        originalExchangeAmountKHR = 0;
//        isPaymentExchanged = false;
//        buyExchangeAmount = 0;
//        sellExchangeAmount = 0;
//        gainOrLostAmount = 0;
//        spotRateAmount = 0;
//        buyAmount = 0;
//        sellAmount = 0;
//        isChangeAvailable = PaymentCreate.ChangeUsdAmount > 0 ? true : false;
//        return;
//    }


//    //curency exchange occur
//    if (CurrencyId == 2 && amount > 0 && amount < PaymentCreate.TotalAmount)
//    {
//        //sell usd to customer
//        var sellUsdAmount = TotalPaidAmount - amount;
//        //sell dollar to customer and buy riels from  customer
//        var totalKhrAmount = (sellUsdAmount * sellingRate).RoundUpKhr(2);
//        //get spot rate amount
//        spotRateAmount = sellUsdAmount * spotRate;
//        //get gain or lost amount
//        gainOrLostAmount = totalKhrAmount - spotRateAmount;
//        sellAmount = sellUsdAmount;
//        buyAmount = PaymentCreate.RecieveKhrAmount = totalKhrAmount;
//        exchangeRate = sellingRate!.Value;
//        isPaymentExchanged = true;
//        PaymentCreate.ChangeUsdAmount = 0;
//        PaymentCreate.ChangeKhrAmount = 0;
//        originalExchangeAmountKHR = 0;
//        originalExchangeAmountUSD = 0;
//        return;
//    }

//    //curency exchange occur
//    if (CurrencyId == 2 && amount == 0 && PaymentCreate.TotalAmount > 0)
//    {

//        var totalKhrAmount = (TotalPaidAmount * sellingRate).RoundUpKhr(2);
//        //sell dollar to customer and buy riels from  customer
//        sellAmount = TotalPaidAmount;
//        sellCurrencyRefId = 2;
//        //buy amount
//        buyAmount = PaymentCreate.RecieveKhrAmount = totalKhrAmount;
//        buyCurrencyRefId = 1;
//        //get spot rate amount
//        spotRateAmount = TotalPaidAmount * spotRate;
//        //get gain or lost amount
//        gainOrLostAmount = totalKhrAmount - spotRateAmount;
//        exchangeRate = sellingRate!.Value;
//        isPaymentExchanged = true;
//        PaymentCreate.ChangeUsdAmount = 0;
//        PaymentCreate.ChangeKhrAmount = 0;
//        originalExchangeAmountKHR = 0;
//        originalExchangeAmountUSD = 0;
//        return;
//    }

//    //Payment currency is riels

//    if (CurrencyId == 1 && amount < 0)
//    {
//        RestoreDefault();
//        return;
//    }
//    if (CurrencyId == 1 && (amount == 0 || PaymentCreate.RecieveUsdAmount == 0))
//    {
//        PaymentCreate.RecieveKhrAmount = TotalPaidAmount;
//        PaymentCreate.ChangeKhrAmount = 0;
//        PaymentCreate.ChangeUsdAmount = 0;
//        PaymentCreate.RecieveUsdAmount = 0;
//        buyAmount = 0;
//        sellAmount = 0;
//        gainOrLostAmount = 0;
//        exchangeGainAndLostAmount = 0;
//        spotRate = 0;
//        return;
//    }

//    if (CurrencyId == 1 && amount > 0 && PaymentCreate.TotalAmount > 0)
//    {
//        //buy us dollar
//        var totalKhrAmount = (amount * buyingRate).RoundDownKhr(2);
//        if (totalKhrAmount < TotalPaidAmount)
//        {
//            //amount is dollar
//            buyAmount = amount;
//            buyCurrencyRefId = 2;
//            //calculate left khmer riels  amount
//            PaymentCreate.RecieveKhrAmount = PaymentCreate.TotalAmount - totalKhrAmount;
//            //calculate selling amount
//            sellAmount = totalKhrAmount;
//            sellCurrencyRefId = 1;
//            //Calculate for spot rate amount
//            spotRateAmount = amount * spotRate;
//            //Calculate for gain and lost
//            gainOrLostAmount = spotRateAmount - totalKhrAmount;
//            //set exchange rate
//            exchangeRate = buyingRate!.Value;
//            PaymentCreate.ChangeKhrAmount = 0;
//            PaymentCreate.ChangeUsdAmount = 0;
//            isPaymentExchanged = true;
//            return;
//        }
//        else
//        {

//            PaymentCreate.RecieveKhrAmount = 0;
//            PaymentCreate.ChangeKhrAmount = totalKhrAmount - PaymentCreate.TotalAmount;
//            originalExchangeAmountKHR = PaymentCreate.ChangeKhrAmount;
//            PaymentCreate.ChangeUsdAmount = 0;
//            //assign buy usd amount
//            buyAmount = amount;
//            buyCurrencyRefId = 2;
//            //assign khr amount.
//            sellAmount = totalKhrAmount;
//            sellCurrencyRefId = 1;
//            //calculate spot rate amount.
//            spotRateAmount = amount * spotRate;
//            //calculate gain and lost
//            gainOrLostAmount = spotRateAmount - TotalPaidAmount;
//            //set exchange rate
//            exchangeRate = buyingRate!.Value;
//            isPaymentExchanged = true;
//            return;
//        }
//    }
//}


//void TextUsdAmountChanged(decimal? amount)
//{

//    if (amount > 0 && originalExchangeAmountUSD == 0 && originalExchangeAmountKHR == 0)
//    {
//        PaymentCreate.ChangeUsdAmount = 0;
//        PaymentCreate.ChangeKhrAmount = 0;
//        amount = 0;
//        isExchangedChanged = false;
//        return;
//    }
//    if ((amount < 0 && originalExchangeAmountUSD > 0 && originalExchangeAmountKHR <= 0) ||
//       (amount < 0 && originalExchangeAmountKHR > 0 && originalExchangeAmountUSD <= 0) ||
//        (originalExchangeAmountUSD > 0 && amount > originalExchangeAmountUSD) ||
//        (originalExchangeAmountKHR > 0 && amount > originalExchangeAmountKHR))
//    {
//        PaymentCreate.ChangeUsdAmount = originalExchangeAmountUSD;
//        PaymentCreate.ChangeKhrAmount = originalExchangeAmountKHR;
//        isExchangedChanged = false;
//        amount = 0;
//        NotificationService.Notify(NotificationSeverity.Info, "please verify amount again.");
//        return;
//    }
//    if (originalExchangeAmountKHR > 0 && originalExchangeAmountKHR < (10 * sellingRate))
//    {
//        PaymentCreate.ChangeUsdAmount = 0;
//        PaymentCreate.ChangeKhrAmount = originalExchangeAmountKHR;
//        NotificationService.Notify(NotificationSeverity.Info, "You dont have enough money to exchange.");
//        return;
//    }

//    if (originalExchangeAmountKHR > 0 && originalExchangeAmountUSD == 0)
//    {
//        //Sell dollar to customer
//        var KhrAmount = (amount * sellingRate).RoundUpKhr(2);

//        if (KhrAmount > originalExchangeAmountKHR)
//        {
//            PaymentCreate.ChangeKhrAmount = originalExchangeAmountKHR;
//            PaymentCreate.ChangeUsdAmount = 0;
//            sellExchangeAmount = 0;
//            buyExchangeAmount = 0;
//            exchangedSpotRateAmount = 0;
//            exchangeGainAndLostAmount = 0;
//            amount = 0;
//            isExchangedChanged = false;
//            return;
//        }
//        else
//        {
//            PaymentCreate.ChangeKhrAmount=originalExchangeAmountKHR - KhrAmount;
//            //set sell currency amount
//            sellExchangeAmount = amount;
//            sellCurrencyRefId = 2;
//            //set buy khr amount from customer
//            buyExchangeAmount = KhrAmount;
//            buyCurrencyRefId = 1;
//            //calculate spot amount
//            exchangedSpotRateAmount = amount * spotRate;
//            //set exchange gain and lost
//            exchangeGainAndLostAmount = KhrAmount - exchangedSpotRateAmount;
//            isExchangedChanged = true;
//            return;
//        }
//    }
//    if (originalExchangeAmountUSD > 0 && originalExchangeAmountKHR == 0)
//    {
//        //buy dollar from customer
//        PaymentCreate.ChangeKhrAmount = ((originalExchangeAmountUSD - amount) * buyingRate).RoundDownKhr(2);
//        sellExchangeAmount = PaymentCreate.ChangeKhrAmount;
//        sellCurrencyRefId = 1;
//        //buy dollar
//        buyExchangeAmount = originalExchangeAmountUSD - amount;
//        buyCurrencyRefId = 2;
//        //Spot amount
//        spotRateExhcangeAmount = buyExchangeAmount * spotRate;
//        exchangeGainAndLostAmount = PaymentCreate.ChangeKhrAmount - spotRateExhcangeAmount;
//        isExchangedChanged = true;
//        return;
//    }


//}

//if ((CurrencyId == 1 || CurrencyId == 2) && amount < 0 && PaymentCreate.TotalAmountWithPenalty > 0)
//{
//    PaymentCreate.RecieveKhrAmount = 0;
//    PaymentCreate.ChangeKhrAmount = 0;
//    PaymentCreate.RecieveUsdAmount = 0;
//    PaymentCreate.ChangeUsdAmount = 0;
//    isPaymentExchanged = false;
//    NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
//    return;
//}
/////no currency exchange occur
//if (CurrencyId == 2 && amount > 0 && amount >= PaymentCreate.TotalAmountWithPenalty)
//{
//    PaymentCreate.ChangeUsdAmount = amount - PaymentCreate.TotalAmountWithPenalty;
//    originalExchangeAmountUSD = PaymentCreate.ChangeUsdAmount;
//    PaymentCreate.RecieveKhrAmount = 0;
//    PaymentCreate.ChangeKhrAmount = 0;
//    originalExchangeAmountKHR = 0;
//    isPaymentExchanged = false;
//    buyExchangeAmount = 0;
//    sellExchangeAmount = 0;
//    gainOrLostAmount = 0;
//    spotRateAmount = 0;
//    buyAmount = 0;
//    sellAmount = 0;
//    isChangeAvailable = PaymentCreate.ChangeUsdAmount > 0 ? true : false;
//    return;
//}


////curency exchange occur
//if (CurrencyId == 2 && amount > 0 && amount < PaymentCreate.TotalAmountWithPenalty)
//{
//    //sell usd to customer
//    var sellUsdAmount = PaymentCreate.TotalAmountWithPenalty - amount;
//    //sell dollar to customer and buy riels from  customer
//    var totalKhrAmount = (sellUsdAmount * sellingRate).RoundUpKhr(2);
//    //get spot rate amount
//    spotRateAmount = sellUsdAmount * spotRate;
//    //get gain or lost amount
//    gainOrLostAmount = totalKhrAmount - spotRateAmount;
//    sellAmount = sellUsdAmount;
//    buyAmount = PaymentCreate.RecieveKhrAmount = totalKhrAmount;
//    exchangeRate = sellingRate!.Value;
//    isPaymentExchanged = true;
//    PaymentCreate.ChangeUsdAmount = 0;
//    PaymentCreate.ChangeKhrAmount = 0;
//    originalExchangeAmountKHR = 0;
//    originalExchangeAmountUSD = 0;
//    return;
//}

////curency exchange occur
//if (CurrencyId == 2 && amount == 0 && PaymentCreate.TotalAmountWithPenalty > 0)
//{

//    var totalKhrAmount = (TotalPaidAmount * sellingRate).RoundUpKhr(2);
//    //sell dollar to customer and buy riels from  customer
//    sellAmount = TotalPaidAmount;
//    sellCurrencyRefId = 2;
//    //buy amount
//    buyAmount = PaymentCreate.RecieveKhrAmount = totalKhrAmount;
//    buyCurrencyRefId = 1;
//    //get spot rate amount
//    spotRateAmount = TotalPaidAmount * spotRate;
//    //get gain or lost amount
//    gainOrLostAmount = totalKhrAmount - spotRateAmount;
//    exchangeRate = sellingRate!.Value;
//    isPaymentExchanged = true;
//    PaymentCreate.ChangeUsdAmount = 0;
//    PaymentCreate.ChangeKhrAmount = 0;
//    originalExchangeAmountKHR = 0;
//    originalExchangeAmountUSD = 0;
//    return;
//}

////Payment currency is riels

//if (CurrencyId == 1 && amount < 0)
//{
//    RestoreDefault();
//    return;
//}
//if (CurrencyId == 1 && (amount == 0 || PaymentCreate.RecieveUsdAmount == 0))
//{
//    PaymentCreate.RecieveKhrAmount = PaymentCreate.TotalAmountWithPenalty;
//    PaymentCreate.ChangeKhrAmount = 0;
//    PaymentCreate.ChangeUsdAmount = 0;
//    buyAmount = 0;
//    sellAmount = 0;
//    gainOrLostAmount = 0;
//    exchangeGainAndLostAmount = 0;
//    PaymentCreate.RecieveUsdAmount = 0;
//    spotRate = 0;
//    return;
//}

//if (CurrencyId == 1 && amount > 0 && PaymentCreate.TotalAmountWithPenalty > 0)
//{
//    //buy us dollar
//    var totalKhrAmount = (amount * buyingRate).RoundDownKhr(2);
//    if (totalKhrAmount < TotalPaidAmount)
//    {
//        //amount is dollar
//        buyAmount = amount;
//        buyCurrencyRefId = 2;
//        //calculate left khmer riels  amount
//        PaymentCreate.RecieveKhrAmount = PaymentCreate.TotalAmountWithPenalty - totalKhrAmount;
//        //calculate selling amount
//        sellAmount = totalKhrAmount;
//        sellCurrencyRefId = 1;
//        //Calculate for spot rate amount
//        spotRateAmount = amount * spotRate;
//        //Calculate for gain and lost
//        gainOrLostAmount = spotRateAmount - totalKhrAmount;
//        //set exchange rate
//        exchangeRate = buyingRate!.Value;
//        PaymentCreate.ChangeKhrAmount = 0;
//        PaymentCreate.ChangeUsdAmount = 0;
//        isPaymentExchanged = true;
//        return;
//    }
//    else
//    {

//        PaymentCreate.RecieveKhrAmount = 0;
//        PaymentCreate.ChangeKhrAmount = totalKhrAmount - PaymentCreate.TotalAmountWithPenalty;
//        originalExchangeAmountKHR = PaymentCreate.ChangeKhrAmount;
//        PaymentCreate.ChangeUsdAmount = 0;
//        //assign buy usd amount
//        buyAmount = amount;
//        buyCurrencyRefId = 2;
//        //assign khr amount.
//        sellAmount = totalKhrAmount;
//        sellCurrencyRefId = 1;
//        //calculate spot rate amount.
//        spotRateAmount = amount * spotRate;
//        //calculate gain and lost
//        gainOrLostAmount = spotRateAmount - TotalPaidAmount;
//        //set exchange rate
//        exchangeRate = buyingRate!.Value;
//        isPaymentExchanged = true;
//        return;
//    }
//}