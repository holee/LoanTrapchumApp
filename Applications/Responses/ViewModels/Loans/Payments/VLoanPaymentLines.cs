namespace Applications.Responses.ViewModels.Loans.Payments
{
    public sealed class VLoanPaymentLines
    {
        public long? Id { get; set; }
        public long? LoanScheduleId { get; set; }
        public long? BranchId { get; set; }
        public long? LoanId { get; set; }
        public int InstallmentNo { get; set; }
        public DateTime? PayDate { get; set; }
        public string? OfficialReceiptNo { get; set; }
        public decimal? PaidPrinciple { get; set; } = 0;
        public decimal? PaidInterest { get; set; } = 0;
        public decimal? PaidCollectionFee { get; set; } = 0;
        public decimal? PenaltyAmount { get; set; } = 0;
        public decimal? SubTotal { get; set; } = 0;
    }
}
