using Domains.Models.Bases;
using Domains.Models.Collaterals;
using Domains.Models.Lists;

namespace Domains.Models.Loans;

public sealed class LoanRequest : BaseEntity
{
    public long Id { get; set; }
    public Guid LoanRequestId { get; set; }
    public Guid? CustomerRefId { get; set; }
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
    public int InstallmentTypeId { get; set; }
    public int PayEvery { get; set; } = 1;
    public int? CurrencyRefId { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? FirstPaymentDate { get; set; } = DateTime.UtcNow.AddMonths(1);
    public DateTime? LastPaymentDate { get; set; }
    public int? Installments { get; set; } = 12;
    public string? InstallmentType { get; set; }
    public string? RepaymentMode { get; set; }
    public decimal? RequestAmount { get; set; }
    public decimal? AprovedAmount { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public int? Circle { get; set; } = 1;
    public long? AprovedUserId { get; set; }
    public DateTime? AprovedDate { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public decimal? PenaltyRate { get; set; }
    public string? Remark { get; set; }
    public string? Memo { get; set; }
    public int? CollectionFeeModeId { get; set; } = 1;
    public string? CollectionFeeModeName { get; set; } = string.Empty;
    public LoanCreditAgent? LoanCreditAgent { get; set; }
    public List<LoanDocuments> LoanDocuments { get; set; } = [];
    public List<GuarantorLink> GuarantorsLink { get; set; } = [];
    public List<CollateralLink> CollateralLinesLink { get; set; } = [];
    public ICollection<LoanDescriptions> LoanDescriptions { get; set; } = [];

}
