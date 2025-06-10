using Applications.Enumerations;
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

namespace Infrastructures.Implements;

public class CustomerService(
    DataContext context,
    IDbContextFactory<DataContext> contextFactory,
    IDapperConnectionFactory dapperFactory,
    ILogger<CustomerService> logger) : ICustomerService
{
    public async Task<Result<IEnumerable<T>>> GetCustomersAsync<T>(string? customerId, int take = 0,
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

        if (customerId.IsNotNullOrEmpty())
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

    public async Task<Pager<VCustomerList>> GetCustomersAsync(QueryParam q, CancellationToken cancellation = default)
    {

        await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

        IQueryable<VCustomerList> queries = cxt.CustomerLists;

        var counts = queries.Count();

        if (!q.IsIncludeInActive)
        {
            queries = queries.Where(x => x.IsActive == true);
        }

        if (!string.IsNullOrEmpty(q.Filter))
        {
            queries = queries.Where(x => (EF.Functions.Like(x.FullName!, $"%{q.Filter}%") ||
                                          EF.Functions.Like(EF.Functions.Collate(x.CustomerId, "Khmer_100_CI_AI_SC_UTF8"),
                                              $"%{q.Filter}%") ||
                                          EF.Functions.Like(x.AltName!, $"%{q.Filter}%") ||
                                          EF.Functions.Like(x.Phone!, $"%{q.Filter}%")));
        }

        if (q.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, q.SortColumn));
        }

        var count = queries.Count();

        var customers = queries.AsNoTracking().Skip(q.Skip).Take(q.PageSize);

        return new Pager<VCustomerList>
        {
            Items = await customers.ToListAsync(),
            Count = count,
        };
    }

    public async Task<Result<Pager<T>>> GetCustomersAsync<T>(string? customerId, QueryParam param)
        where T : class
    {
        try
        {

            await using var cxt = await contextFactory.CreateDbContextAsync();

            var queries = cxt.Database.SqlQuery<VCustomerList>(
                $"SELECT * FROM VCustomerList WHERE IsLoanActive=CAST(0 AS BIT) AND IsActive=CAST(1 AS BIT)");

            if (!string.IsNullOrEmpty(param.Filter))
            {
                queries = queries.Where(x => EF.Functions.Like(x.FullName!, $"%{param.Filter}%") ||
                                             EF.Functions.Like(x.AltName!, $"%{param.Filter}%") ||
                                             EF.Functions.Like(x.AltPhone!, $"%{param.Filter}%") ||
                                             EF.Functions.Like(x.FirstName, $"%{param.Filter}%") ||
                                             EF.Functions.Like(x.LastName, $"%{param.Filter}%"));
            }


            if (customerId.IsNotNullOrEmpty())
            {
                queries = queries.Where(x => x.CustomerId == customerId);
            }

            var counts = queries.Count();

            var items = await queries
                                    .OrderBy(x => x.FirstName)
                                    .ThenBy(x => x.LastName)
                                    .ProjectToType<T>()
                                    .AsNoTracking()
                                    .Skip(param.Skip)
                                    .Take(param.PageSize)
                                    .ToListAsync();
            return Result.Success<Pager<T>>(new Pager<T>(items, counts));
        }
        catch (SqlException ex)
        {
            return Result.Failure<Pager<T>>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }

    public async Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, bool include = false)
    {
        try
        {
            using var db = await dapperFactory.CreateConnectionAsync();

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

            var coustmerDictionary = new Dictionary<string, VCustomerList>();
            //"spGetCustomerDetailsById"
            var customerInclude = await db.QueryAsync(ProcedureConstant.SpGetCustomerDetailsById,
                (VCustomerList customer, VLoanDisbursments loan, VLoanRequests loanRequest,
                    VCollateralLines collateralLine, VCustomerIncomes customerIncome, VCustomerExpenses customerExpense,
                    VCustomerNotes customerNote) =>
                {
                    if (coustmerDictionary.TryGetValue(customer.CustomerId!, out var existingCustomer))
                    {
                        customer = existingCustomer;
                    }
                    else
                    {
                        coustmerDictionary.Add(customer.CustomerId!, customer);
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


    public async Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(int? branchId, string? customerId, bool onlyComplete = false, CancellationToken cancellation = default)
    {
        try
        {

            using var db = await dapperFactory.CreateConnectionAsync(cancellation);

            var sql = @"SELECT
                            r.LoanRequestId,
				            r.RequestDate,
				            r.CreditAgentId,
				            r.RequestAmount,
				            r.AprovedAmount,
				            r.AprovedBy,
				            r.AprovedDate,
				            r.CurrencyId,
				            r.CurrencyName,
				            r.AltName,
				            r.BranchId,
				            r.BranchName,
				            r.[Status]
                        FROM VLoanRequests r WHERE r.CustomerId=@customerId AND ((@onlyComplete='TRUE' AND r.StatusId=7) OR @onlyComplete='FALSE')";


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
    
   


    public async Task<Result<Pager<T>>> GetCustomerLoanRequestAsync<T>(int? branchId, string? customerId, bool includeComplete = false)
        where T : class
    {
        try
        {
            var customerLoanRequests = await context.Database
                .SqlQuery<VLoanRequests>($"SELECT * FROM VLoanRequests WHERE CustomerId={customerId} AND BranchId={branchId}")
                .WhereIf(includeComplete, c => c.StatusId == (int)RequestStatus.Completed)
                .ProjectToType<T>()
                .AsNoTracking()
                .ToListAsync();

            if (!customerLoanRequests.Any())
            {
                return Result.Failure<Pager<T>>(ErrorResponse.NotFound("No note found."));
            }

            var counts = customerLoanRequests.Count;

            return Result.Success(new Pager<T>(customerLoanRequests, counts));
        }
        catch (SqlException ex)
        {
            return Result.Failure<Pager<T>>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
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

    public async Task<Result<IEnumerable<T>>> GetCustomerNotes<T>(string? customerId)
        where T : class
    {
        try
        {
            logger.LogInformation("try adding new customer note");

            var notes = await context.Database
                .SqlQuery<T>($@"SELECT 
	                 c.NoteId,
	                 c.CustomerRefId,
	                 CASE
		                WHEN c.CreditAgentName IS NULL THEN 'Administrator'
		                ELSE c.CreditAgentName
	                END AS CreditAgentName,
	                c.Description,
	                c.Created
                FROM VCustomerNotes c WHERE c.CustomerRefId={customerId}")
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

    public async Task<Result<IEnumerable<T>>> GetCustomerActivies<T>(string? customerId)
        where T : class
    {
        try
        {
            logger.LogInformation("try adding new customer note");

            var notes = await context.Database
                .SqlQuery<T>($"SELECT * FROM VCustomerActivities WHERE CustomerRefId={customerId}")
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

    public async Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId)
    {
        var customer = await context.EntityNames
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
        var customersQuery = context.Customers
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

    public async Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(string? customerId)
    {
        var customer = await context.CustomerLists.FirstOrDefaultAsync(x => x.CustomerId == customerId);

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
            await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

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


    public async Task<Result<string>> MakeCustomerInActiveAsync(string? customerId, int editSeq = 0, bool isActive = true)
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
                                   .Where(x => x.CustomerRefId == customerId)
                                   .ToListAsync(cancellation);
        return customerInUse.Any();
    }



}

