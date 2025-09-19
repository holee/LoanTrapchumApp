using Applications.Features.Collaterals.Requests;
using Applications.Features.Entities.Guarantors;
using Domains.Models.Loans;

namespace Applications.Features.LoanRequests.Requests;

public sealed class LoanRequestDtoUpdate 
{

    public long Id { get; set; }
    public long? CustomerRefId { get; set; }
    public int? LoanRequestStatusRefId { get; set; }
    public int? BranchRefId { get; set; }
    public long? UserRefId { get; set; }
    public int RepaymentModeRefId { get; set; }
    public int LoanSubTypeRefId { get; set; }
    public int LoanCategoryRefId { get; set; }
    public int BankRefId { get; set; }
    public int LoanProductRefId { get; set; }
    public long CreditAgentRefId { get; set; }
    public string? InstallmentName { get; set; }
    public int InstallmentTypeId { get; set; }
    public int PayEvery { get; set; }
    public int? CurrencyRefId { get; set; }
    public DateTime? RequestDate { get; set; }
    public int? Installments { get; set; }
    public string? InstallmentType { get; set; }
    public string? RepaymentMode { get; set; }
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public int? ApprovalLevel { get; set; }
    public long? AprovedUserId { get; set; }
    public string? ApprovedBy { get; set; }
    public string? Summiter { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public decimal? PenaltyRate { get; set; }
    public string? Remark { get; set; }
    public string? Memo { get; set; }
    public ICollection<LoanRequestSchedule> LoanRequestSchedules { get; set; } = [];
    public List<GuarantorDtoCreate>? LoanGuarantors { get; set; } = [];
    public List<CollateralLineDtoCreate>? LoanCollateralLines { get; set; } = [];
}
