using Applications.Extensions.mappers;
using Applications.Features.Customers.Services;
using Applications.Features.Entities;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Domains.Models.Lists.Customers;

namespace Applications.Features.Customers.UseCases
{

 
    public class CustomerIncomeExpendService(ICustomerIncomeExpendService cusService) : IIncomeExpendServiceImpl 
    {
        //public async Task<Result> AddExpensesAsync(string? customerRefId, List<AddExpensesDtoCreate> req)
        //{

        //    var model = req.MapList<CustomerExpense>();

        //    return await cusService.AddExpensesAsync(customerRefId, model);
        //}

        //public async Task<Result> AddIncomesAsync(string? customerRefId, List<CustomerIncomeDtoCreate> req)
        //{
        //    var model = req.MapList<CustomerIncome>();

        //    return await cusService.AddIncomesAsync(customerRefId, model);
        //}

        //public async Task<Result> AddOrUpdateExpensesAsync(string? customerId, List<AddExpensesDtoCreate> req)
        //{

        //    var model=req.MapList<CustomerExpense>();

        //    return await cusService.AddOrUpdateExpensesAsync(customerId, model);
        //}

  

        //public async Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(string? customerId)
        //{
        //    return await cusService.GetExpenses(customerId);
        //}

       

        //public async Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(string? customerId)
        //{
        //    return await cusService.GetCustomerCustomerIncomes(customerId);
        //}

       
    }
}
