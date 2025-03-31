namespace Applications.Requests.Loans.Disbursements;

public sealed class LoanDtoCreate
{
    public Guid? LoanRequestId { get; set; }
    public int? LoanStatusRefId { get; set; }
    public int? BranchId { get; set; } 
    public int? RepaymentModeRefId { get; set; }
    public int? LoanSubTypeRefId { get; set; }
    public int? LoanCategoryRefId { get; set; }
    public int? BankRefId { get; set; } = 11;
    public Guid? UserRefId { get; set; }
    public int? LoanProductRefId { get; set; }
    public string? LoanAccountCode { get; set; }
    public decimal? Principle { get; set; } = 0;
    public int? Period { get; set; }
    public string? PeriodName { get; set; }
    public int? Installments { get; set; }
    public int? CurrencyRefId { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? PenaltyRate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public int? TermTypeId { get; set; }
    public int? InstallmentTypeId { get; set; }
    public Guid? CreditAgentRefId { get; set; }
    public Guid? CustomerRefId { get; set; }
    public DateTime? RegisterDate { get; set; }
    public int? PayEvery { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public string? InstallmentType { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public int? OtherChargeId { get; set; } = 1;
    public string? OtherChargName { get; set; } = string.Empty;
    public string? Remark { get; set; }
    public int? BranchRefId { get; set; }
    public string? BranchName { get; set; }
    public int Circle { get; set; } = 1;
    public string? CustomerName { get; set; } = string.Empty;
    public string? CreditAgentName { get; set; } = string.Empty;
    public string? RepaymentModeName { get; set; } = string.Empty;
    public string? LoanCategoryName { get; set; } = string.Empty;
    public string? CurrencyName { get; set; } = string.Empty;
    public string? InstallmentName { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; } = string.Empty;
    public decimal? PaidAmount => Principle - AdminFeeAmount;
    public List<LoanScheduleDtoCreate>? LoanSchedules { get; set; } = [];
}



//public List<GuarantorDtoCreate>? LoanGuarantors { get; set; } = [];
//public List<CollateralLineDtoCreate>? LoanCollateralLines { get; set; } = [];
//public decimal? ArrearRate { get; set; }
//public decimal? PaidPrinciple { get; set; }
// public decimal? PaidInterest { get; set; }
//public int? PaidInstallment { get; set; }
// public decimal? InsInstallment { get; set; }
//public int? PaymentLocationId { get; set; }