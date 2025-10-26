namespace Applications.Features.Customers.Commands.IncomeCommands.CreateIncomeCommands;

public sealed class IncomeDtoCreate
{
    public long Id { get; set; }
    public string? CustomerId { get; set; }
    public string? IncomeSource { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
}


