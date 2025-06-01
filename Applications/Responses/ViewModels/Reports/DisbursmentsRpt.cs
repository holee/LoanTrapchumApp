namespace Applications.Responses.ViewModels.Reports;

public sealed class DisbursmentsRpt
{
    public string? CustomerName { get; set; }
    public string? ContractNumber { get; set; }
    public string? AccountNumber { get; set; } 
    public string? BranchName { get; set; }
    public string? Phone { get; set; }
    public string? Salutation { get; set; }
    public string? RegisterDate { get; set; }
    public string? AgentName { get; set; }
    public string? AgentPhone { get; set; }
    public string? Principle { get; set; }
    public string? LastPaymentDate { get; set; }
    public string? Address { get; set; }
    public int? Circle { get; set; }
    public int? Period { get; set; }
    public ICollection<ScheduleRpt>? Schedules { get; set; } = [];
}
