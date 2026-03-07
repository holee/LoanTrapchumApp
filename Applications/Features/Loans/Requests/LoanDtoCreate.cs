using Applications.Calculations;
using Applications.Enumerations;
using Domains.Enumerations;

namespace Applications.Features.Loans.Requests;

public static class LoanDtoCreateExtension
{
    public static LoanDtoCreate ToLoanDtoCreate(this LoanDtoCreate loanDtoCreate,
        int? branchId,
        string? userId,
        decimal? penaltyRate,
        int? loanStatusRefId,
        int? loanProductRefId,
        int? loanSubTypeRefId,
        int? bankId = 11,
        int? termTypeId = 11)
    {
        return new LoanDtoCreate(
            branchId,
            userId,
            penaltyRate,
            loanStatusRefId,
            loanProductRefId,
            loanSubTypeRefId,
            bankId,
            termTypeId);
    }
}

public sealed class LoanDtoCreate : CalculatorBase, ICalculatorBase
{
    public LoanDtoCreate()
    {

    }
    public LoanDtoCreate(
           int? branchId,
           string? userId,
           decimal? penaltyRate,
           int? LoanStatusRefId,
           int? loanProductRefId,
            int? loanSubTypeRefId,
           int? bankId = 11,
           int? termTypeId = 11)
    {
        BranchId = branchId;
        UserRefId = userId;
        PenaltyRate = penaltyRate;
        TermTypeId = termTypeId;
        BankRefId = bankId;
        LoanProductRefId = loanProductRefId ?? (int)ProductTypeOption.Mortgage;
        LoanStatusRefId = LoanStatusRefId ?? (int)LoanStatuses.Active;
        LoanSubTypeRefId = loanSubTypeRefId ?? (int)LoanSubTypeOption.Standard;
    }

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
    public int? CurrencyRefId { get; set; }
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
    public List<HolidayVm> Holidays { get; set; } = [];
    public List<LoanScheduleDtoCreate>? LoanSchedules { get; set; } = [];
}


