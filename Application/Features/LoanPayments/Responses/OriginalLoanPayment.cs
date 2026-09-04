namespace Application.Features.LoanPayments.Responses
{
    public sealed class OriginalLoanPayment
    {
        public decimal? OriginalPaidAmount { get; set; } = 0; 
        public decimal? OriginalPenaltyAmount { get; set; } = 0;
    }
}
