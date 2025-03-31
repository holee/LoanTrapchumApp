using Domains.Models.Bases;
using Domains.Models.Transaction;

namespace Domains.Models.Loans;

public sealed class Loan : BaseEntity
{
    public long Id { get; set; }
    public Guid LoanDisbursementsId { get; set; }
    public long? LoanRequestId { get; set; }
    public Guid? LoanRequestRefId { get; set; } 
    public Guid? CustomerRefId { get; set; }
    public long? UserRefId { get; set; }
    public long? TranxRefId { get; set; }
    public Guid? TransactionRefId { get; set; }
    public Guid LoanId { get; set; } = Guid.NewGuid();
    public Guid? AccIdInsurance { get; set; }
    public Guid? LinkWithAccountRefId { get; set; }
    public string? LoanAccountCode { get; set; }
    public string? InstallmentType { get; set; }
    public string? ContractNumber { get; set; }
    public string? AccountNumber { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? MaturityDate { get; set; }
    public DateTime? RegisterDate { get; set; } = DateTime.UtcNow;
    public int? LoanStatusRefId { get; set; }
    public int? RepaymentModeRefId { get; set; }
    public int? LoanSubTypeRefId { get; set; }
    public int? LoanCategoryRefId { get; set; }
    public int? BankRefId { get; set; }
    public int? LoanProductRefId { get; set; }
    public int? LoanCheckSum { get; set; }
    public int? Installments { get; set; }
    public int? CurrencyRefId { get; set; }
    public int? TermTypeId { get; set; }
    public int? InstallmentTypeId { get; set; }
    public int? DaysExceed { get; set; }
    public int? FundId { get; set; }
    public Guid? CreditAgentRefId { get; set; }
    public int? GracePeriodNo { get; set; }
    public int? GracePeriodTypeRefId { get; set; }
    public int? PaidInstallment { get; set; }
    public bool? IsGroupLoan { get; set; }
    public int? CustomerGroupCycle { get; set; }
    public int? CustomerIndividualCycle { get; set; }
    public int? LoanCompoundInterestModeRefId { get; set; }
    public int? PayEvery { get; set; }
    public int? PaymentLocationId { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsInsurance { get; set; }
    public bool? IsInterestOnGracePeriod { get; set; }
    public bool? IsEmergency { get; set; }
    public bool? IsPremiumExcluded { get; set; }
    public bool? IsStopAccruedInterest { get; set; }
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? ArrearRate { get; set; }
    public decimal? SavingCompousalry { get; set; }
    public decimal? InsurranFee { get; set; }
    public decimal? SavingNew { get; set; }
    public decimal? InsurranceNew { get; set; }
    public decimal? InsInstallment { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public decimal? RiskFeeRate { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? PenaltyRate { get; set; } = 0.02M;
    public decimal? AdminFeeAmount { get; set; }
    public decimal? SavingMeada { get; set; }
    public decimal? Premium { get; set; }
    public decimal? TruePrinciple { get; set; }
    public decimal? TmpAmount { get; set; }
    public decimal? AccruedArreaRate { get; set; }
    public decimal? LastBalance { get; set; }
    public decimal? LastAccrued { get; set; }
    public decimal? AccruedAmount { get; set; }
    public decimal? AccruedBalance { get; set; }
    public decimal? Adjustment { get; set; }
    public decimal? ProvisionForPrinciple { get; set; }
    public decimal? ProvisionForAccr { get; set; }
    public decimal? PaidPrinciple { get; set; }
    public decimal? PaidInterest { get; set; }
    public string? Remark { get; set; }
    public int? BranchRefId { get; set; }
    public string? BranchName { get; set; }
    public int? Circle { get; set; }
    public int? Period { get; set; }
    public string? PeriodName { get; set; } = "Months";
    public LoanRequest? LoanRequest { get; set; }
    public LoanTransaction? LoanTransaction { get; set; }
    public ICollection<LoanSchedule>? LoanSchedules { get; set; } = [];
   
}

//public Customer? Customer { get; set; }
//public LoanCreditAgent? LoanCreditAgent { get; set; }