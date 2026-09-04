namespace Application.Responses.ViewModels.Reports;

public sealed class DisbursmentsRpt
{
    public string? FullName { get; set; }
    public string? ContractNumber { get; set; }
    public string? CustomerNumber { get; set; } 
    public string? AccountNumber { get; set; } 
    public string? BranchName { get; set; }
    public string? Phone { get; set; }
    public DateTime? RegisterDate { get; set; }
    public string? AgentName { get; set; }
    public string? AgentPhone { get; set; }
    public string? Principle { get; set; }
    public decimal? TotalPrinciple { get; set; }
    public decimal? TotalInterest { get; set; }
    public string? LastPaymentDate { get; set; }
    public string? Address { get; set; }
    public string? PeriodName { get; set; }
    public int? Circle { get; set; }
    public int? TotalDay { get; set; }
    public int? CurrencyId { get; set; } = 1;
    public byte[]? LogoUrl { get; set; } = [];
    public ICollection<ScheduleRpt>? Schedules { get; set; } = [];
}
