namespace Application.Features.Incomes.Dto;

public sealed class EditIncomeDto
{
    public EditIncomeDto()
    {
    }

    public EditIncomeDto(long id, string? customerId, string? incomeSource, decimal? amount, string? description)
    {
        Id = id;
        CustomerId = customerId;
        IncomeSource = incomeSource;
        Amount = amount;
        Description = description;
    }

    public long Id { get; set; }
    public string? CustomerId { get; set; }
    public string? IncomeSource { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
}
