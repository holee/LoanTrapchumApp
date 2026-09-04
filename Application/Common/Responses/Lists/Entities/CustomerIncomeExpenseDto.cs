using Domains.Models.Lists.Customers;

namespace Application.Responses.Lists.Entities;

public class CustomerIncomeExpenseDto
{
    public List<CustomerExpense>? CustomerExpenses { get; set; }
    public List<CustomerIncome>?  CustomerIncomes { get; set; } 
}
