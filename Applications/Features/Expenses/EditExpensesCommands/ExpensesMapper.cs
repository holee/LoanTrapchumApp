using Domains.Models.Lists.Customers;

namespace Applications.Features.Expenses.EditExpensesCommands
{
    public static class ExpensesMapper 
    {
        public static CustomerExpense ToDto(this ExpensesDtoUpdate source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new CustomerExpense
            {
                Amount = source.Amount,
                ExpenseTitle = source.ExpenseTitle,
                Description = source.Description,
                CustomerRefId = source.CustomerId,
            };
        }


        public static List<CustomerExpense> ToDtos(this EditExpensesCommand source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Requests.Select(x => x.ToDto()).ToList();
        }


    }
}
