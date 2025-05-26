using Applications.Calculations;
using Applications.Responses.ViewModels.Holidays;
using Domains.Enumerations;

namespace Applications.Requests.Loans.Disbursements;

public sealed class LoanDtoCreate : CalculatorBase, ICalculatorBase
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
    public string? LoanAccountCode { get; set; }
    public int? Period { get; set; }
    public string? PeriodName { get; set; }
    public int? CurrencyRefId { get; set; }
    public decimal? PenaltyRate { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? FirstPaymentDate { get; set; }
    public DateTimeOffset? LastPaymentDate { get; set; }
    public int? TermTypeId { get; set; }
    public int? InstallmentTypeId { get; set; }
    public string? CustomerRefId { get; set; }
    public DateTimeOffset? RegisterDate { get; set; }
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
    public List<HolidayVm> Holidays { get; set; } = [];
    public List<LoanScheduleDtoCreate>? LoanSchedules { get; set; } = [];
}


