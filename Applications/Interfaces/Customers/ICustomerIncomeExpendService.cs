using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Interfaces.Customers
{
    public interface ICustomerIncomeExpendService
    {
        Task<Result> AddExpensesAsync(long customerRefId, List<CustomerExpense> req);
        Task<Result> AddIncomesAsync(long customerRefId, List<CustomerIncome> req);
        Task<Result> AddOrUpdateExpensesAsync(long customerId, List<CustomerExpense> req);
        Task<Result> AddOrUpdateIncomesAsync(long customerId, List<CustomerIncome> req);
        Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(long customerId);
        Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(long customerId);
    }
}
