using Applications.Commons.Mappers.Expenses;
using Applications.Commons.Mappers.Incomes;
using Applications.Commons.Messages.Customers;
using Applications.Commons.Messages.Expenses;
using Applications.Commons.Messages.Incomes;
using Applications.Extensions;
using Applications.Features.Expenses.Dto;
using Applications.Features.Incomes.Dto;
using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Dapper;
using Domains.Enumerations;
using Domains.Models.Lists.Customers;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructures.Implements
{
    public class IncomeExpendRepository : ICustomerIncomeExpendService
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;

        private readonly ILogger<IncomeExpendRepository> _logger;

        private readonly IDapperConnectionFactory _connectionFactory;

        public IncomeExpendRepository(IDbContextFactory<DataContext> context,
            ILogger<IncomeExpendRepository> logger,
            IDapperConnectionFactory connectionFactory)
        {
            _contextFactory = context;

            _logger = logger;

            _connectionFactory = connectionFactory;
        }

        public async Task<ErrorOr<IEnumerable<VCustomerExpenses>>> GetExpenses(string? customerId)
        {
            using var db = _connectionFactory.CreateConnection();

            var expenses =
                await db.QueryAsync<VCustomerExpenses>(@"SELECT * FROM VCustomerExpenses 
                            WHERE CustomerRefId=@customerId", new { customerId });

            if (expenses is null)
            {
                return Error.NotFound(ExpenseMessage.NotFound);
            }

            return ErrorOrFactory.From(expenses);
        }

        public async Task<ErrorOr<VCustomerExpenses>> GetExpense(long? id, string? customerId, CancellationToken cancellation = default)
        {
            using var db = await _connectionFactory.CreateConnectionAsync(cancellation);

            var expense = await db.QueryFirstOrDefaultAsync<VCustomerExpenses>(
                @"SELECT TOP(1) * FROM 
                        VCustomerExpenses 
                    WHERE CustomerRefId=@customerId AND ExpendId=@Id", new { customerId, id });

            if (expense is null)
            {
                return Error.NotFound(ExpenseMessage.NotFound);
            }

            return expense;
        }

        public async Task<ErrorOr<string>> DeleteExpensesAsync(long? id, string? customerRefId, CancellationToken cancellation = default)
        {
            _logger.LogInformation("Adding income...");

            var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var existing = await context.CustomerExpenses
                .FirstOrDefaultAsync(x => x.Id == id && x.CustomerRefId == customerRefId);

            if (existing is null)
            {
                Error.NotFound(ExpenseMessage.NotFound);
            }

            try
            {
                context.CustomerExpenses.Remove(existing!);

                await context.SaveChangesAsync(cancellation);

                return ExpenseMessage.Deleted;
            }
            catch (DbUpdateException ex)
            {
                return Error.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Error.Unexpected(ex.Message);
            }
        }

        public async Task<ErrorOr<ExpenseDtoUpdate>> GetEditExpensesAsync(long? id, string? customerId, CancellationToken cancellation = default)
        {
            try
            {
                _logger.LogInformation("Adding income...");

                var context = await _contextFactory.CreateDbContextAsync();

                var expense = await context.CustomerExpenses
                    .FirstOrDefaultAsync(x => x.Id == id && x.CustomerRefId == customerId);

                if (expense is null)
                {
                    return Error.NotFound(ExpenseMessage.NotFound);
                }


                var editExpense = expense.ToEditExpense();

                return editExpense;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Error.Unexpected(ex.Message);
            }
        }

        public async Task<ErrorOr<string>> AddOrUpdateExpensesAsync(string? customerId, List<CustomerExpense> req)
        {
            try
            {
                _logger.LogInformation("Adding or updating Expense for customer {CustomerId}...", customerId);

                var context = await _contextFactory.CreateDbContextAsync();

                var existingCustomer = await context.Customers
                    .Include(x => x.CustomerExpenses)
                    .FirstOrDefaultAsync(x => x.CustomerId == customerId);

                if (existingCustomer is null)
                {
                    return Error.NotFound(ExpenseMessage.NotFound);
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

                return ExpenseMessage.Created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Error.Unexpected(ex.Message);
            }
        }

        public async Task<ErrorOr<long>> AddOrUpdateExpenseAsync(CustomerExpense req, CancellationToken cancellation = default)
        {
            _logger.LogInformation("Adding or updating income for customer {CustomerId}...", req.CustomerRefId);

            var context = await _contextFactory.CreateDbContextAsync(cancellation);
            try
            {

                var incomeSource = req.ExpenseTitle?.Trim();

                if (!string.IsNullOrWhiteSpace(incomeSource))
                {
                    var exists = await context.IncomeExpenses
                        .AnyAsync(x => x.Description.ToLower() == incomeSource.ToLower());

                    if (!exists)
                    {
                        context.IncomeExpenses.Add(new IncomeExpense
                        {
                            Description = incomeSource,
                            Types = IncomeExpenseTypes.Expense
                        });
                    }
                }

                if (req.Id > 0)
                {
                    context.CustomerExpenses.Update(req);
                }
                else
                {
                    context.CustomerExpenses.Add(req);
                }

                await context.SaveChangesAsync(cancellation);

                return req.Id;
            }
            catch (DbUpdateException ex)
            {
                return Error.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Error.Unexpected(ex.Message);
            }
        }

        public async Task<ErrorOr<string>> AddOrUpdateIncomesAsync(string? customerId, List<CustomerIncome> req)
        {
            try
            {
                _logger.LogInformation("Adding or updating income for customer {CustomerId}...", customerId);

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

                return IncomeMessage.Created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Error.Failure(ex.Message);
            }
        }

        public async Task<ErrorOr<bool>> DeleteIncomeAsync(long? id, string? customerId, CancellationToken cancellationToken = default)
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var existing = await context.CustomerIncomes
                .FirstOrDefaultAsync(x => x.Id == id && x.CustomerRefId == customerId, cancellationToken);

            if (existing is null)
            {
                return Error.NotFound(CustomerMessage.NotSelected);
            }

            try
            {
                context.CustomerIncomes.Remove(existing);

                await context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Error.Failure("Income.DeleteFailed", $"Unable to delete income: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Error.Unexpected("Income.DeleteError", $"Unexpected error while deleting income: {ex.Message}");
            }
        }


        public async Task<ErrorOr<IEnumerable<VCustomerIncomes>>> GetIncomesAsync(string? customerId)
        {
            using var db = _connectionFactory.CreateConnection();

            var incomes =
                await db.QueryAsync<VCustomerIncomes>($"SELECT * FROM VCustomerIncomes WHERE CustomerRefId=@customerId",
                    new { customerId });

            if (!incomes.Any())
            {
                return Error.NotFound(IncomeMessage.NotFound);
            }

            return incomes.ToList();
        }


        public async Task<ErrorOr<VCustomerIncomes>> GetIncomeByIdAsync(long? id, string? customerId)
        {
            using var db = _connectionFactory.CreateConnection();

            var income = await db.QueryFirstOrDefaultAsync<VCustomerIncomes>(
                $"SELECT * FROM VCustomerIncomes WHERE CustomerRefId=@customerId AND IncomeId=@id",
                new { customerId, id });

            if (income is null)
            {
                return Error.NotFound(IncomeMessage.NotFound);
            }

            return income;
        }


        public async Task<ErrorOr<EditIncomeDto>> GetEditIncomeAsync(long? id, string? customerId,
            CancellationToken cancellation = default)
        {
            _logger.LogInformation("Adding income...");

            var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var income = await context.CustomerIncomes
                .FirstOrDefaultAsync(x => x.Id == id && x.CustomerRefId == customerId);

            if (income is null)
            {
                return Error.NotFound(IncomeMessage.NotFound);
            }

            return income.ToIncomeEdit();
        }

        public async Task<ErrorOr<long>> AddOrUpdateIncomeAsync(CustomerIncome req, CancellationToken cancellation = default)
        {
            try
            {
                _logger.LogInformation("Adding or updating income for customer {CustomerId}...", req.CustomerRefId);

                var context = await _contextFactory.CreateDbContextAsync(cancellation);

                var incomeSource = req.IncomeSource?.Trim();


                // ✅ normalize early
                if (!string.IsNullOrWhiteSpace(incomeSource))
                {
                    var exists = await context.IncomeExpenses
                        .AnyAsync(x => x.Description.ToLower() == incomeSource.ToLower());

                    if (!exists)
                    {
                        context.IncomeExpenses.Add(new IncomeExpense
                        {
                            Description = incomeSource,
                            Types = IncomeExpenseTypes.Income
                        });
                    }
                }

                if (req.Id != 0)
                {
                    context.Entry(req).State = EntityState.Modified;
                }
                else
                {
                    context.CustomerIncomes.Add(req);
                }

                await context.SaveChangesAsync(cancellation);

                return req.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding or updating income for customer {CustomerId}", req.CustomerRefId);

                return Error.Unexpected(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<Pager<IncomeExpenseDto>> GetIncomeExpenseAsPagingAsync(int? id, IncomeExpenseTypes type,
            QueryParam query, CancellationToken cancellation = default)
        {
            _logger.LogInformation("Try Getting income...");

            var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var source = context.IncomeExpenses
                .WhereIf(id.HasValue && id > 0, x => x.Id == id);

            source = type switch
            {
                IncomeExpenseTypes.Expense => source.Where(x => x.Types == IncomeExpenseTypes.Expense),
                IncomeExpenseTypes.Income => source.Where(x => x.Types == IncomeExpenseTypes.Income),
                _ => source
            };

            source = ApplyFilter(source, query);

            source = source.ApplyOrdering(query, x => x.Types);

            var counts = await source.CountAsync(cancellation);

            var items = await source
                .Skip(query.Page)
                .Take(query.PageSize)
                .Select(ProjectToIncomeExpense())
                .ToListAsync(cancellation);

            return new Pager<IncomeExpenseDto>
            {
                Count = counts,
                Items = items
            };
        }

        public async Task<ErrorOr<int>> AddOrUpdateIncomeExpenseAsync(IncomeExpense req,
            CancellationToken cancellation = default)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

                if (req.Id == 0)
                {
                    context.IncomeExpenses.Add(req);
                }
                else
                {
                    context.IncomeExpenses.Update(req);
                }

                await context.SaveChangesAsync();

                return req.Id;
            }
            catch (DbUpdateException ex)
            {
                return Error.Unexpected(ex.Message);
            }
        }

        public async Task<ErrorOr<IncomeExpenseDto>> GetEditIncomeExpenseAsync(int? id,
            CancellationToken cancellation = default)
        {
            _logger.LogInformation("Try Getting income...");

            var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var source = await context.IncomeExpenses
                .Select(x => new IncomeExpenseDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    Types = x.Types.ToString()
                })
                .FirstOrDefaultAsync(x => x.Id == id, cancellation);

            if (source is not null)
            {
                return source;
            }

            return Error.NotFound(IncomeExpenseMessages.NotFound);
        }

        public async Task<ErrorOr<IncomeExpense>> GetIncomeExpenseAsync(int? id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Try Getting income...");

            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var source = await context.IncomeExpenses.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (source is not null)
            {
                return source;
            }

            return Error.NotFound(IncomeExpenseMessages.NotFound);
        }

        private static Expression<Func<IncomeExpense, IncomeExpenseDto>> ProjectToIncomeExpense()
        {
            return x => new IncomeExpenseDto
            {
                Id = x.Id,
                Description = x.Description,
                Types = x.Types.ToString()
            };
        }

        private IQueryable<IncomeExpense> ApplyFilter(IQueryable<IncomeExpense> source, QueryParam query)
        {
            if (string.IsNullOrWhiteSpace(query.Filter)) return source;

            string pattern = $"%{query.Filter}%";

            if (!string.IsNullOrWhiteSpace(query.FilterColumn))
            {
                string[] filterColums = [nameof(IncomeExpense.Description), nameof(IncomeExpense.Types)];

                source = source.WhereMatchesFilter(pattern, filterColums);
            }


            source = source.Where(x => EF.Functions.Like(x.Description, pattern) ||
                                       EF.Functions.Like(x.Types.ToString(), pattern));

            return source;
        }

        private static Expression<Func<CustomerIncome, VCustomerIncomes>> ProjectTo() => x => new VCustomerIncomes
        {
            IncomeId = x.Id,
            CustomerRefId = x.CustomerRefId,
            IncomeSource = x.IncomeSource,
            IncomeAmount = x.Amount,
            InDescription = x.Description
        };

        public async Task<ErrorOr<string>> UpdateIncomeExpenseAsync(int? id, IncomeExpense incomeExpense, CancellationToken cancellationToken = default)
        {
            if (id == null || incomeExpense == null)
            {
                return Error.NotFound(IncomeExpenseMessages.Required);
            }

            try
            {
                var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
                var existing = await context.IncomeExpenses.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (existing == null)
                {
                    return Error.NotFound(IncomeExpenseMessages.NotFound);
                }

                existing.Description = incomeExpense.Description;
                existing.Types = incomeExpense.Types;

                context.IncomeExpenses.Update(existing);
                await context.SaveChangesAsync(cancellationToken);
                return IncomeExpenseMessages.Updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating IncomeExpense with id {Id}", id);
                return Error.Unexpected($"{IncomeExpenseMessages.Unexpected} {ex.Message}");
            }
        }

        public async Task<ErrorOr<string>> DeleteIncomeExpenseAsync(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return Error.NotFound(IncomeExpenseMessages.Required);
            }

            try
            {
                var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
                var existing = await context.IncomeExpenses.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (existing == null)
                {
                    return Error.NotFound(IncomeExpenseMessages.NotFound);
                }

                context.IncomeExpenses.Remove(existing);
                await context.SaveChangesAsync(cancellationToken);
                return IncomeExpenseMessages.Deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting IncomeExpense with id {Id}", id);
                return Error.Unexpected($"{IncomeExpenseMessages.Unexpected} {ex.Message}");
            }
        }
    }
}


//public async Task<Result<string>> AddIncomesAsync(string? customerRefId, List<CustomerIncome> req)
//{
//    try
//    {
//        _logger.LogInformation("Adding income...");


//        var context = await _contextFactory.CreateDbContextAsync();

//        var customer = await context.Customers
//            .Include(x => x.CustomerIncomes)
//            .FirstOrDefaultAsync(x => x.CustomerId == customerRefId);
//        if (customer is null)
//        {
//            return Applications.Responses.Result.Failure<string>(
//                ErrorResponse.NotFound("no customer is selected"));
//        }

//        customer.CustomerIncomes = req;

//        await context.SaveChangesAsync();

//        return Applications.Responses.Result.Success("customer income was added.");
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, ex.Message);
//        return Applications.Responses.Result.Failure<string>(
//            ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}


//public async Task<ErrorOr<VCustomerIncomes>> AddOrUpdateIncomeAsync(string? customerId, CustomerIncome req)
//{
//    try
//    {
//        if (string.IsNullOrWhiteSpace(customerId))
//        {
//            return Error.NotFound("Customer ID is required");
//        }

//        var context = await _contextFactory.CreateDbContextAsync();

//        _logger.LogInformation("Adding or updating income for customer {CustomerId}...", customerId);

//        var existingCustomer = await context.Customers
//            .Include(x => x.CustomerIncomes)
//            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

//        if (existingCustomer is null)
//        {
//            return Error.NotFound("No customer is selected");
//        }

//        if (req.Id != 0)
//        {
//            var existingIncome = existingCustomer.CustomerIncomes?
//                .FirstOrDefault(x => x.Id == req.Id);

//            if (existingIncome is not null)
//            {
//                context.Entry(existingIncome).CurrentValues.SetValues(req);
//            }
//            else
//            {
//                return Error.NotFound(IncomeMessage.NotFound);
//            }
//        }
//        else
//        {
//            existingCustomer.CustomerIncomes ??= new List<CustomerIncome>();

//            existingCustomer.CustomerIncomes.Add(req);
//        }

//        await context.SaveChangesAsync();


//        var addedOrUpdatedIncome = await context.CustomerIncomes
//            .Select(ProjectTo())
//            .FirstOrDefaultAsync(x => x.IncomeId == req.Id);

//        if (addedOrUpdatedIncome is not null)
//        {
//            return addedOrUpdatedIncome;
//        }

//        return Error.Failure("Failed to retrieve the added or updated income record");
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, "Error adding or updating income for customer {CustomerId}", customerId);

//        return Error.Unexpected(ex.InnerException?.Message ?? ex.Message);
//    }
//}


//public async Task<ErrorOr<long>> AddOrUpdateAsync(CustomerIncome req)
//{
//    try
//    {
//        _logger.LogInformation("Adding income...");

///
//        var context = await _contextFactory.CreateDbContextAsync();

//        if (req.Id > 0)
//        {
//            context.CustomerIncomes.Update(req);
//        }
//        else
//        {
//            context.CustomerIncomes.Update(req);
//        }
//        await context.SaveChangesAsync();

//        return req.Id;
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, ex.Message);

//        return Error.Unexpected(ex.Message);
//    }
//}


//public async Task<Result<IEnumerable<VCustomerIncomes>>> GetCustomerCustomerIncomes(string? customerId)
//{
//    using var db = _connectionFactory.CreateConnection();

//    var incomes =
//        await db.QueryAsync<VCustomerIncomes>($"SELECT * FROM VCustomerIncomes WHERE CustomerRefId=@customerId",
//            new { customerId });

//    if (incomes is null)
//    {
//        return Applications.Responses.Result.Failure<IEnumerable<VCustomerIncomes>>(
//            ErrorResponse.NotFound("No expense found"));
//    }

//    return Applications.Responses.Result.Success(incomes);
//}


//public async Task<Result<string>> AddExpensesAsync(string? customerRefId, List<CustomerExpense> req)
//{
//    try
//    {
//        _logger.LogInformation("Adding income...");

//        var context = await _contextFactory.CreateDbContextAsync();

//        var customer = await context
//            .Customers
//            .Include(x => x.CustomerExpenses)
//            .FirstOrDefaultAsync(x => x.CustomerId == customerRefId);

//        if (customer is null)
//        {
//            return Applications.Responses.Result.Failure<string>(
//                ErrorResponse.NotFound("no customer is selected"));
//        }

//        customer.CustomerExpenses = req;

//        await context.SaveChangesAsync();

//        return Applications.Responses.Result.Success("customer expense was added.");
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, ex.Message);
//        return Applications.Responses.Result.Failure<string>(
//            ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}