using System.Data;
using Applications.Constants.Tables;
using Applications.Interfaces.Customers;
using Applications.Requests.Lists.Customers;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Dapper;
using Domains.Models.Lists.Customers;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public class CustomerService(ApplicationDbContext context, IDapperConnectionFactory dapperService, ILogger<CustomerService> logger) : ICustomerService
{
    public async Task<Result<IEnumerable<T>>> GetCustomersAsync<T>(Guid? customerId, int take = 0,
        bool includeInactive = false,
        bool exceptActiveLoan = true,
        string? searchTerm = null) where T : class
    {
        IQueryable<VCustomerList> customerQueries = context.CustomerLists;

        if (includeInactive)
        {
            customerQueries = customerQueries.IgnoreQueryFilters();
        }

        if (exceptActiveLoan)
        {
            customerQueries = customerQueries.Where(x => x.IsLoanActive == false);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            customerQueries = customerQueries.Where(x => (EF.Functions.Like(x.FullName!, $"%{searchTerm}%") ||
                                                          EF.Functions.Like(x.AltName!, $"%{searchTerm}%") ||
                                                          EF.Functions.Like(
                                                              EF.Functions.Collate(x.CustomerId!.ToString(),
                                                                  "Khmer_100_CI_AI_SC_UTF8"), $"%{searchTerm}%") ||
                                                          EF.Functions.Like(x.Phone!, $"%{searchTerm}%")));
        }

        if (customerId.HasValue)
        {
            customerQueries = customerQueries.Where(x => x.CustomerId == customerId);
        }

        if (take > 0)
        {
            customerQueries = customerQueries.Take(take).OrderBy(x => x.CustomerId);
        }

        if (!customerQueries.Any())
        {
            return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("no customer found."));
        }

        var results = await customerQueries
            .ProjectToType<T>()
            .AsNoTracking()
            .ToListAsync();
        return Result.Success<IEnumerable<T>>(results);
    }

    public async Task<Pager<VCustomerList>> GetCustomers(QueryParam q)
    {
        IQueryable<VCustomerList> queries = context.CustomerLists;

        if (!q.IsIncludeInActive)
        {
            queries = queries.Where(x => x.IsActive == true);
        }

        if (!string.IsNullOrEmpty(q.Filter))
        {
            queries = queries.Where(x => (EF.Functions.Like(x.FullName!, $"%{q.Filter}%") ||
                                          EF.Functions.Like(
                                              EF.Functions.Collate(x.CustomerId.ToString(), "Khmer_100_CI_AI_SC_UTF8"),
                                              $"%{q.Filter}%") ||
                                          EF.Functions.Like(x.AltName!, $"%{q.Filter}%") ||
                                          EF.Functions.Like(x.Phone!, $"%{q.Filter}%")));
        }

        if (q.SortDescending)
        {
            queries = queries.OrderBy(x => EF.Property<object>(x, q.SortColumn));
        }
        else
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, q.SortColumn));
        }

        var count = queries.Count();
        var customers = queries.AsNoTracking().Skip(q.PageIndex).Take(q.PageSize);
        return new Pager<VCustomerList>
        {
            Items = await customers.ToListAsync(),
            Count = count,
        };
    }

    public async Task<Result<string>> AddCustomerNote(CustomerNote model)
    {
        try
        {
            logger.LogInformation("try adding new customer note");
            context.CustomerNote.Add(model);
            await context.SaveChangesAsync();
            return Result.Success(Messages.DataCreated);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return Result.Failure<string>(ErrorResponse.ServerError(e.Message));
        }
    }


    public async Task<Result<string>> AddCustomerActivity(CustomerActivity model) 
    {
        try
        {
            logger.LogInformation("try adding new customer note");
            context.CustomerActivites.Add(model);
            await context.SaveChangesAsync();
            return Result.Success(Messages.DataCreated);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return Result.Failure<string>(ErrorResponse.ServerError(e.Message));
        }
    }

    public async Task<Result<IEnumerable<T>>> GetCustomerNotes<T>(Guid? customerId)
        where T : class
    {
        try
        {
            logger.LogInformation("try adding new customer note");

            var notes = await context.Database.SqlQuery<T>($"SELECT * FROM VCustomerNotes WHERE CustomerRefId={customerId.ToString()}")
                .ProjectToType<T>()
                .AsNoTracking()
                .ToListAsync();

            if (notes.Any())
            {
                return Result.Success<IEnumerable<T>>(notes);
            }

            return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("There is no any notes."));
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return Result.Failure<IEnumerable<T>>(ErrorResponse.ServerError(e.Message));
        }
    }

    public async Task<Result<IEnumerable<T>>> GetCustomerActivies<T>(Guid? customerId) 
        where T : class
    {
        try
        {
            logger.LogInformation("try adding new customer note");

            var notes = await context.Database.SqlQuery<T>($"SELECT * FROM VCustomerActivities WHERE CustomerRefId={customerId.ToString()}")
                .ProjectToType<T>()
                .AsNoTracking()
                .ToListAsync();

            if (notes.Any())
            {
                return Result.Success<IEnumerable<T>>(notes);
            }

            return Result.Failure<IEnumerable<T>>(ErrorResponse.NotFound("Something went wrong with server."));
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e);
            return Result.Failure<IEnumerable<T>>(ErrorResponse.ServerError(e.Message));
        }
    }

    public async Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string customerId, bool include = false)
    {
        using var connection =  dapperService.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@customerId", customerId);
        parameters.Add("@include", include);

        if (!include)
        {
            // Simple case - just get customer details
            var customer = await connection.QuerySingleOrDefaultAsync<VCustomerList>(
                ProcedureConstant.GetCustomerDetailById, parameters, commandType: CommandType.StoredProcedure);
            if (customer is not null)
            {
                return Result.Success(customer);
            }

            return Result.Failure<VCustomerList>(ErrorResponse.NotFound("No customer found."));
        }

        // Complex case - handle one-to-many relationships

        var multiMap = await connection.QueryMultipleAsync(ProcedureConstant.GetCustomerDetailById, parameters,
            commandType: CommandType.StoredProcedure);

        var customerIncludes = await multiMap.ReadFirstOrDefaultAsync<VCustomerList>();

        if (customerIncludes is not null)
        {
            var loans = await multiMap.ReadAsync<VLoanDisbursments>();
            var expenses = await multiMap.ReadAsync<VCustomerExpenses>();
            var incomes = await multiMap.ReadAsync<VCustomerIncomes>();
            var notes = await multiMap.ReadAsync<VCustomerNotes>();
            var requests = await multiMap.ReadAsync<VLoanRequests>();
            var collaterallines = await multiMap.ReadAsync<VCollateralLines>();
            var activities = await multiMap.ReadAsync<VCustomerActivities>();
            customerIncludes.CustomerExpenses = expenses.ToArray() ?? [];
            customerIncludes.CustomerIncomes = incomes.ToArray() ?? [];
            customerIncludes.CustomerNotes = notes.ToArray() ?? [];
            customerIncludes.LoanRequests = requests.ToArray() ?? [];
            customerIncludes.CollateralLines = collaterallines.ToArray() ?? [];
            customerIncludes.CustomerActivities = activities.ToArray() ?? [];
            return Result.Success(customerIncludes);
        }

        return Result.Failure<VCustomerList>(ErrorResponse.NotFound("No customer found."));
    }

    public async Task<Result<EntityNames>> GetCustomerByIdAsync(Guid? customerId)
    {
        var customer = await context.EntityNames
            .Include(e => e.Customer)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == customerId);
        if (customer is null)
        {
            return Result.Failure<EntityNames>(ErrorResponse.NotFound($"No Customer Found"));
        }

        return Result.Success(customer);
    }

    public async Task<Result<Customer>> GetCustomerByIdAsync(Guid? customerId, params string[] includes)
    {
        IQueryable<Customer> customersQuery = context.Customers.Where(x => x.CustomerId == customerId);
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

        return Result.Success(customer!);
    }

    public async Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(Guid? customerId)
    {
        IQueryable<VCustomerList> customersQuery = context.CustomerLists
            .Where(x => x.CustomerId == customerId);
        if (!customersQuery.Any())
        {
            return Result.Failure<VCustomerList>(ErrorResponse.ServerError($"no customer found"));
        }

        var customer = await customersQuery
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return Result.Success(customer!);
    }

    public async Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(Guid? customerId, bool include = false)
    {
        try
        {
            using var db = await dapperService.CreateConnectionAsync();

            if (!include)
            {
                var customer = await db.QueryFirstOrDefaultAsync<VCustomerList>("spGetCustomerDetailsById",
                    new
                    {
                        customerId = customerId,
                        include = include
                    }, commandType: CommandType.StoredProcedure);

                if (customer is null)
                {
                    return Result.Failure<VCustomerList>(
                        ErrorResponse.NotFound("no data found."));
                }

                return Result.Success(customer);
            }

            var coustmerDictionary = new Dictionary<Guid, VCustomerList>();

            var customerInclude = await db.QueryAsync("spGetCustomerDetailsById",
                (VCustomerList customer, VLoanDisbursments loan, VLoanRequests loanRequest,
                    VCollateralLines collateralLine, VCustomerIncomes customerIncome, VCustomerExpenses customerExpense,
                    VCustomerNotes customerNote) =>
                {
                    if (coustmerDictionary.TryGetValue(customer.CustomerId!.Value, out var existingCustomer))
                    {
                        customer = existingCustomer;
                    }
                    else
                    {
                        coustmerDictionary.Add(customer.CustomerId!.Value, customer);
                    }

                    customer.Loans.Add(loan);
                    customer.LoanRequests.Add(loanRequest);
                    customer.CollateralLines.Add(collateralLine!);
                    customer.CustomerExpenses.Add(customerExpense);
                    customer.CustomerIncomes.Add(customerIncome);
                    customer.CustomerNotes.Add(customerNote);
                    return customer;
                },
                new
                {
                    customerId = customerId,
                    include = include
                },
                commandType: CommandType.StoredProcedure,
                splitOn: "Id,LoanRequestId,CustomerRefId,IncomeId,ExpendId,NoteId");


            if (!customerInclude.Any())
                return Result.Failure<VCustomerList>(ErrorResponse.NotFound("No note found."));

            var item = customerInclude.First();

            return Result.Success(item);
        }
        catch (SqlException ex)
        {
            return Result.Failure<VCustomerList>(
                ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }

    public async Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(Guid? customerId, bool includeInUsed = false)
    {
        var queries =
            context.Database.SqlQuery<VCollateralLines>($"SELECT * FROM VCustomerCollateralLines WHERE CollateralId={customerId.ToString()}");

        if (!includeInUsed)
        {
            queries = queries.Where(x => x.IsInUsed == false);
        }

        if (!queries.Any())
        {
            return Result.Failure<IEnumerable<VCollateralLines>>(ErrorResponse.NotFound("no data."));
        }

        var items = await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VCollateralLines>>(items);
    }

    public async Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(Guid? customerId)
    {
        using var db = dapperService.CreateConnection();

        var sql = @"SELECT * FROM VLoanDisbursments WHERE CustomerId=@customerId";

        var loanHistories = await db.QueryAsync<VLoanDisbursments>(sql, new { @customerId = customerId });

        return Result.Success(loanHistories);
    }

    public async Task<Result<string>> DeleteCustomerAsync(Guid? customerId)
    {
        try
        {
            var customer = await context.Customers
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);
            if (customer is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no Customer in the List."));
            }

            var customerInUse = context.LoanDisbursments.AsNoTracking().Where(x => x.CustomerRefId == customerId)
                .ToList();

            if (customerInUse.Any())
            {
                return Result.Failure<string>(
                    ErrorResponse.Conflict("This Customer information is in use, so cannot delete"));
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

    public async Task<Result<string>> MakeCustomerInActiveAsync(Guid? customerId, int editSeq = 0, bool isActive = true)
    {
        try
        {
            var increaseEdit = editSeq + 1;
            var updatedRows = await context.Customers
                .Where(x => x.CustomerId == customerId && x.EntityNames!.EditSeq == editSeq)
                .ExecuteUpdateAsync(prop => prop
                    .SetProperty(x => x.EntityNames!.IsActive, isActive)
                    .SetProperty(x => x.EntityNames!.EditSeq, increaseEdit));
            if (updatedRows > 0)
            {
                return Result.Success(new string("customer was updated"));
            }

            return Result.Failure<string>(ErrorResponse.ServerError($"cannot update customer."));
        }
        catch (SqlException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }

    public async Task<Result<string>> CreateAsync(EntityNames model)
    {
        try
        {
            context.EntityNames.Add(model);
            await context.SaveChangesAsync();
            return Result.Failure<string>(ErrorResponse.ServerError($"New Customer was added."));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }

    public async Task<Result<string>> UpdateAsync(EntityNames model)
    {
        try
        {
            context.EntityNames.Update(model);

            await context.SaveChangesAsync();

            return Result.Success<string>("Customer update customer.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
}


//var customerDictionary = new Dictionary<Guid, VCustomerList>();
//    await connection.QueryAsync<VCustomerList, VLoanDisbursments, VLoanRequests, VCollateralLines, VCustomerIncomes, VCustomerExpenses, VCustomerNotes, VCustomerList>
//        (ProcedureConstant.GetCustomerDetailsById,
//            (
//                 customer,
//                 loan,
//                 request,
//                 collateral,
//                 income,
//                 expense,
//                 note
//            ) =>
//                {
//        if (!customerDictionary.TryGetValue(customer.CustomerId!.Value, out VCustomerList customerEntry))
//        {
//            customerEntry = customer;
//            customerEntry.Loans = [];
//            customerEntry.LoanRequests = [];
//            customerEntry.CollateralLines = [];
//            customerEntry.CustomerIncomes = [];
//            customerEntry.CustomerExpenses = [];
//            customerEntry.CustomerNotes = [];
//            //customerEntry.Activities = [];
//            customerDictionary.Add(customer.CustomerId.Value, customerEntry);
//        }
//        if (loan != null) customerEntry.Loans.Add(loan);
//        if (request != null) customerEntry.LoanRequests.Add(request);
//        if (collateral != null) customerEntry.CollateralLines.Add(collateral);
//        if (income != null) customerEntry.CustomerIncomes.Add(income);
//        if (expense != null) customerEntry.CustomerExpenses.Add(expense);
//        if (note != null) customerEntry.CustomerNotes.Add(note);
//        //if (activity != null) customerEntry.CustomerActivities.Add(activity);
//        return customerEntry;
//    },
//                param: parameters,
//                commandType: CommandType.StoredProcedure,
//                splitOn: "LoanId,LoanRequestId,CollateralId,IncomeId,ExpendId,NoteId,ActivityId");

//        return customerDictionary.Values.FirstOrDefault();


//var customerIncludes = multiMap.Read<VCustomerList, VLoanDisbursments, VLoanRequests, VCollateralLines, VCustomerIncomes, VCustomerExpenses, VCustomerNotes, VCustomerActivities, VCustomerList>(
//            (customer, loan, request, collateral, income, expense, note, ac) =>
//            {
//                customer.Loans.Add(loan);
//                customer.LoanRequests.Add(request);
//                customer.CollateralLines.Add(collateral);
//                customer.CustomerIncomes.Add(income);
//                customer.CustomerExpenses.Add(expense);
//                customer.CustomerNotes.Add(note);
//                return customer;
//            }, splitOn: "LoanId,LoanRequestId,CollateralId,IncomeId,ExpendId,NoteId");
//public async Task<VCustomerList?> GetByIdAsync(Guid customerId)
//{
//    using var connection = await dapperService.CreateConnectionAsync();

//    const string sql = """
//        SELECT c.*, ce.*, ci.*, cn.*, cl.*
//        FROM Customers c
//        LEFT JOIN CustomerExpenses ce ON c.CustomerId = ce.CustomerRefId
//        LEFT JOIN CustomerIncomes ci ON c.CustomerId = ci.CustomerRefId
//        LEFT JOIN CustomerNotes cn ON c.CustomerId = cn.CustomerRefId
//        LEFT JOIN CollateralLines cl ON c.CustomerId = cl.CustomerId
//        WHERE c.CustomerId = @CustomerId AND c.IsActive = 1
//        """;

//    var customerDictionary = new Dictionary<Guid, VCustomerList>();
//    await connection.QueryAsync<VCustomerList, VCustomerExpenses, VCustomerIncomes,VCustomerNotes, VCollateralLines, VCustomerList>(
//        sql, (customer, expenses, incomes, notes, collaterals) =>
//        {
//            if (!customerDictionary.TryGetValue(customer.CustomerId!.Value, out var customerEntry))
//            {
//                customerEntry = customer;
//                customerEntry.CustomerExpenses = [];
//                customerEntry.CustomerIncomes = [];
//                customerEntry.CustomerNotes = [];
//                customerEntry.CollateralLines = [];
//                customerDictionary.Add(customer.CustomerId.Value, customerEntry);
//            }

//            if (expenses != null) customerEntry.CustomerExpenses.Add(expenses);
//            if (incomes != null) customerEntry.CustomerIncomes.Add(incomes);
//            if (notes != null) customerEntry.CustomerNotes.Add(notes);
//            if (collaterals != null) customerEntry.CollateralLines.Add(collaterals);
//            return customerEntry;
//        },
//        new { CustomerId = customerId }, 
//        splitOn: "ExpendId,IncomeId,NoteId,Id");

//    return customerDictionary.Values.FirstOrDefault();
//}
//public async Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(Guid? customerId)
//{
//    var db = await dapperService.CreateConnectionAsync();

//    var results = await db.QueryMultipleAsync("spGetCustomerDetails", new
//    {
//        @customerId = customerId
//    }, commandType: CommandType.StoredProcedure);

//    var customer = await results.ReadFirstOrDefaultAsync<VCustomerList>();

//    if (customer is null)
//    {
//        return Result.Failure<VCustomerList>(ErrorResponse.NotFound("No Customer found."));
//    }

//    var expenses = results.Read<VCustomerExpenses>().ToList();
//    var incomes = results.Read<VCustomerIncomes>().ToList();
//    var notes = results.Read<VCustomerNotes>().ToList();
//    var requests = results.Read<VLoanRequests>().ToList();
//    var collaterallines = results.Read<VCollateralLines>().ToList();
//    customer.CustomerExpenses = expenses ?? [];
//    customer.CustomerIncomes = incomes ?? [];
//    customer.LoanRequests = requests ?? [];
//    customer.CustomerNotes = notes ?? [];
//    customer.CollateralLines = collaterallines ?? [];
//    return Result.Success<VCustomerList>(customer);
//}
//public async Task<Result<string>> AddOrUpdateCustomerAsync(Customer customer)
//{
//    try
//    {
//        var existingCustomer =
//            await context.Customers.FirstOrDefaultAsync(x => x.CustomerId == customer.CustomerId);

//        if (existingCustomer is null && customer.CustomerId.IsNullOrEmpty())
//        {
//            context.Customers.Add(customer);
//        }
//        else
//        {
//            context.Entry(existingCustomer!).CurrentValues.SetValues(customer);
//        }

//        await context.SaveChangesAsync();

//        return Result.Success(new string("New Customer was crated"));
//    }
//    catch (Exception ex)
//    {
//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}

//public async Task<Result<string>> UpdateCustomerAsync(Customer customer)
//{
//    try
//    {
//        var existedCustomer = context.Customers
//            .AsNoTracking()
//            .Where(x => x.CustomerId == customer.CustomerId &&
//                        customer.EntityNames!.EditSeq == customer.EntityNames!.EditSeq)
//            .FirstOrDefault();

//        if (existedCustomer is null)
//        {
//            return Result.Failure<string>(ErrorResponse.NotFound("There is no any customer in the system."));
//        }

//        context.Customers.Update(customer);

//        await context.SaveChangesAsync();

//        return Result.Success<string>(new string("customer was updated"));
//    }
//    catch (Exception ex)
//    {
//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}