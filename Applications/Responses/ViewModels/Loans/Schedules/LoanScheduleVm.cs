namespace Applications.Responses.ViewModels.Loans.Schedules;

public sealed class LoanScheduleVm 
{

    public string LoanId { get; set; } = string.Empty;
    public string? ContractNumber { get; set; }=string.Empty;   
    public string? BranchName { get; set; } = string.Empty;
    public string? CustomerKhmerName { get; set; } =string.Empty;
    public string? FullName { get; set; } =string.Empty;
    public string? CustomerPhone { get; set; }
    public string? Salutation { get; set; } = string.Empty;
    public DateTime? RegisterDate { get; set; }
    public string? AgentName { get; set; } = string.Empty;
    public string? AgentKhmerName { get; set; }      
    public string? AgentPhone { get; set; }

    public int? CurrencyId { get; set; } 
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
    public ICollection<ScheduleViewModel> LoanSchedules { get; set; } = [];
}
