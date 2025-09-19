namespace Applications.Features.LoanPayments.Requests
{
    public sealed class PaymentCreate
    {
        public string? RecieptNumber { get; set; }
        public decimal? PenaltyAmount { get; set; } = 0;
        public decimal? OriginalPenaltyAmount { get; set; }
        public decimal? OriginalPaidAmount { get; set; }
        public decimal? TotalAmountNoPenalty { get; set; } = 0;
        public decimal? TotalAmountWithPenalty { get; set; } = 0;
        public decimal? RecieveAmount { get; set; } = 0;
        public decimal? RecieveKhrAmount { get; set; } = 0;
        public decimal? ChangeAmount { get; set; } = 0;
        public decimal? ChangeKhrAmount { get; set; } = 0;
        public List<LoanPaymentLineDtoCreate> LoanRepaymentLines { get; set; } = [];
    }
}
