namespace Domains.Models.Loans
{
    public sealed class LoanPayment
    {
        public string Id { get; set; } = $"rec-{Guid.CreateVersion7()}";
        public string? TransactionRefId { get; set; } 
        public string? LoanRefId { get; set; }
        public string? CreditAgentRefId { get; set; }
        public string? UserRefId { get; set; }
        public int? BranchRefId { get; set; }
        public string? OfficialReceiptNo { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? Principle { get; set; }
        public decimal? Interest { get; set; }
        public decimal? CollectionFeeAmount { get; set; }
        public decimal? PenaltyAmount { get; set; }
        public decimal? SubTotal { get; set; }
        public bool? IsReturn { get; set; }
        public int? CurrencyRefId { get; set; }
        public ICollection<LoanPaymentLine>? LoanRepaymentLines { get; set; } = [];
    }
}
