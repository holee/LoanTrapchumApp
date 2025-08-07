using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Dapper;
using Domains.Models.Lists.Customers;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class IncomeExpendRepository : ICustomerIncomeExpendService
    {
        private readonly DataContext _context;

        private readonly ILogger<IncomeExpendRepository> _logger;

        private readonly IDapperConnectionFactory connectionFactory;
        public IncomeExpendRepository(DataContext context, ILogger<IncomeExpendRepository> logger, IDapperConnectionFactory connectionFactory)
        {
            _context = context;
            _logger = logger;
            this.connectionFactory = connectionFactory;
        }
        public async Task<Result<string>> AddExpensesAsync(string? customerRefId, List<CustomerExpense> req)
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
                    return Result.Failure<string>(ErrorResponse.NotFound("no customer is selected"));
                }
                customer.CustomerExpenses = req;
                await _context.SaveChangesAsync();

                return Result.Success("customer expense was added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }

        }

        public async Task<Result<string>> AddIncomesAsync(string? customerRefId, List<CustomerIncome> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var customer = await _context.Customers
                                            .Include(x => x.CustomerIncomes)
                                            .FirstOrDefaultAsync(x => x.CustomerId == customerRefId);
                if (customer is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("no customer is selected"));
                }

                customer.CustomerIncomes = req;

                await _context.SaveChangesAsync();

                return Result.Success("customer income was added.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }


        }

        public async Task<Result<string>> AddOrUpdateExpensesAsync(string? customerId, List<CustomerExpense> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var existingCustomer = await _context.Customers
                                            .Include(x => x.CustomerExpenses)
                                            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

                if (existingCustomer is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("no customer is selected"));
                }

                _context.Entry(existingCustomer).CurrentValues.SetValues(req);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }

        public async Task<Result<string>> AddOrUpdateIncomesAsync(string? customerId, List<CustomerIncome> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var existingCustomer = await _context.Customers
                                            .Include(x => x.CustomerIncomes)
                                            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
                if (existingCustomer is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("no customer is selected"));
                }

                _context.Entry(existingCustomer).CurrentValues.SetValues(req);

                foreach (var item in req)
                {
                    var existingIncomes = existingCustomer.CustomerIncomes?.Where(x => x.Id == item.Id)
                                                                 .FirstOrDefault();

                    if (existingIncomes is null || existingIncomes.Id == 0)
                    {
                        existingCustomer.CustomerIncomes?.Add(item);
                    }
                    else
                    {
                        _context.Entry(existingIncomes).CurrentValues.SetValues(item);
                    }
                }

                foreach (var item in existingCustomer.CustomerIncomes!)
                {
                    if (!req.Any(x => x.Id == item.Id))
                    {
                        _context.Remove(item);
                    }

                }

                await _context.SaveChangesAsync();

                return Result.Success("customer income was added.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }


        }

        public async Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(string? customerId)
        {

            using var db = connectionFactory.CreateConnection();

            var expense = await db.QueryAsync<VCustomerExpenses>($"SELECT * FROM VCustomerExpenses WHERE CustomerRefId=@customerId",new { customerId});

            if (expense is null)
            {
                return Result.Failure<IEnumerable<VCustomerExpenses>>(ErrorResponse.NotFound("No expense found"));
            }

            return Result.Success(expense);

        }


        public async Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(string? customerId)
        {

            using var db = connectionFactory.CreateConnection();

            var incomes = await db.QueryAsync<VCustomerIncomes>($"SELECT * FROM VCustomerIncomes WHERE CustomerRefId=@customerId", new { customerId });

            if (incomes is null)
            {
                return Result.Failure<IEnumerable<VCustomerIncomes>>(ErrorResponse.NotFound("No expense found"));
            }

            return Result.Success(incomes);
        }


        public async Task<Result<IEnumerable<T>>> GetCustomerCustomerIncomes<T>(string? customerId)
            where T : class
        {
            var incomes = await _context.CustomerIncomes
                                 .Where(x => x.CustomerRefId == customerId)
                                 .ProjectToType<T>()
                                 .AsNoTracking()
                                 .ToListAsync();

            if (incomes is null)
            {
                return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("No expense found"));
            }

            return Result.Success<IEnumerable<T>>(incomes);
        }

        public async Task<Result<IEnumerable<T>>> GetCustomerCustomerExpenses<T>(string? customerId)
                    where T : class
        {
            var expense = await _context.CustomerExpenses
                                 .Where(x => x.CustomerRefId == customerId)
                                 .ProjectToType<T>()
                                 .AsNoTracking()
                                 .ToListAsync();
            if (expense is null)
            {
                return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("No expense found"));
            }

            return Result.Success<IEnumerable<T>>(expense);

        }

    }
}
