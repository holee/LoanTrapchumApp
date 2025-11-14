using Domains.Enumerations;

namespace Domains.Models.Lists.Customers;


public sealed class IncomeExpense
{
    public int Id { get; set; }
    public string Description { get; set; }=string.Empty;
    public IncomeExpenseTypes Types { get; set; } = IncomeExpenseTypes.Income;
}
