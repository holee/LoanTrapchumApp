using Applications.Features.Customers.Commands.ExpenseCommands.EditExpensCommands;
using Applications.Responses.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Customers.Commands.Expenses.Mapper
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
