using Applications.Extensions;
using Applications.Extensions.Maths;
using Applications.Extensions.NumberExtensions;
using Applications.Features.Currencies.Responses;
using Applications.Features.LoanPayments.Requests;
using Applications.Features.Transactions.ForiegnExchanges;
using Applications.Features.Transactions.LoanTransactions;
using Applications.Interfaces.Currencies;
using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Services.Transactions.Currencies;

namespace TrapchumApp.Pages.Transactions.Loans.Repayments
{

    public partial class ConfirmPaymentDialogue
    {
        private readonly ICurrencyService _currencyService;
        private readonly IForiegnCurrencyExchangeImpl _exchangeServiceImpl;
        private readonly ILoanPaymentService _paymentService;
        public ConfirmPaymentDialogue(ICurrencyService currencyService, IForiegnCurrencyExchangeImpl exchangeServiceImpl, ILoanPaymentService loanPaymentService)
        {
            _currencyService = currencyService;
            _exchangeServiceImpl = exchangeServiceImpl;
            _paymentService = loanPaymentService;
        }

        Dictionary<string, object> attributes = new Dictionary<string, object>
    {
        {  "class","input-height" }
    };
        [Parameter] public VSchedule LoanPayment { set; get; } = new();
        [Parameter] public int CurrencyId { set; get; } = 1;
        [Parameter] public string? LoanId { set; get; }
        [Parameter] public string? CreditAgentId { set; get; }
        [Parameter] public string? UserId { set; get; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        decimal? appliedAmountUSD = 0;
        decimal? appliedAmountKHR = 0;
        decimal? receiveAmountUsd = 0;
        decimal? receiveAmountKhr = 0;
        decimal? changedAmountUSD = 0;
        decimal? changedAmountKHR = 0;
        decimal? changeAppliedAmountUSD = 0;
        decimal? changeAppliedAmountKHR = 0;
        decimal? rielsLeftAmount = 0;
        decimal? dollarLeftAmount = 0;
        decimal? penaltyAmount = 0;
        decimal exchangeRate = 0;
        decimal? paidAmount = 0;
        decimal? totalPenaltyAmount = 0;
        private IEnumerable<VCurrenciesList> Currencies = [];
        private VCurrenciesList? selectedCurrency;
        private decimal? totalPaidAmount = 0;
        bool isBuy = true;

        TrxForeignExchangeDtoCreate model = new TrxForeignExchangeDtoCreate();

        ForeignExchangeDtoCreate c = new ForeignExchangeDtoCreate();
        private LoanPaymentDtoCreate LoanRepayment { get; set; } = new();
        public ICollection<LoanPaymentLineDtoCreate> LoanRepaymentLines { get; set; } = [];

        bool isBusy = false;

        bool disableButton = true;

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            var user = auth.User;

            await GetCurrencies();

            GetTotalPaidAmount();

            await base.OnInitializedAsync();
        }

        private async Task GetCurrencies()
        {
            var currencyResponse = await _currencyService.GetCurrencyDetailById(1);

            //if (currencyResponse.IsSuccess)
            //{
            //    selectedCurrency = currencyResponse.Data!;
            //}
        }

        void GetTotalPaidAmount()
        {
            if (LoanPayment.LoanId.IsNullOrEmpty()) return;

            paidAmount = LoanPayment.TotalPay;

            penaltyAmount = LoanPayment.PenaltyAmount.RoundUp(2, CurrencyId);

            totalPenaltyAmount = penaltyAmount;

            if (CurrencyId == 1)
            {
                appliedAmountKHR = paidAmount + penaltyAmount;
            }
            if (CurrencyId == 2)
            {
                appliedAmountUSD = paidAmount + penaltyAmount;
            }
        }

        void CloseModal()
        {
            DialogService.Close(false);
        }
        async Task OnSaveChanges()
        {
            try
            {
                var line = new LoanPaymentLineDtoCreate
                {
                    LoanRefId = LoanPayment.LoanId,
                    LoanScheduleRefId = LoanPayment.LoanScheduleId,
                    InstallmentNo = LoanPayment!.InstallmentNo,
                    NumberOfDay = LoanPayment!.TotalDay,
                    DayArrea = LoanPayment.DayArrea,
                    PayDate = LoanPayment.PaymentDate,
                    PaidPrinciple = LoanPayment.Principle,
                    PaidInterest = LoanPayment.Interest,
                    PaidCollectionFee = LoanPayment.CollectionFee,
                    PaidPenaltyAmount = penaltyAmount,
                    SubTotal = LoanPayment.TotalPay + penaltyAmount
                };
                LoanRepayment.LoanRefId = LoanId;
                LoanRepayment.CurrencyRefId = CurrencyId;
                LoanRepayment.CreditAgentRefId = CreditAgentId;
                LoanRepayment.PayDate = DateTime.Now;
                LoanRepayment.Principle = LoanPayment.Principle;
                LoanRepayment.Interest = LoanPayment.Interest;
                LoanRepayment.CollectionFeeAmount = LoanPayment.CollectionFee;
                LoanRepayment.PenaltyAmount = penaltyAmount;
                LoanRepayment.SubTotal = LoanPayment.TotalPay;
                LoanRepayment.OfficialReceiptNo = "";
                LoanRepayment.BranchRefId = LoanPayment.BranchId;
                LoanRepayment.UserRefId = UserId;
                LoanRepayment.LoanPaymentLines.Add(line);

                if (appliedAmountKHR > 0)
                {
                    var tranxCashManagement = new TrxCashManagementDtoCreate()
                    {
                        CurrencyRefId = 1,
                        TransactionDate = DateTime.Now,
                        BusinessDate = DateTime.Now,
                        CashStatusRefId = 2,
                        BankRefId = 11,
                        Amount = appliedAmountKHR,
                    };
                    //LoanRepayment.LoanTransaction!.CashManagements!.Add(tranxCashManagement);
                }

                if (appliedAmountUSD > 0)
                {
                    var tranxCashManagement = new TrxCashManagementDtoCreate()
                    {
                        CurrencyRefId = 2,
                        TransactionDate = DateTime.Now,
                        BusinessDate = DateTime.Now,
                        CashStatusRefId = 2,
                        BankRefId = 11,
                        Amount = appliedAmountUSD,
                    };
                    //LoanRepayment.LoanTransaction!.CashManagements!.Add(tranxCashManagement);
                }

                model.UserRefId = UserId;
                model.TranTypeRefId = 67;
                model.TransactionTypeName = "Currency Foreign Exchange";
                //c.StaffRefId = userId;
                //c.StaffAuthRefId = UserId;
                c.BankRefId = 11;
                if (isBuy)
                {
                    c.BuyAmount = appliedAmountKHR;
                    c.SellAmount = appliedAmountUSD;
                    c.BuyCurrencyRefId = 2;
                    c.SellCurrencyRefId = 1;
                    c.ExchangeRate = exchangeRate;
                    c.BuyLedgerRefId = 1;
                    c.SellLedgerRefId = 2;
                    c.SpotRate = selectedCurrency!.SpotRate;
                }
                else
                {
                    c.BuyAmount = appliedAmountUSD;
                    c.SellAmount = appliedAmountKHR;
                    c.BuyCurrencyRefId = 1;
                    c.SellCurrencyRefId = 2;
                    c.ExchangeRate = exchangeRate;
                    c.BuyLedgerRefId = 2;
                    c.SellLedgerRefId = 1;
                    c.SpotRate = selectedCurrency!.SpotRate;
                }
                //model.ForeignExchanges!.Add(c);

                //var resultExchanged = await _exchangeServiceImpl.Exchange(model);

                //if (!resultExchanged.IsSuccess)
                //{
                //    NotificationService.Notify(NotificationSeverity.Error, resultExchanged.ErrMessage);
                //    return;
                //}

                var confirmPayment = await DialogService.Confirm("Please make sure that money receive amount is correct",
                "Confirm Payment", new ConfirmOptions
                {
                    CancelButtonText = "Cancel",
                    OkButtonText = "Yes",
                });

                if (confirmPayment == false)
                {
                    return;
                }

                var resultResponse = await _paymentService.RecievePayment(LoanRepayment);

                if (resultResponse.IsSuccess)
                {
                    NotificationService.Notify(NotificationSeverity.Success, resultResponse.Data);
                   
                    DialogService.Close(true);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, resultResponse.ErrMessage);
                }

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }






        void ReceivePaymentUSDChanged(decimal? amount)
        {
            if (amount <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                receiveAmountUsd = 0;
                receiveAmountKhr = 0;
                return;
            }

            //selling dollar
            if (CurrencyId == 2)
            {
                if (amount > appliedAmountUSD)
                {
                    var leftamount = amount - appliedAmountUSD;
                    changedAmountUSD = leftamount;
                    changeAppliedAmountUSD = changedAmountUSD;
                    receiveAmountKhr = 0;
                    changeAppliedAmountKHR = 0;
                    changedAmountKHR = 0;
                }
                else
                {
                    var leftamount = appliedAmountUSD - amount;
                    receiveAmountKhr = (leftamount * selectedCurrency!.SellingRate)!.Value.RoundUp(-2);
                    changeAppliedAmountUSD = 0;
                    changedAmountUSD = 0;
                    changeAppliedAmountKHR = 0;
                    changedAmountKHR = 0;
                }
            }

            if (CurrencyId == 1)
            {
                var exchangedKhrToDollars = (appliedAmountKHR / selectedCurrency!.SellingRate)!.Value.RoundDown(2);

                if (amount > exchangedKhrToDollars)
                {
                    receiveAmountKhr = 0;
                    changedAmountKHR = 0;
                    changeAppliedAmountKHR = 0;
                    var leftAmount = amount - exchangedKhrToDollars;
                    changedAmountUSD = leftAmount;
                    changeAppliedAmountUSD = leftAmount;
                }
                else
                {
                    changedAmountUSD = 0;
                    changeAppliedAmountUSD = 0;
                    var leftAmount = (exchangedKhrToDollars - amount) * selectedCurrency.SellingRate;
                    changedAmountKHR = leftAmount!.Value!.RoundUp(-2);
                    changeAppliedAmountKHR = leftAmount!.Value!.RoundUp(-2);
                }
            }
        }
        void ReceivePaymentKHRChanged(decimal? amount)
        {
            if (amount <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "please input apply amount.");
                SetDefualt();
                return;
            }
            if (CurrencyId == 2)
            {
                var exchangeReceiveAmount = (amount / selectedCurrency!.BuyingRate!.Value)!.Value!.RoundUp(2);
                if (exchangeReceiveAmount < appliedAmountUSD)
                {
                    var leftAmount = appliedAmountUSD - exchangeReceiveAmount;
                    receiveAmountUsd = leftAmount;
                    changedAmountUSD = 0;
                    changeAppliedAmountUSD = 0;
                    changedAmountKHR = 0;
                    changeAppliedAmountKHR = 0;
                }
                else
                {
                    var exchangedUsdToKhr = (appliedAmountUSD * selectedCurrency.SellingRate)!.Value.RoundUp((-2));
                    var leftAmount = amount - exchangedUsdToKhr;
                    changeAppliedAmountKHR = leftAmount;
                    changedAmountKHR = leftAmount;
                    receiveAmountUsd = 0;
                    changedAmountUSD = 0;
                    changeAppliedAmountUSD = 0;
                }
            }


            if (CurrencyId == 1)
            {
                if (amount >= appliedAmountKHR)
                {
                    var leftAmount = amount - appliedAmountKHR;
                    changeAppliedAmountKHR = changedAmountKHR = leftAmount;
                    receiveAmountUsd = 0;
                    changedAmountUSD = 0;
                    appliedAmountUSD = 0;
                    changeAppliedAmountUSD = 0;
                }
                else
                {
                    changeAppliedAmountKHR = 0;
                    receiveAmountUsd = 0;
                    changedAmountUSD = 0;
                    appliedAmountUSD = 0;
                    changeAppliedAmountUSD = 0;
                    changedAmountKHR = 0;
                    receiveAmountKhr = 0;
                    amount = 0;
                }
            }


        }

        void CalculatePenaltyAmount(decimal? amount)
        {
            if (amount < 0)
            {
                penaltyAmount = totalPenaltyAmount;
                return;
            }
            if (amount > totalPenaltyAmount)
            {
                NotificationService.Notify(NotificationSeverity.Error, "input amount must be less than penalty amount.");
                penaltyAmount = totalPenaltyAmount;
                if (CurrencyId == 1)
                {
                    appliedAmountKHR = paidAmount + totalPenaltyAmount;
                }
                else
                {
                    appliedAmountUSD = paidAmount + totalPenaltyAmount;
                }
                SetDefualt();
                return;
            }

            if (CurrencyId == 1)
            {
                appliedAmountKHR = paidAmount + amount;
                SetDefualt();
            }
            if (CurrencyId == 2)
            {
                appliedAmountUSD = paidAmount + amount;
                SetDefualt();
            }
        }

        void CalculatePenaltyAmount(ChangeEventArgs e)
        {
            var amount = e.Value!.ToNumer<decimal>();

            if (amount < 0)
            {
                penaltyAmount = totalPenaltyAmount;
                return;
            }
            if (amount > totalPenaltyAmount)
            {
                NotificationService.Notify(NotificationSeverity.Error, "input amount must be less than penalty amount.");

                penaltyAmount = totalPenaltyAmount;

                if (CurrencyId == 1)
                {
                    appliedAmountKHR = paidAmount + totalPenaltyAmount;
                }
                else
                {
                    appliedAmountUSD = paidAmount + totalPenaltyAmount;
                }
                SetDefualt();
                return;
            }

            if (CurrencyId == 1)
            {
                appliedAmountKHR = paidAmount + amount;
                SetDefualt();
            }
            if (CurrencyId == 2)
            {
                appliedAmountUSD = paidAmount + amount;
                SetDefualt();
            }
        }



        void CalculateChangeAppliedUsd(decimal? amount)
        {
            if (amount >= 0 && changedAmountUSD > 0 && changeAppliedAmountUSD <= changedAmountUSD)
            {
                var takeDollarForChange = changedAmountUSD - changeAppliedAmountUSD;

                var echangeDollarToKhmer = (takeDollarForChange * selectedCurrency!.BuyingRate) + changedAmountKHR;

                changeAppliedAmountKHR = echangeDollarToKhmer!.Value.RoundDown(-2);
            }
            else
            {
                changeAppliedAmountUSD = changedAmountUSD;
            }

        }
        void SetDefualt()
        {
            changedAmountKHR = 0.00M;
            changedAmountUSD = 0.00M;
            receiveAmountKhr = 0.00M;
            receiveAmountUsd = 0.00M;
            changeAppliedAmountKHR = 0.00M;
            changeAppliedAmountUSD = 0.00M;
        }
        void RestoreDefault()
        {
            changedAmountKHR = 0.00M;
            changedAmountUSD = 0.00M;
            receiveAmountKhr = 0.00M;
            receiveAmountUsd = 0.00M;
            changeAppliedAmountKHR = 0.00M;
            changeAppliedAmountUSD = 0.00M;
            if (CurrencyId == 2)
            {
                appliedAmountUSD = paidAmount;
                appliedAmountKHR = 0;
            }
            else
            {
                appliedAmountKHR = paidAmount;
                appliedAmountUSD = 0;
            }
        }


    }
}