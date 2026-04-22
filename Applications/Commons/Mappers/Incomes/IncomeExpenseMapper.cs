using Domains.Enumerations;
using Domains.Models.Lists.Customers;

namespace Applications.Commons.Mappers.Incomes;

public static class IncomeExpenseMapper
{
    private static IncomeExpenseTypes ParseType(string? type)
    {
        return Enum.TryParse(type, true, out IncomeExpenseTypes parsed)
            ? parsed
            : IncomeExpenseTypes.Income;
    }
    public static IncomeExpense ToIncomeExpenseCreate(this CreateIncomeExpenseDto dto)
    {
        return new IncomeExpense
        {
            Description = dto.Description,
            Types = ParseType(dto.Types)
        };
    }

    public static IncomeExpense ToIncomeExpenseEdit(this CreateIncomeExpenseDto dto)
    {
        return new IncomeExpense
        {
            Id = dto.Id,
            Description = dto.Description,
            Types = ParseType(dto.Types),
        };
    }


    public static CreateIncomeExpenseDto ToIncomeExpenseEdit(this IncomeExpense source)
    {
        return new CreateIncomeExpenseDto
        {
            Id = source.Id,
            Description = source.Description,
            Types = source.Types.ToString(),
        };
    }

    public static CreateIncomeExpenseDto ToIncomeExpenseEdit(this IncomeExpenseDto source)
    {
        return new CreateIncomeExpenseDto
        {
            Id = source.Id,
            Description = source.Description,
            Types = source.Types.ToString(),
        };
    }
    public static IncomeExpenseDto ToIncomeExpenseDetails(this CreateIncomeExpenseDto source)
    {
        return new IncomeExpenseDto
        {
            Id = source.Id,
            Description = source.Description,
            Types = source.Types.ToString(),
        };
    }
}
