using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Services.Customers
{
    public class CustomerIncomeExpendService : ICustomerIncomeExpendService
    {
        private readonly ICustomerIncomeExpendService _cusService;

        public CustomerIncomeExpendService(ICustomerIncomeExpendService cusService)
        {
            _cusService = cusService;
        }

        public async Task<Result> AddExpensesAsync(long customerRefId, List<CustomerExpense> req)
        {
            return await _cusService.AddExpensesAsync(customerRefId, req);
        }

        public async Task<Result> AddIncomesAsync(long customerRefId, List<CustomerIncome> req)
        {
            return await _cusService.AddIncomesAsync(customerRefId, req);
        }

        public async Task<Result> AddOrUpdateExpensesAsync(long customerId, List<CustomerExpense> req)
        {
            return await _cusService.AddOrUpdateExpensesAsync(customerId, req);
        }

        public async Task<Result> AddOrUpdateIncomesAsync(long customerId, List<CustomerIncome> req)
        {
            return await _cusService.AddOrUpdateIncomesAsync(customerId, req);
        }

        public async Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(long customerId)
        {
            return await _cusService.GetCustomerCustomerExpenses(customerId);
        }

        public async Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(long customerId)
        {
            return await _cusService.GetCustomerCustomerIncomes(customerId);
        }
    }
}
