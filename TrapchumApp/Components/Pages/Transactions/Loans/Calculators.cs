using Applications.Requests.Loans.Disbursements;
using Domains.Models.Loans;

namespace TrapchumApp.Components.Pages.Transactions.Loans
{
    public sealed class Calculators
    {
        int repaymentModeId;
        int currencyId;
        public static Calculators CreateBuilder(int? repaymentModeId, int? currencyId)
        {
            return new Calculators(repaymentModeId, currencyId);
        }
        private Calculators(int? repaymentModeId, int? currencyId)
        {
            this.repaymentModeId = repaymentModeId!.Value;

            this.currencyId = currencyId!.Value;
        }
        public CalculatorBuilder Build(LoanInfo e)
        {
            return new(currencyId, e.PayEvery, repaymentModeId, e.CollectionFeeModeId, e.InstallmentTypeId, e.EffectiveRate, e.FirstPayment, e.StartDate);
        }

        public CalculatorBuilder Build(LoanDtoCreate e)
        {
            return new(currencyId, e.PayEvery!.Value, repaymentModeId, e.OtherChargeId!.Value, e.InstallmentTypeId!.Value, e.CollectionFeeRate!.Value, e.FirstPaymentDate!.Value, e.StartDate!.Value);
        }
    }
}
