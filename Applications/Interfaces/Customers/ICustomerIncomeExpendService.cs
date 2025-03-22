using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Interfaces.Customers
{
    public interface ICustomerIncomeExpendService
    {
        Task<Result> AddExpensesAsync(Guid? customerRefId, List<CustomerExpense> req);
        Task<Result> AddIncomesAsync(Guid? customerRefId, List<CustomerIncome> req);
        Task<Result> AddOrUpdateExpensesAsync(Guid? customerId, List<CustomerExpense> req);
        Task<Result> AddOrUpdateIncomesAsync(Guid? customerId, List<CustomerIncome> req);
        Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(Guid? customerId);
        Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(Guid? customerId);
    }
}
