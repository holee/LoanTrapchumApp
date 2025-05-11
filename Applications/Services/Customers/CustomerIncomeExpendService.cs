using Applications.Extensions.mappers;
using Applications.Interfaces.Customers;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Domains.Models.Lists.Customers;

namespace Applications.Services.Customers
{


    public class CustomerIncomeExpendService(ICustomerIncomeExpendService cusService) : IIncomeExpendServiceImpl
    {
        public async Task<Result> AddExpensesAsync(string? customerRefId, List<CustomerExpenseDtoCreate> req)
        {

            var model = req.MapList<CustomerExpense>();

            return await cusService.AddExpensesAsync(customerRefId, model);
        }

        public async Task<Result> AddIncomesAsync(string? customerRefId, List<CustomerIncomeDtoCreate> req)
        {
            var model = req.MapList<CustomerIncome>();

            return await cusService.AddIncomesAsync(customerRefId, model);
        }

        public async Task<Result> AddOrUpdateExpensesAsync(string? customerId, List<CustomerExpenseDtoCreate> req)
        {

            var model=req.MapList<CustomerExpense>();

            return await cusService.AddOrUpdateExpensesAsync(customerId, model);
        }

        public async Task<Result> AddOrUpdateIncomesAsync(string? customerId, List<CustomerIncomeDtoCreate> req)
        {
            var model = req.MapList<CustomerIncome>();

            return await cusService.AddOrUpdateIncomesAsync(customerId, model);
        }


        public async Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(string? customerId)
        {
            return await cusService.GetCustomerCustomerExpenses(customerId);
        }

        public async Task<Result<IEnumerable<T>>> GetCustomerCustomerExpenses<T>(string? customerId) where T : class
        {
            return await cusService.GetCustomerCustomerExpenses<T>(customerId);
        }

        public async Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(string? customerId)
        {
            return await cusService.GetCustomerCustomerIncomes(customerId);
        }

        public async Task<Result<IEnumerable<T>>> GetCustomerCustomerIncomes<T>(string? customerId) where T : class
        {
           return await cusService.GetCustomerCustomerIncomes<T>(customerId);
        }
    }
}
