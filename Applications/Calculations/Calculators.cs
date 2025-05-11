using Applications.Requests.Loans.Disbursements;

namespace Applications.Calculations
{
    public sealed class Calculators
    {
        int? repaymentModeId;

        int? currencyId;

        public static Calculators CreateBuilder(int? repaymentModeId, int? currencyId)
        {
            return new Calculators(repaymentModeId, currencyId);
        }
        public static Calculators CreateBuilder(LoanDtoCreate req)
        {
            return new Calculators(req.RepaymentModeRefId, req.CurrencyRefId);
        }

        private Calculators(int? repaymentModeId, int? currencyId)
        {
            this.repaymentModeId = repaymentModeId;

            this.currencyId = currencyId;
        }
        public CalculatorBuilder Build(LoanInfo e)
        {
            return new(currencyId, e.PayEvery, repaymentModeId, e.CollectionFeeModeId, e.InstallmentTypeId, e.CollectionFeeRate, e.FirstPayment, e.StartDate);
        }

        public CalculatorBuilder Build(LoanDtoCreate e)
        {
            return new(e.CurrencyRefId, e.PayEvery, e.RepaymentModeRefId, e.CollectionFeeModeId, e.InstallmentTypeId, e.CollectionFeeRate, e.FirstPaymentDate, e.StartDate);
        }
    }
}
