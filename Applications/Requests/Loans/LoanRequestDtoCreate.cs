using Applications.Requests.Lists.Entities.Guarantors;
using Domains.Models.Collaterals;

namespace Applications.Requests.Loans;

public sealed class LoanRequestDtoCreate
{
    public long Id { get; set; } = 0;
    public long? CustomerRefId { get; set; }
    public int? LoanRequestStatusRefId { get; set; }
    public int? BranchRefId { get; set; }
    public long? RequestUserRefId { get; set; }
    public int? RepaymentModeRefId { get; set; }
    public int? LoanSubTypeRefId { get; set; }
    public int? LoanCategoryRefId { get; set; }
    public int? BankRefId { get; set; }
    public int? LoanProductRefId { get; set; }
    public long? CreditAgentRefId { get; set; }
    public string? InstallmentName { get; set; }
    public int? InstallmentTypeId { get; set; }
    public int? PayEvery { get; set; } = 1;
    public int? CurrencyRefId { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? FirstPaymentDate { get; set; } 
    public DateTime? LastPaymentDate { get; set; }
    public int? Installments { get; set; }
    public string? InstallmentType { get; set; }
    public string? RepaymentMode { get; set; }
    public decimal? RequestAmount { get; set; }
    public decimal? AprovedAmount { get; set; } 
    public decimal? InterestRate { get; set; }
    public int? CollectionFeeModeId { get; set; } = 1;
    public string? CollectionFeeModeName { get; set; } = string.Empty;
    public decimal? CollectionFeeRate { get; set; }
    public int? Circle { get; set; } = 1;
    public int? ApprovalLevel { get; set; }
    public string? AprovedUserId { get; set; }
    public DateTime? AprovedDate { get; set; } = DateTime.UtcNow;
    public string? ApprovedBy { get; set; }
    public string? Summiter { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public decimal? PenaltyRate { get; set; }
    public string? Remark { get; set; }
    public string? Memo { get; set; }

    public List<LoanGuarantorDtoCreate> GuarantorsLink { get; set; } = []; 
    public List<CollateralLinkDtoCreate> CollateralLinesLink { get; set; } = [];
    public List<LoanDocumentDtoCreate> LoanDocuments { get; set; } = [];
    public List<LoanDecriptionDtoCreate> LoanDescriptions { get; set; } = [];
}
