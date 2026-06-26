using Applications.Calculations;
using Applications.Features.Loans.Dtos.Requests;
using Domains.Enumerations;

namespace Applications.Features.Loans.Dtos;

public sealed class LoanDtoUpdate : CalculatorBase, ICalculatorBase
{
    public string? LoanId { get; set; }
    public string? LoanRequestId { get; set; }
    public int? LoanStatusRefId { get; set; }
    public int? CollectionFeeModeId { get; set; } = 1;
    public int? LoanSubTypeRefId { get; set; }
    public int? LoanCategoryRefId { get; set; }
    public int? BankRefId { get; set; } = 11;
    public int? InstallmentNo { get; set; }
    public string? UserRefId { get; set; }
    public int? LoanProductRefId { get; set; }
    public long LoanAccountCode { get; set; }
    public int? Period { get; set; }
    public string? PeriodName { get; set; }
    public decimal? PenaltyRate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public int? TermTypeId { get; set; }
    public int? InstallmentTypeId { get; set; }
    public string? CustomerRefId { get; set; }
    public DateTime? RegisterDate { get; set; }
    public int? PayEvery { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public string? InstallmentType { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public string? OtherChargName { get; set; } = string.Empty;
    public string? Remark { get; set; }
    public string? BranchName { get; set; }
    public int Circle { get; set; } = 1;
    public string? CustomerName { get; set; } = string.Empty;
    public string? CreditAgentName { get; set; } = string.Empty;
    public string? RepaymentModeName { get; set; } = string.Empty;
    public string? LoanCategoryName { get; set; } = string.Empty;
    public string? CurrencyName { get; set; } = string.Empty;
    public string? InstallmentName { get; set; } = string.Empty;
    public string? AprovedUserId { get; set; }
    public string? ApprovedBy { get; set; } = string.Empty;
    public decimal? PaidAmount => Principle - AdminFeeAmount;
    public int PayementSechuleModeId { get; set; } = 2;
    public AdjustmentOption ScheduleMode { get; set; } = AdjustmentOption.After;
    public List<LoanScheduleDtoEdit>? LoanSchedules { get; set; } = [];
    public List<LoanGuarantorDtoEdit>? Guarantors { get; set; } = [];
    public List<LoanCollateralLinesDtoEdit>? Collaterals { get; set; } = [];
    public List<LoanDescriptionDtoEdit>? Descriptions { get; set; } = [];
    public List<CoPawnCustomerDtoEdit>? CoPawCustomers { get; set; } = [];
    public List<LoanDocumentsDtoEdit>? Documents { get; set; } = []; 
}
