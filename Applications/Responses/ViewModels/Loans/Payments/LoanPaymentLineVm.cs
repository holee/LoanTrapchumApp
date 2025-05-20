namespace Applications.Responses.ViewModels.Loans.Payments
{
    public sealed class LoanPaymentLineVm 
    {
        public long Id { get; set; }
        public string? LoanPaymentRefId { get; set; }
        public string? LoanRefId { get; set; }
        public long? LoanScheduleRefId { get; set; }
        public int? BranchRefId { get; set; }
        public int? InstallmentNo { get; set; }
        public int? NumberOfDay { get; set; }
        public DateTime? PayDate { get; set; } = DateTime.Now;
        public int? DayArrea { get; set; }
        public string? OfficialReceiptNo { get; set; }
        public decimal? PaidPrinciple { get; set; } = 0;
        public decimal? PaidInterest { get; set; } = 0;
        public decimal? PaidCollectionFee { get; set; } = 0;
        public decimal? PenaltyAmount { get; set; } = 0;
        public decimal? SubTotal { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
    }
}
