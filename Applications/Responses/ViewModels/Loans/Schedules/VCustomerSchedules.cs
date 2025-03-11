namespace Applications.Responses.ViewModels.Loans.Schedules;

public sealed class VCustomerSchedules
{

    public long? LoanId { get; set; }
    public string? ContractNumber { get; set; }
    public string? BranchName { get; set; }
    public string? CustomerKhmerName { get; set; } 
    public string? CustomerName { get; set; } 
    public string? CustomerPhone { get; set; }
    public string? Salutation { get; set; }
    public DateTime? RegisterDate { get; set; }
    public string? AgentName { get; set; }
    public string? AgentKhmerName { get; set; } 
    public string? AgentPhone { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? AdminFeeAmount { get; set; }
    public decimal? AdminFeeRate { get; set; }
    public decimal? OutStanding { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? Address { get; set; }
    public int? Circle { get; set; }
    public int? Period { get; set; }
    public string? PeriodName { get; set; }
    public string? InstallmentType { get; set; }
    public ICollection<VLoanSchedules> LoanSchedules { get; set; } = [];
}
