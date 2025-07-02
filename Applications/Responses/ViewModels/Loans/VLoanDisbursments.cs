using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Responses.ViewModels.Loans;

public sealed class VLoanDisbursments
{
    public string? LoanId { get; set; }
    public string? LoanRequestId { get; set; }
    public int? BranchId { get; set; }
    public string? ContractNumber { get; set; }
    public string? BranchName { get; set; }
    public string? CustomerId { get; set; }
    public string? AltName { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Salutation { get; set; }
    public DateTime? RegisterDate { get; set; }
    public string? CreditAgentRefId { get; set; }
    public string? AgentName { get; set; }
    public string? CreditAgentKhmer { get; set; }
    public string? AgentPhone { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public int? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? Principle { get; set; } 
    public decimal? Amount { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? CollectionFeeRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? PaidPrinciple { get; set; }
    public decimal? OutStanding { get; set; }
    public decimal? PaidInterest { get; set; }
    public decimal? PaidCollectionFee { get; set; }
    public decimal? TotalOtherCharge { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? LoanStatus { get; set; }
    public int LoanstatusId { get; set; }
    public string? Address { get; set; }
    public int? Circle { get; set; }
    public int? Period { get; set; }
    public string? PeriodName { get; set; }
    public string? InstallmentType { get; set; }
    public ICollection<VSchedule>? Schedules { get; set; } = [];
}
