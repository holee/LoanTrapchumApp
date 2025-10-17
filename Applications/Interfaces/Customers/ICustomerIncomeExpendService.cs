using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Interfaces.Customers
{
    public interface ICustomerIncomeExpendService
    {
        Task<Result<string>> AddExpensesAsync(string? customerRefId, List<CustomerExpense> req);
        Task<Result<string>> AddIncomesAsync(string? customerRefId, List<CustomerIncome> req);
        Task<Result<string>> AddOrUpdateExpensesAsync(string? customerId, List<CustomerExpense> req);
        Task<Result<VCustomerIncomes>> AddOrUpdateIncomeAsync(string? customerId, CustomerIncome req);
        Task<Result<string>> AddOrUpdateIncomesAsync(string? customerId, List<CustomerIncome> req);
        Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(string? customerId);
        Task<Result<IEnumerable<T>>> GetCustomerCustomerExpenses<T>(string? customerId) where T : class;
        Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(string? customerId);
        Task<Result<IEnumerable<T>>> GetCustomerCustomerIncomes<T>(string? customerId) where T : class;
    }
}
