using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Responses.ViewModels.Loans.Requests;

public sealed class VLoanRequests
{
    public long LoanRequestId { get; set; }
    public DateTime? RequestDate { get; set; }
    public decimal? RequestAmount { get; set; }
    public decimal? AprovedAmount { get; set; } 
    public decimal? InterestRate { get; set; }
    public int? Installments { get; set; }
    public string? InstallmentType { get; set; }
    public string? RepaymentMode { get; set; }
    public string? AprovedUserId { get; set; }
    public DateTime? AprovedDate { get; set; }
    public string? Summiter { get; set; }
    public string? Remark { get; set; }
    public string? Memo { get; set; }
    public int? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public int? StatusId { get; set; }
    public string? Status { get; set; }
    public long CreditAgentId { get; set; }
    public string? UserRequestId { get; set; } 
    public string? UserName { get; set; }
    public int? BankId { get; set; }
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
    public long? CustomerId { get; set; }
    public string? AltName { get; set; }
    public string? FullName { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public int Circle { get; set; } = 0;
    public decimal? CollectionFeeRate { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? InstallmentName { get; set; }
    public int? InstallmentTypeId { get; set; }
    public int? RepaymentModeRefId { get; set; }
    public int? LoanCategoryRefId { get; set; }
    public int? LoanProductRefId { get; set; }
    public string? LoanCategoryName { get; set; } = string.Empty;
    public int? PayEvery { get; set; } = 1;
    public int? CollectionFeeModeId { get; set; } = 1;
    public string? CollectionFeeModeName { get; set; } = string.Empty;
    public string? AprovedBy { get; set; }=string.Empty;
    [NotMapped] public IEnumerable<VLoanRequestGuarantors> Guarantors { get; set; } = [];
    [NotMapped] public IEnumerable<VLoanRequestCollateralLines> Collaterals { get; set; } = [];
    [NotMapped] public IEnumerable<VLoanRequestsDocuments> Documents { get; set; } = [];
    [NotMapped] public VCustomerList Customer { get; set; } = new();
}
