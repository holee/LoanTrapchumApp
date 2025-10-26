using Applications.Commons.Messages.Incomes;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;
using Dapper;
using Domains.Models.Lists.Customers;
using ErrorOr;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public class IncomeExpendRepository : ICustomerIncomeExpendService
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;

        private readonly ILogger<IncomeExpendRepository> _logger;

        private readonly IDapperConnectionFactory _connectionFactory;
        public IncomeExpendRepository(IDbContextFactory<DataContext> context, ILogger<IncomeExpendRepository> logger, IDapperConnectionFactory connectionFactory)
        {
            _contextFactory = context;

            _logger = logger;

            _connectionFactory = connectionFactory;

        }

        public async Task<Result<IEnumerable<VCustomerExpenses>>> GetCustomerCustomerExpenses(string? customerId)
        {

            using var db = _connectionFactory.CreateConnection();

            var expense = await db.QueryAsync<VCustomerExpenses>($"SELECT * FROM VCustomerExpenses WHERE CustomerRefId=@customerId", new { customerId });

            if (expense is null)
            {
                return Applications.Responses.Result.Failure<IEnumerable<VCustomerExpenses>>(ErrorResponse.NotFound("No expense found"));
            }

            return Applications.Responses.Result.Success(expense);

        }



        public async Task<Result<string>> AddExpensesAsync(string? customerRefId, List<CustomerExpense> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var context = await _contextFactory.CreateDbContextAsync();

                var customer = await context
                              .Customers
                              .Include(x => x.CustomerExpenses)
                              .FirstOrDefaultAsync(x => x.CustomerId == customerRefId);

                if (customer is null)
                {
                    return Applications.Responses.Result.Failure<string>(ErrorResponse.NotFound("no customer is selected"));
                }
                customer.CustomerExpenses = req;

                await context.SaveChangesAsync();

                return Applications.Responses.Result.Success("customer expense was added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Applications.Responses.Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }

        }

        public async Task<Result<string>> AddIncomesAsync(string? customerRefId, List<CustomerIncome> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");


                var context = await _contextFactory.CreateDbContextAsync();

                var customer = await context.Customers
                                            .Include(x => x.CustomerIncomes)
                                            .FirstOrDefaultAsync(x => x.CustomerId == customerRefId);
                if (customer is null)
                {
                    return Applications.Responses.Result.Failure<string>(ErrorResponse.NotFound("no customer is selected"));
                }

                customer.CustomerIncomes = req;

                await context.SaveChangesAsync();

                return Applications.Responses.Result.Success("customer income was added.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Applications.Responses.Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }


        }

        public async Task<Result<string>> AddOrUpdateExpensesAsync(string? customerId, List<CustomerExpense> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");


                var context = await _contextFactory.CreateDbContextAsync();

                var existingCustomer = await context.Customers
                                            .Include(x => x.CustomerExpenses)
                                            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

                if (existingCustomer is null)
                {
                    return Applications.Responses.Result.Failure<string>(ErrorResponse.NotFound("no customer is selected"));
                }

                context.Entry(existingCustomer).CurrentValues.SetValues(req);

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
                        context.Entry(existingExpense).CurrentValues.SetValues(item);
                    }
                }

                foreach (var item in existingCustomer.CustomerExpenses!)
                {
                    if (!req.Any(x => x.Id == item.Id))
                    {
                        context.Remove(item);
                    }

                }
                await context.SaveChangesAsync();

                return Applications.Responses.Result.Success("customer expense was added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Applications.Responses.Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }
        }



        public async Task<ErrorOr<string>> AddOrUpdateIncomesAsync(string? customerId, List<CustomerIncome> req)
        {
            try
            {
                _logger.LogInformation("Adding income...");


                var context = await _contextFactory.CreateDbContextAsync();

                var existingCustomer = await context.Customers
                                            .Include(x => x.CustomerIncomes)
                                            .FirstOrDefaultAsync(x => x.CustomerId == customerId);
                if (existingCustomer is null)
                {
                    return Error.NotFound("no customer is selected");
                }

                context.Entry(existingCustomer).CurrentValues.SetValues(req);

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
                        context.Entry(existingIncomes).CurrentValues.SetValues(item);
                    }
                }

                foreach (var item in existingCustomer.CustomerIncomes!)
                {
                    if (!req.Any(x => x.Id == item.Id))
                    {
                        context.Remove(item);
                    }

                }

                await context.SaveChangesAsync();

                return Applications.Responses.Result.Success("customer income was added.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Applications.Responses.Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
            }


        }


        public async Task<ErrorOr<long>> AddOrUpdateIncomeAsync(CustomerIncome req)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var context = await _contextFactory.CreateDbContextAsync();

                var existingCustomer = await context.Customers
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.CustomerId == req.CustomerRefId);

                if (existingCustomer is null)
                {
                    return Error.NotFound("no customer is selected");
                }

                if (req.Id == 0)
                {
                    await context.CustomerIncomes.AddAsync(req);
                }
                else
                {
                    context.Entry(req).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();

                return req.Id;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Error.Unexpected($"{ex.InnerException!.Message}");
            }


        }



        public async Task<Result<VCustomerIncomes>> AddOrUpdateIncomeAsync(string? customerId, CustomerIncome req)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    return Applications.Responses.Result.Failure<VCustomerIncomes>(ErrorResponse.NotFound("Customer ID is required"));
                }

                var context = await _contextFactory.CreateDbContextAsync();

                _logger.LogInformation("Adding or updating income for customer {CustomerId}...", customerId);

                var existingCustomer = await context.Customers
                                    .Include(x => x.CustomerIncomes)
                                    .FirstOrDefaultAsync(x => x.CustomerId == customerId);

                if (existingCustomer is null)
                {
                    return Applications.Responses.Result.Failure<VCustomerIncomes>(ErrorResponse.NotFound("No customer is selected"));
                }

                if (req.Id != 0)
                {
                    var existingIncome = existingCustomer.CustomerIncomes?
                                    .FirstOrDefault(x => x.Id == req.Id);

                    if (existingIncome is not null)
                    {
                        context.Entry(existingIncome).CurrentValues.SetValues(req);
                    }
                    else
                    {
                        return Applications.Responses.Result.Failure<VCustomerIncomes>(ErrorResponse.NotFound("Income record not found"));
                    }
                }
                else
                {
                    existingCustomer.CustomerIncomes ??= new List<CustomerIncome>();

                    existingCustomer.CustomerIncomes.Add(req);
                }

                await context.SaveChangesAsync();


                var addedOrUpdatedIncome = await context.CustomerIncomes
                    .Select(ProjectTo())
                    .FirstOrDefaultAsync(x => x.IncomeId == req.Id);

                if (addedOrUpdatedIncome is not null)
                {
                    return Applications.Responses.Result.Success(addedOrUpdatedIncome);
                }

                return Applications.Responses.Result.Failure<VCustomerIncomes>(ErrorResponse.ServerError("Failed to retrieve the added or updated income record"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding or updating income for customer {CustomerId}", customerId);
                return Applications.Responses.Result.Failure<VCustomerIncomes>(ErrorResponse.ServerError(ex.InnerException?.Message ?? ex.Message));
            }

        }





        public async Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(string? customerId)
        {

            using var db = _connectionFactory.CreateConnection();

            var incomes = await db.QueryAsync<VCustomerIncomes>($"SELECT * FROM VCustomerIncomes WHERE CustomerRefId=@customerId", new { customerId });

            if (incomes is null)
            {
                return Applications.Responses.Result.Failure<IEnumerable<VCustomerIncomes>>(ErrorResponse.NotFound("No expense found"));
            }

            return Applications.Responses.Result.Success(incomes);
        }


        public async Task<ErrorOr<IEnumerable<VCustomerIncomes>>> GetIncomes(string? customerId)
        {

            using var db = _connectionFactory.CreateConnection();

            var incomes = await db.QueryAsync<VCustomerIncomes>($"SELECT * FROM VCustomerIncomes WHERE CustomerRefId=@customerId", new { customerId });

            if (!incomes.Any())
            {
                return Error.NotFound(IncomeMessage.NotFound);
            }

            return incomes.ToList();
        }
        public async Task<ErrorOr<VCustomerIncomes>> GetIncome(long? id, string? customerId)
        {

            using var db = _connectionFactory.CreateConnection();

            var income = await db.QueryFirstOrDefaultAsync<VCustomerIncomes>($"SELECT * FROM VCustomerIncomes WHERE CustomerRefId=@customerId AND IncomeId=@id", new { customerId, id });

            if (income is null)
            {
                return Error.NotFound(IncomeMessage.NotFound);
            }

            return income;
        }



        public async Task<ErrorOr<long>> AddOrUpdateIncomeAsync(CustomerIncome req)
        {
            try
            {
                _logger.LogInformation("Adding or updating income for customer {CustomerId}...", req.CustomerRefId);

                var context = await _contextFactory.CreateDbContextAsync();

                if (req.Id != 0)
                {
                    context.Entry(req).State = EntityState.Modified;
                }
                else
                {
                    context.CustomerIncomes.Add(req);
                }

                await context.SaveChangesAsync();

                
                return req.Id;  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding or updating income for customer {CustomerId}", req.CustomerRefId);
                
                return Error.Unexpected(ex.InnerException?.Message ?? ex.Message);
            }

        }




        public async Task<Result<IEnumerable<T>>> GetCustomerCustomerIncomes<T>(string? customerId)
            where T : class
        {

            var context = await _contextFactory.CreateDbContextAsync();

            var incomes = await context.CustomerIncomes
                                 .Where(x => x.CustomerRefId == customerId)
                                 .ProjectToType<T>()
                                 .AsNoTracking()
                                 .ToListAsync();

            if (incomes is null)
            {
                return Applications.Responses.Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("No expense found"));
            }

            return Applications.Responses.Result.Success<IEnumerable<T>>(incomes);
        }

        public async Task<Result<IEnumerable<T>>> GetCustomerCustomerExpenses<T>(string? customerId)
                    where T : class
        {
            var expense = await _contextFactory.CustomerExpenses
                                 .Where(x => x.CustomerRefId == customerId)
                                 .ProjectToType<T>()
                                 .AsNoTracking()
                                 .ToListAsync();
            if (expense is null)
            {
                return Applications.Responses.Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("No expense found"));
            }

            return Applications.Responses.Result.Success<IEnumerable<T>>(expense);

        }



        private static System.Linq.Expressions.Expression<Func<CustomerIncome, VCustomerIncomes>> ProjectTo()
        {
            return x => new VCustomerIncomes
            {
                IncomeId = x.Id,
                CustomerRefId = x.CustomerRefId,
                IncomeSource = x.IncomeSource,
                IncomeAmount = x.Amount,
                InDescription = x.Description
            };
        }

    }
}
