using Applications.Features.Customers.Commands.ExpenseCommands.CreateExpenseCommands;
using Applications.Features.Customers.Commands.ExpenseCommands.EditExpensCommands;
using Applications.Features.Customers.Commands.Expenses.CreateExpensesCommands;
using Applications.Features.Customers.Commands.Expenses.Dto;
using Applications.Features.Customers.Commands.Expenses.EditExpenseCommands;
using Applications.Features.Customers.Commands.Expenses.Mapper;
using Domains.Models.Lists.Customers;

namespace Applications.Features.Customers.Commands.Expenses.Mapper
{
    public static class ExpenseMapper 
    {
        
        public static CustomerExpense ToCreateDto(this ExpenseDtoUpdate source)
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

        public static CustomerExpense ToEditDto(this ExpenseDtoUpdate source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new CustomerExpense
            {
                Id = source.Id,
                Amount = source.Amount,
                ExpenseTitle = source.ExpenseTitle,
                Description = source.Description,
                CustomerRefId = source.CustomerId,
            };
        }
        public static CustomerExpense ToDto(this EditExpenseCommand source) 
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Request.ToEditDto();
        }

        public static CustomerExpense ToDto(this ExpenseDtoCreate source) 
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new CustomerExpense
            {
                Amount = source.Amount,
                ExpenseTitle = source.ExpenseTitle,
                Description = source.Description,
                CustomerRefId = source.CustomerRefId,
            };
        }


        public static CustomerExpense ToDto(this AddExpenseCommand source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Request.ToDto();
        }


        public static List<CustomerExpense> ToDtos(this AddExpensesCommand source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Requests.Select(x => x.ToDto()).ToList();
        }

    }
}
