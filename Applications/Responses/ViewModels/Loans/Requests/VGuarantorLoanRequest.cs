namespace Applications.Responses.ViewModels.Loans.Requests
{
    public sealed class VGuarantorLoanRequest  
    {
        public string? EntityNameRefId { get; set; }
        public string? BranchName { get; set; }
        public string? LoanRequestId { get; set; }
        public decimal? RequestAmount { get; set; }
        public decimal? AprovedAmount { get; set; }
        public string? CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? CollectionFeeRate { get; set; }
        public decimal? AdminFeeRate { get; set; }
        public string? CreditAgentName { get; set; }
        public string? AprovedBy { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? Remark { get; set; }
        public string? Status { get; set; }
    }

}
