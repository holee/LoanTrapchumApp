using Applications.Features.Expenses.Dto;
using Applications.Responses.ViewModels.Customers;

namespace Applications.Commons.Mappers.Expenses
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
