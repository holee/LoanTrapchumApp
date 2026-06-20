namespace Applications.Features.Reports.Dtos;

public sealed class RptScheduleDetails 
{
    public string? LoanId { get; set; }
    public string? LoanAccount { get; set; }
    public string? ContractNumber { get; set; }
    public string? BranchName { get; set; }
    public string? AltName { get; set; }
    public string? Phone { get; set; }
    public string? Salutation { get; set; }
    public DateTime? RegisterDate { get; set; }
    public string? ManagerId { get; set; } = string.Empty;
    public string? AgentName { get; set; }
    public string? CreditAgentKhmer { get; set; }
    public string? AgentPhone { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public int? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? Principle { get; set; }
    public decimal? Amount { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? OutStanding { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? Address { get; set; }
    public int? Circle { get; set; }
    public int? Period { get; set; }
    public string? PeriodName { get; set; }
    public int? ScheduleNum { get; set; } = 0;
    public FontModel FontModel { get; set; } = new();
    public List<RptScheduleDetailsLine> Schedules { get; set; } = [];
}
