using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Services.Customers
{
    public interface IIncomeExpendServiceImpl
    {
        Task<Result> AddExpensesAsync(string? customerRefId, List<CustomerExpenseDtoCreate> req);
        Task<Result> AddIncomesAsync(string? customerRefId, List<CustomerIncomeDtoCreate> req);
        Task<Result> AddOrUpdateExpensesAsync(string? customerId, List<CustomerExpenseDtoCreate> req);
        Task<Result> AddOrUpdateIncomesAsync(string? customerId, List<CustomerIncomeDtoCreate> req);
        Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(string? customerId);
        Task<Result<IEnumerable<T>>> GetCustomerCustomerExpenses<T>(string? customerId) where T : class;
        Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(string? customerId);
        Task<Result<IEnumerable<T>>> GetCustomerCustomerIncomes<T>(string? customerId) where T : class;
    }
}
