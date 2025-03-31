using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Services.Customers
{
    public interface IIncomeExpendServiceImpl
    {
        Task<Result> AddExpensesAsync(Guid? customerRefId, List<CustomerExpenseDtoCreate> req);
        Task<Result> AddIncomesAsync(Guid? customerRefId, List<CustomerIncomeDtoCreate> req);
        Task<Result> AddOrUpdateExpensesAsync(Guid? customerId, List<CustomerExpenseDtoCreate> req);
        Task<Result> AddOrUpdateIncomesAsync(Guid? customerId, List<CustomerIncomeDtoCreate> req);
        Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(Guid? customerId);
        Task<Result<IEnumerable<T>>> GetCustomerCustomerExpenses<T>(Guid? customerId) where T : class;
        Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(Guid? customerId);
        Task<Result<IEnumerable<T>>> GetCustomerCustomerIncomes<T>(Guid? customerId) where T : class;
    }
}
