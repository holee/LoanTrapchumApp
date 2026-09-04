using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Features.Incomes.Dto;

public sealed class IncomeExpenseDto
{
    public int Id { get; set; }
    public string Description { get; set; }=string.Empty;
    public string Types { get; set; } = string.Empty;
    [NotMapped] public int RowIndex { get; set; } = 1;
}
