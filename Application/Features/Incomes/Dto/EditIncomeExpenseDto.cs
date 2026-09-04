namespace Application.Features.Incomes.Dto;

public sealed class EditIncomeExpenseDto 
{
    public int Id { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public string? Types { get; set; }
}
