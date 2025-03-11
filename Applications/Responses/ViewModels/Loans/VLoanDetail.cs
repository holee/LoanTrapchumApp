using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Responses.ViewModels.Loans;

public sealed class VLoanDetail
{
    public long Id { get; set; }
    public string? AgentName { get; set; }
    public string? AgentPhone { get; set; }
    public long? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? ContractId { get; set; }
    public string? AccountNumber { get; set; }
    public int Circle { get; set; }
    public string? Address { get; set; }
    public DateTime? StartDate { get; set; }
    public int? Period { get; set; }
    public decimal? Amount { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? CollectionFee { get; set; }
    public decimal? AdminFee { get; set; }
    public DateTime? LastDate { get; set; }
    public ICollection<VSchedule>? Schedules { get; set; }
}
