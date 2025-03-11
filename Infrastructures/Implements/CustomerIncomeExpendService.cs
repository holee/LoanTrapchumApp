using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Dapper;
using Domains.Models.Lists.Customers;
using Infrastructures.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoreBussiness.Implements
{
    public class CustomerIncomeExpendService : ICustomerIncomeExpendService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerIncomeExpendService> _logger;

        public CustomerIncomeExpendService(ApplicationDbContext context, ILogger<CustomerIncomeExpendService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> AddIncomesAsync(long customerRefId, List<CustomerIncome> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var customer = await _context.Customers
                                            .Include(x => x.CustomerIncomes)
                                            .FirstOrDefaultAsync(x => x.CustomerId == customerRefId);
                if (customer is null)
                {
                    return Result.Failure(ErrorResponse.NotFound("no customer is selected"));
                }

                customer.CustomerIncomes = req;

                await _context.SaveChangesAsync();

                return Result.Success("customer income was added.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }


        }

        public async Task<Result> AddOrUpdateIncomesAsync(long customerId, List<CustomerIncome> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var customer = await _context.Customers
                                            .Include(x => x.CustomerIncomes)
                                            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
                if (customer is null)
                {
                    return Result.Failure(ErrorResponse.NotFound("no customer is selected"));
                }


                foreach (var item in req)
                {
                    var existingIncomes = customer.CustomerIncomes?.Where(x => x.Id == item.Id)
                                                                 .FirstOrDefault();

                    if (existingIncomes is null || existingIncomes.Id == 0)
                    {
                        customer.CustomerIncomes?.Add(item);
                    }
                    else
                    {
                        _context.Entry(existingIncomes).CurrentValues.SetValues(item);
                    }
                }

                foreach (var item in customer.CustomerIncomes!)
                {
                    if (!req.Any(x => x.Id == item.Id))
                    {
                        _context.Remove(item);
                    }

                }
                //customer?.CustomerIncomes?.AddRange(req);

                await _context.SaveChangesAsync();

                return Result.Success("customer income was added.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }


        }

        public async Task<Result> AddExpensesAsync(long customerRefId, List<CustomerExpense> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var customer = await _context
                              .Customers
                              .Include(x => x.CustomerExpenses)
                              .FirstOrDefaultAsync(x => x.CustomerId == customerRefId);

                if (customer is null)
                {
                    return Result.Failure(ErrorResponse.NotFound("no customer is selected"));
                }
                customer.CustomerExpenses = req;
                await _context.SaveChangesAsync();

                return Result.Success("customer expense was added.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure(ErrorResponse.ServerError("{ex.InnerException!.Message}"));
            }

        }

        public async Task<Result> AddOrUpdateExpensesAsync(long customerId, List<CustomerExpense> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var existingCustomer = await _context.Customers
                                            .Include(x => x.CustomerExpenses)
                                            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

                if (existingCustomer is null)
                {
                    return Result.Failure(ErrorResponse.NotFound("no customer is selected"));
                }

                foreach (var item in req)
                {
                    var existingExpense = existingCustomer.CustomerExpenses?.Where(x => x.Id == item.Id)
                                                                 .FirstOrDefault();

                    if (existingExpense is null || existingExpense.Id == 0)
                    {
                        existingCustomer.CustomerExpenses?.Add(item);
                    }
                    else
                    {
                        _context.Entry(existingExpense).CurrentValues.SetValues(item);
                    }
                }

                foreach (var item in existingCustomer.CustomerExpenses!)
                {
                    if (!req.Any(x => x.Id == item.Id))
                    {
                        _context.Remove(item);
                    }

                }
                await _context.SaveChangesAsync();

                return Result.Success("customer expense was added.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }

        }

        public async Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(long customerId)
        {

            using var db = _context.Database.GetDbConnection();

            var sql = @"SELECT * FROM VCustomerIncomes WHERE CustomerRefId=@customerId";

            var incomes = db.Query<VCustomerIncomes>(sql, new { @customerId = customerId })
                             .ToList();

            var incomeResult = await Task.FromResult(incomes);

            return Result.Success<IEnumerable<VCustomerIncomes>>(incomeResult);
        }
        public async Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(long customerId)
        {

            using var db = _context.Database.GetDbConnection();

            var sql = @"SELECT * FROM VCustomerExpenses WHERE CustomerRefId=@customerId";

            var expenses = db.Query<VCustomerExpenses>(sql, new { @customerId = customerId })
                              .ToList();

            var expensesResult = await Task.FromResult(expenses);

            return Result.Success<IEnumerable<VCustomerExpenses>>(expensesResult);
        }



    }
}
