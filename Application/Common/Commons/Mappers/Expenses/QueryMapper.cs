using Application.Features.Expenses.Dto;
using Application.Responses.ViewModels.Customers;

namespace Application.Commons.Mappers.Expenses
{
    public static class QueryMapper
    {
        public static ExpenseDtoUpdate ToExpenseEdit(this VCustomerExpenses source)
        {
            return new ExpenseDtoUpdate
            {
                Id = source.ExpendId,
                Amount = source.ExpendAmount,
                ExpenseTitle = source.ExpenseTitle,
                Description = source.ExDescription,
                CustomerId = source.CustomerRefId
            };
        }
    }
}
