namespace Applications.Features.Incomes.Dto;

public sealed class CreateIncomeExpenseDto
{
    public int Id { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public string? Types { get; set; }
}
public sealed class UpdateIncomeExpenseDto
{
    public int Id { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public string? Types { get; set; }
}
