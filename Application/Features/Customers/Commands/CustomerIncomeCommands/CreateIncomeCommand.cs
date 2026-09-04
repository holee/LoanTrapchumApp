namespace Application.Features.Customers.Commands.CustomerIncomeCommands;

public sealed class CreateIncomeCommand
{
    public string? CustomerRefId { get; set; }
    public string? IncomeSource { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }

}

