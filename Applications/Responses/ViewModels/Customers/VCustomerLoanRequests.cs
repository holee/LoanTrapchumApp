namespace Applications.Responses.ViewModels.Customers;

public sealed class VCustomerLoanRequests
{
    public int CustomerId { get; set; }
    public int LoanRequestId { get; set; }
    public DateTime RequestDate { get; set; }
    public decimal RequestAmount { get; set; }
    public decimal AprovedAmount { get; set; }
    public float InterestRate { get; set; }
    public float AdminFeeRate { get; set; }
    public int Installments { get; set; }
    public string? InstallmentName { get; set; } // Combined: "Installments + InstallmentName"
    public string? InstallmentType { get; set; }
    public string? BranchName { get; set; }
    public int? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public string? CreditAgentName { get; set; } // From Summiter
    public string? AprovedBy { get; set; }
    public string? Status { get; set; }

    //public decimal? CollectionFeeRate { get; set; }
    //public string? RepaymentMode { get; set; }
    //public string? AprovedUserId { get; set; }
    //public DateTime? AprovedDate { get; set; }
    //public string? Summiter { get; set; }
    //public string? Remark { get; set; }
    //public int? BranchId { get; set; }
    //public string? AltName { get; set; }
    //public decimal? AdminFeeAmount { get; set; }
    //public int Circle { get; set; } = 0;
    //public int? PayEvery { get; set; } = 1;
    //public string? CollectionFeeModeName { get; set; } = string.Empty;
}
