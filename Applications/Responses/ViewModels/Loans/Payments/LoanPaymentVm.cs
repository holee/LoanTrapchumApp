namespace Applications.Responses.ViewModels.Loans.Payments
{
    public sealed class LoanPaymentVm 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? TransactionRefId { get; set; }
        public string? LoanRefId { get; set; }
        public string? CreditAgentRefId { get; set; }
        public string? UserRefId { get; set; }
        public int? BranchRefId { get; set; }
        public int? CurrencyRefId { get; set; }
        public string? OfficialReceiptNo { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? Principle { get; set; }
        public decimal? Interest { get; set; }
        public decimal? CollectionFeeAmount { get; set; }
        public decimal? PenaltyAmount { get; set; }
        public decimal? SubTotal { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public bool? IsReturn { get; set; } = false;
    }
}
