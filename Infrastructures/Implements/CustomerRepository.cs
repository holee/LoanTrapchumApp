using Applications.Extensions;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Dapper;
using Domains.Models.Lists.Customers;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace Infrastructures.Implements;

public sealed class CustomerRepository(
    IDbContextFactory<DataContext> contextFactory,
    IDapperConnectionFactory dapperFactory,
    ILogger<CustomerRepository> logger) : ICustomerService
{

    public async Task<Pager<VCustomerList>> GetCustomersAsync(string? customerId, QueryParam param, CancellationToken cancellation = default)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

        var queries = cxt.VCustomerLists.AsNoTracking().Where(x => x.IsLoanActive == false);


        if (!string.IsNullOrEmpty(param.Filter) && string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.Where(x => EF.Functions.Like(x.FullName ?? "", $"{param.Filter}%") ||
                                         EF.Functions.Like(x.AltName ?? "", $"{param.Filter}%") ||
                                         EF.Functions.Like(x.AltPhone ?? "", $"{param.Filter}%") ||
                                         EF.Functions.Like(x.FirstName ?? "", $"{param.Filter}%") ||
                                         EF.Functions.Like(x.LastName ?? "", $"{param.Filter}%") ||
                                         x.CustomerKey == param.Filter);
        }

        if (!string.IsNullOrEmpty(param.Filter) && !string.IsNullOrEmpty(param.FilterColumn))
        {
            var property = typeof(VCustomerList).GetProperty(param.FilterColumn);

            if (property != null)
            {
                queries = queries.Where(e =>
                    EF.Functions.Like(EF.Property<string>(e, param.FilterColumn), $"{param.Filter}%"));
            }
        }


        if (customerId.IsNotNullOrEmpty())
        {
            queries = queries.Where(x => x.CustomerId == customerId);
        }

        queries = ApplyOrdering(queries, param);

        var countTask =await queries.CountAsync(cancellation);

        var dataTask = await queries
                        .Skip(param.Skip)
                        .Take(param.PageSize)
                        .ToListAsync(cancellation);

        return new Pager<VCustomerList>(dataTask, countTask);

    }
   
    public async Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, bool include = false, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching customer details for ID {CustomerId} (Include Related: {Include})", customerId, include);

        using var db = await dapperFactory.CreateConnectionAsync(cancellationToken);

        var parameters = new
        {
            customerId = customerId,
            include = include
        };

        using var multi = await db.QueryMultipleAsync(ProcedureConstant.SpGetCustomerDetailsById, parameters, commandType: CommandType.StoredProcedure);

        var customer = await multi.ReadSingleOrDefaultAsync<VCustomerList>();

        if (customer == null)
        {
            return Result.Failure<VCustomerList>(ErrorResponse.NotFound("Customer not found."));
        }

        if (!include)
        {
            return Result.Success(customer);
        }

        customer.Loans = (await multi.ReadAsync<VLoanDisbursments>()).ToList();
        customer.LoanRequests = (await multi.ReadAsync<VLoanRequests>()).ToList();
        customer.CollateralLines = (await multi.ReadAsync<VCollateralLines>()).ToList();
        customer.CustomerIncomes = (await multi.ReadAsync<VCustomerIncomes>()).ToList();
        customer.CustomerExpenses = (await multi.ReadAsync<VCustomerExpenses>()).ToList();
        customer.CustomerNotes = (await multi.ReadAsync<VCustomerNotes>()).ToList();
        customer.CustomerActivities = (await multi.ReadAsync<VCustomerActivities>()).ToList();
        return Result.Success(customer);

    }
    public async Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, params IEnumerable<Expression<Func<VCustomerList, object>>> includes)
    {
        logger.LogInformation("Fetching customer details for ID {CustomerId}", customerId);

        await using var context = contextFactory.CreateDbContext();

        var baseQuery = context.VCustomerLists.AsQueryable();

        if (includes.Any())
        {
            foreach (var include in includes)
            {
                baseQuery = baseQuery.Include(include);
            }
        }

        var customer = await baseQuery
                            .AsSplitQuery()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        if (customer is null)
            return Result.Failure<VCustomerList>(ErrorResponse.NotFound("Customer Not Found."));

        return Result.Success(customer);

    }
    public async Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(string? customerId, bool onlyComplete = false, CancellationToken cancellation = default)
    {
        try
        {
            using var db = await dapperFactory.CreateConnectionAsync(cancellation);

            var sql = @"SELECT *
                            FROM VLoanRequests r 
                        WHERE r.CustomerId=@customerId AND 
                        ((@onlyComplete='TRUE' AND r.StatusId=7) OR 
                        @onlyComplete='FALSE')";
            var parameters = new DynamicParameters();
            parameters.Add("@customerId", customerId);
            parameters.Add("@onlyComplete", onlyComplete);
            var rsp = await db.QueryAsync<VLoanRequests>(sql, parameters);
            if (!rsp.Any())
            {
                return Result.Failure<IEnumerable<VLoanRequests>>(ErrorResponse.NotFound("No note found."));
            }
            return Result.Success(rsp);
        }
        catch (SqlException ex)
        {
            return Result.Failure<IEnumerable<VLoanRequests>>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
    public async Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanAsync(string? customerId, CancellationToken cancellation = default)
    {
        try
        {
            using var db = await dapperFactory.CreateConnectionAsync(cancellation);

            var sql = @"SELECT * FROM  VLoanDisbursments r WHERE r.CustomerId=@customerId";

            var parameters = new DynamicParameters();

            parameters.Add("@customerId", customerId);

            var rsp = await db.QueryAsync<VLoanDisbursments>(sql, parameters);

            if (!rsp.Any())
            {
                return Result.Failure<IEnumerable<VLoanDisbursments>>(ErrorResponse.NotFound("No note found."));
            }

            return Result.Success(rsp);
        }
        catch (SqlException ex)
        {
            return Result.Failure<IEnumerable<VLoanDisbursments>>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
    public async Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId)
    {

        await using var cxt = await contextFactory.CreateDbContextAsync();

        var customer = await cxt.EntityNames
            .Where(x => x.Id == customerId)
            .Include(e => e.Customer)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == customerId);
        if (customer is null)
        {
            return Result.Failure<EntityNames>(ErrorResponse.NotFound($"No Customer Found"));
        }

        return Result.Success(customer);
    }
    public async Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes)
    {

        await using var cxt = await contextFactory.CreateDbContextAsync();

        var customersQuery = cxt.Customers
                                     .AsNoTracking()
                                      .Where(x => x.CustomerId == customerId);
        if (includes.Any())
        {
            foreach (var includeProp in includes)
            {
                customersQuery = customersQuery.Include(includeProp);
            }
        }

        if (!customersQuery.Any())
        {
            return Result.Failure<Customer>(ErrorResponse.ServerError($"no customer found"));
        }

        var customer = await customersQuery
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

        if (customer is null)
        {
            return Result.Failure<Customer>(ErrorResponse.ServerError($"no customer found"));
        }

        return Result.Success(customer);
    }
    public async Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(string? customerId, CancellationToken cancellation = default)
    {

        await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

        var customer = await cxt.VCustomerLists.FirstOrDefaultAsync(x => x.CustomerId == customerId);

        if (customer is null)
        {
            return Result.Failure<VCustomerList>(ErrorResponse.ServerError($"no customer found"));
        }
        return Result.Success(customer);
    }
    public async Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? customerId, bool includeInUsed = false)
    {
        using var db = dapperFactory.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@customerId", customerId);

        parameters.Add("@includeInUsed", includeInUsed);

        var sql = $@"SELECT * FROM VCustomerCollateralLines 
                    WHERE CollateralId=@customerId AND 
                    ((@includeInUsed='FALSE' AND IsInUsed=CAST(0 AS BIT)) OR 
                      @includeInUsed=CAST(1 AS BIT));
                    ";

        var items = await db.QueryAsync<VCollateralLines>(sql, parameters);

        if (!items.Any())
        {
            return Result.Failure<IEnumerable<VCollateralLines>>(ErrorResponse.NotFound("No data."));
        }

        return Result.Success(items);
    }
    public async Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId)
    {
        using var db = dapperFactory.CreateConnection();

        var sql = @"SELECT * FROM VLoanDisbursments WHERE CustomerId=@customerId";

        var loanHistories = await db.QueryAsync<VLoanDisbursments>(sql, new { @customerId = customerId });

        return Result.Success(loanHistories);
    }
    public async Task<Result<string>> DeleteCustomerAsync(string? customerId, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var customer = await context.Customers.FindAsync(customerId, cancellation);

            if (customer is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no Customer in the List."));
            }

            if (await IsCustomerInUsed(customerId, cancellation))
            {
                var rsp = await MakeCustomerInActiveAsync(customerId, false, cancellation);

                if (rsp.IsSuccess)
                {
                    return Result.Success("This customer was disable,because of is being used by someone.");
                }
            }

            context.Customers.Remove(customer);

            await context.SaveChangesAsync();

            return Result.Success("Customer was removed");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
    public async Task<Result<string>> MakeCustomerInActiveAsync(string? customerId, bool state, CancellationToken cancellation = default)
    {
        try
        {

            await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

            var existingCustomer = await _context.EntityNames
                                                .Where(x => x.Id == customerId)
                                                .Include(x => x.Customer)
                                                .FirstOrDefaultAsync(cancellation);
            if (existingCustomer is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            existingCustomer.IsActive = state;

            await _context.SaveChangesAsync(cancellation);

            return Result.Success<string>("customer state was changed.");
        }
        catch (SqlException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
    public async Task<Result<string>> CreateAsync(EntityNames model, CancellationToken cancellation = default)
    {
        try
        {
            if (await IsCustomerRegister(null, model.Phone, model.IdCard, cancellation))
            {
                return Result.Failure<string>(ErrorResponse.Conflict($"Customer With Name:{model.AltName} is already register."));
            }

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var customerSequence = await GetMaxSequenceNUmber(context);

            model.Customer!.CustomerInfId = customerSequence;

            model.Customer.CustomerKey = CreateCustomerKey(customerSequence, model.FirstName, model.LastName);

            context.EntityNames.Add(model);

            await context.SaveChangesAsync(cancellation);

            return Result.Success<string>($"New Customer was added.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
    public async Task<Result<string>> UpdateAsync(EntityNames model, CancellationToken cancellation = default)
    {
        try
        {

            if (await IsCustomerRegister(model.Id, model.Phone, model.IdCard, cancellation))
            {
                return Result.Failure<string>(ErrorResponse.Conflict($"Customer With Name:{model.AltName} is already register."));
            }

            await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

            cxt.EntityNames.Update(model);

            await cxt.SaveChangesAsync(cancellation);

            return Result.Success<string>("Customer update customer.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
    private async Task<bool> IsCustomerInUsed(string? customerId, CancellationToken cancellation = default)
    {
        await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

        var customerInUse = await _context.LoanDisbursments
                                   .AsNoTracking()
                                   .AnyAsync(x => x.CustomerRefId == customerId, cancellation);
        return customerInUse;
    }
    private async Task<bool> IsCustomerRegister(string? customerId, string? phoneNumber, string? idCard, CancellationToken cancellation = default)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required", nameof(phoneNumber));

        if (string.IsNullOrWhiteSpace(idCard))
            throw new ArgumentException("ID Card is required", nameof(idCard));

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        return await context.VCustomerLists
                            .AsNoTracking()
                            .AnyAsync(x => (x.Phone == phoneNumber || x.IdCard == idCard) &&
                                        (string.IsNullOrEmpty(x.CustomerId) || x.CustomerId != customerId),
                                cancellation);

    }
    private async Task<long> GetMaxSequenceNUmber(DataContext context)
    {
        var maxSequence = await context.EntityNames
                      .Include(x => x.Customer)
                      .AsNoTracking()
                      .Select(x => x.Customer!.CustomerInfId)
                      .MaxAsync();
        return maxSequence > 0 ? maxSequence + 1 : 1;
    }
    private string? CreateCustomerKey(long? customerSequence, string? firstName, string? lastName)
    {
        var customerKey = "";

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            var key = firstName.Substring(0, 1);

            customerKey = $"{customerSequence}{key}".ToUpper();
        }
        else
        {
            var key = lastName?.Substring(0, 1);

            customerKey = $"{customerSequence}{key}".ToUpper();
        }

        return customerKey;
    }

    private IQueryable<VCustomerList> ApplyOrdering(IQueryable<VCustomerList> query, QueryParam param)
    {
        if (!string.IsNullOrWhiteSpace(param.SortColumn))
        {
            var isDescending = param.SortDescending;

            var property = typeof(VCustomerList).GetProperty(param.SortColumn);

            if (property != null)
            {
                query = isDescending
                    ? query.OrderByDescending(e => EF.Property<object>(e, param.SortColumn))
                    : query.OrderBy(e => EF.Property<object>(e, param.SortColumn));
            }
        }
        else
        {
            query = query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
        }

        return query;
    }


}



//public async Task<Pager<VCustomerList>> GetCustomersAsync(QueryParam q, CancellationToken cancellation = default)
//{

//    await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

//    IQueryable<VCustomerList> queries = cxt.VCustomerLists.OrderByDescending(x => x.ModifiedDT);

//    var counts = queries.Count();

//    if (!q.IsIncludeInActive)
//    {
//        queries = queries.Where(x => x.IsActive == true);
//    }

//    if (!string.IsNullOrEmpty(q.Filter))
//    {
//        queries = queries.Where(x => (EF.Functions.Like(x.FullName!, $"%{q.Filter}%") ||
//                                      EF.Functions.Like(EF.Functions.Collate(x.CustomerId, "Khmer_100_CI_AI_SC_UTF8"),
//                                          $"%{q.Filter}%") ||
//                                      EF.Functions.Like(x.AltName!, $"%{q.Filter}%") ||
//                                      EF.Functions.Like(x.Phone!, $"%{q.Filter}%")));
//    }

//    if (q.SortDescending)
//    {
//        queries = queries.OrderByDescending(x => EF.Property<object>(x, q.SortColumn));
//    }

//    var count = queries.Count();

//    var customers = queries.AsNoTracking().Skip(q.Skip).Take(q.PageSize);

//    return new Pager<VCustomerList>
//    {
//        Items = await customers.ToListAsync(),
//        Count = count,
//    };
//}

//public async Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, bool include = false)
//{
//    try
//    {
//        using var db = await dapperFactory.CreateConnectionAsync();

//        if (!include)
//        {
//            var customer = await db.QueryFirstOrDefaultAsync<VCustomerList>("spGetCustomerDetailsById",
//                new
//                {
//                    customerId = customerId,
//                    include = include
//                }, commandType: CommandType.StoredProcedure);

//            if (customer is null)
//            {
//                return Result.Failure<VCustomerList>(
//                    ErrorResponse.NotFound("no data found."));
//            }

//            return Result.Success(customer);
//        }

//        var coustmerDictionary = new Dictionary<string, VCustomerList>();
//        //"spGetCustomerDetailsById"
//        var customerInclude = await db.QueryAsync(ProcedureConstant.SpGetCustomerDetailsById,
//            (VCustomerList customer, VLoanDisbursments loan, VLoanRequests loanRequest,
//                VCollateralLines collateralLine, VCustomerIncomes customerIncome, VCustomerExpenses customerExpense,
//                VCustomerNotes customerNote) =>
//            {
//                if (coustmerDictionary.TryGetValue(customer.CustomerId!, out var existingCustomer))
//                {
//                    customer = existingCustomer;
//                }
//                else
//                {
//                    coustmerDictionary.Add(customer.CustomerId!, customer);
//                }

//                customer.Loans.Add(loan);
//                customer.LoanRequests.Add(loanRequest);
//                customer.CollateralLines.Add(collateralLine!);
//                customer.CustomerExpenses.Add(customerExpense);
//                customer.CustomerIncomes.Add(customerIncome);
//                customer.CustomerNotes.Add(customerNote);
//                return customer;
//            },
//            new
//            {
//                customerId = customerId,
//                include = include
//            },
//            commandType: CommandType.StoredProcedure,
//            splitOn: "Id,LoanRequestId,CustomerRefId,IncomeId,ExpendId,NoteId");


//        if (!customerInclude.Any())
//            return Result.Failure<VCustomerList>(ErrorResponse.NotFound("No note found."));

//        var item = customerInclude.First();

//        return Result.Success(item);
//    }
//    catch (SqlException ex)
//    {
//        return Result.Failure<VCustomerList>(
//            ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}



//public async Task<Result<IEnumerable<T>>> GetCustomersAsync<T>(string? customerId, int take = 0,
//    bool includeInactive = false,
//    bool exceptActiveLoan = true,
//    string? searchTerm = null) where T : class
//{
//    await using var context =await contextFactory.CreateDbContextAsync();
//    IQueryable<VCustomerList> customerQueries = context.VCustomerLists;

//    if (includeInactive)
//    {
//        customerQueries = customerQueries.IgnoreQueryFilters();
//    }

//    if (exceptActiveLoan)
//    {
//        customerQueries = customerQueries.Where(x => x.IsLoanActive == false);
//    }

//    if (!string.IsNullOrEmpty(searchTerm))
//    {
//        customerQueries = customerQueries.Where(x => (EF.Functions.Like(x.FullName!, $"%{searchTerm}%") ||
//                                                      EF.Functions.Like(x.AltName!, $"%{searchTerm}%") ||
//                                                      EF.Functions.Like(
//                                                          EF.Functions.Collate(x.CustomerId!.ToString(),
//                                                              "Khmer_100_CI_AI_SC_UTF8"), $"%{searchTerm}%") ||
//                                                      EF.Functions.Like(x.Phone!, $"%{searchTerm}%")));
//    }

//    if (customerId.IsNotNullOrEmpty())
//    {
//        customerQueries = customerQueries.Where(x => x.CustomerId == customerId);
//    }

//    if (take > 0)
//    {
//        customerQueries = customerQueries.Take(take).OrderBy(x => x.CustomerId);
//    }

//    if (!customerQueries.Any())
//    {
//        return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("no customer found."));
//    }

//    var results = await customerQueries
//        .ProjectToType<T>()
//        .AsNoTracking()
//        .ToListAsync();
//    return Result.Success<IEnumerable<T>>(results);
//}
//public async Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, CancellationToken cancellationToken = default)
//{
//    logger.LogInformation("Fetching customer details for ID {CustomerId}", customerId);

//    await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

//    var customer = await context.VCustomerLists
//                        .Include(x => x.CustomerActivities)
//                        .Include(x => x.CustomerExpenses)
//                        .Include(x => x.CustomerIncomes)
//                        .Include(x => x.CustomerNotes)
//                        .Include(x => x.CollateralLines)
//                        .Include(x => x.LoanRequests)
//                        .Include(x => x.Loans)
//                        .AsSplitQuery()
//                        .AsNoTracking()
//                        .FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);

//    if (customer is null)
//        return Result.Failure<VCustomerList>(ErrorResponse.NotFound("Customer Not Found."));

//    return Result.Success(customer);

//}
