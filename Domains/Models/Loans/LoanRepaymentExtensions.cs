namespace Domains.Models.Loans
{
    public static class LoanRepaymentExtensions
    {
        public static decimal Total(this LoanPayment loanRepayment)
        {
            var total=loanRepayment.Principle + loanRepayment.Interest + loanRepayment.CollectionFeeAmount + loanRepayment.PenaltyAmount;
            return total ?? 0;
        }
    }
}
