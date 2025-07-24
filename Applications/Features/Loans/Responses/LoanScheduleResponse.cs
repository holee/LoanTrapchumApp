namespace Applications.Features.Loans.Responses;

public sealed class LoanScheduleResponse
{
    public int? CurrencyId { get; set; }
    public string? ContractNumber { get; set; }
    public string? BranchName { get; set; }
    public string? AltName { get; set; }
    public string? Phone { get; set; }
    public string? Salutation { get; set; }
    public DateTime? RegisterDate { get; set; }
    public string? CreditAgentKhmer { get; set; }
    public string? AgentPhone { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public decimal? Principle { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? OutStanding { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? Address { get; set; }
    public int? Circle { get; set; }
    public string? PeriodName { get; set; }
    public int? ScheduleNum { get; set; } = 1;
    public List<ScheduleResponse> Schedules { get; set; } = [];
}