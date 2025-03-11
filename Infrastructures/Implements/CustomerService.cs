using System.Data;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Dapper;
using Domains.Models.Lists.Customers;
using Infrastructures.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CoreBussiness.Implements;

public class CustomerService : ICustomerService
{

    private readonly ApplicationDbContext _context;

    private readonly IDapperConnectionFactory _db;

    public CustomerService(ApplicationDbContext context, IDapperConnectionFactory dapperService)
    {
        _context = context;
        _db = dapperService;
    }
    public async Task<Result<IEnumerable<VCustomerList>>> GetCustomersAsync(long customerId, int take = 0,
        bool includeInactive = false,
        bool exceptActiveLoan = true,
        string? searchTerm = null)
    {
        IQueryable<VCustomerList> customerQueries = _context.CustomerLists;

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
                                              EF.Functions.Like(EF.Functions.Collate(x.CustomerId!.ToString(), "Khmer_100_CI_AI_SC_UTF8"), $"%{searchTerm}%") ||
                                              EF.Functions.Like(x.Phone!, $"%{searchTerm}%")));
        }
        if (customerId > 0)
        {
            customerQueries = customerQueries.Where(x => x.CustomerId == customerId);
        }
        if (take > 0)
        {
            customerQueries = customerQueries.Take(take).OrderBy(x => x.CustomerId);
        }

        if (!customerQueries.Any())
        {
            return Result.Failure<IEnumerable<VCustomerList>>(ErrorResponse.NotFound("no customer found."));
        }

        var results = await customerQueries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VCustomerList>>(results);

    }
    public async Task<Pager<VCustomerList>> GetCustomers(QueryParam q)
    {
        IQueryable<VCustomerList> queries = _context.CustomerLists;

        if (!q.IsIncludeInActive)
        {
            queries = queries.Where(x => x.IsActive == true);
        }

        if (!string.IsNullOrEmpty(q.Filter))
        {
            queries = queries.Where(x => (EF.Functions.Like(x.FullName!, $"%{q.Filter}%") ||
                                              EF.Functions.Like(EF.Functions.Collate(x.CustomerId.ToString(), "Khmer_100_CI_AI_SC_UTF8"), $"%{q.Filter}%") ||
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
    public async Task<Result<Customer>> GetCustomerByIdAsync(long customerId)
    {
        IQueryable<Customer> customersQuery = _context.Customers
                                            .Include(x => x.Collateral)
                                                .ThenInclude(x => x!.CollateralLines)
                                            .Include(x => x.CustomerExpenses)
                                            .Include(x => x.CustomerIncomes);
        //.Include(x => x.);
        if (!customersQuery.Any())
        {
            return Result.Failure<Customer>(ErrorResponse.ServerError($"no customer found"));
        }
        var customer = await customersQuery
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        return Result.Success<Customer>(customer!);
    }
    public async Task<Result<Customer>> GetCustomerAsync(long customerId, params string[] includes)
    {
        IQueryable<Customer> customersQuery = _context.Customers
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

        return Result.Success(customer!);
    }
    public async Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(long customerId)
    {
        IQueryable<VCustomerList> customersQuery = _context.CustomerLists
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
    public async Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(long customerId)
    {
        var db = await _db.CreateConnectionAsync();
        var results = await db.QueryMultipleAsync("spGetCustomerDetails", new
        {
            @customerId = customerId
        }, commandType: CommandType.StoredProcedure);
        var customer = await results.ReadFirstOrDefaultAsync<VCustomerList>();
        if (customer is null)
        {
            return Result.Failure<VCustomerList>(ErrorResponse.NotFound("No Customer found."));
        }
        var expenses = results.Read<VCustomerExpenses>().ToList();
        var incomes = results.Read<VCustomerIncomes>().ToList();
        var notes = results.Read<VCustomerNotes>().ToList();
        var requests = results.Read<VLoanRequests>().ToList();
        var collaterallines = results.Read<VCollateralLines>().ToList();
        customer.CustomerExpenses = expenses ?? [];
        customer.CustomerIncomes = incomes ?? [];
        customer.LoanRequests = requests ?? [];
        customer.CustomerNotes = notes ?? [];
        customer.CollateralLines = collaterallines ?? [];
        return Result.Success(customer);
    }
    public async Task<Result<VCustomerList>> GetCustomerDetailsById(long customerId, bool include = false)
    {
        try
        {
            using var db = await _db.CreateConnectionAsync();

            if (!include)
            {
                var customer = await db.QueryFirstOrDefaultAsync<VCustomerList>("spGetCustomerDetailsById",
                    new
                    {
                        @customerId = customerId,
                        @include = include
                    }, commandType: CommandType.StoredProcedure);

                if (customer is null)
                {
                    return Result.Failure<VCustomerList>(ErrorResponse.NotFound("no data found."));
                }

                return Result.Success(customer);

            }
            var coustmerDictionary = new Dictionary<long, VCustomerList>();

            var customerInclude = await db.QueryAsync<VCustomerList,
                                            VLoanDisbursments,
                                            VLoanRequests,
                                            VCollateralLines,
                                            VCustomerIncomes,
                                            VCustomerExpenses,
                                            VCustomerNotes,
                                            VCustomerList>("spGetCustomerDetailsById",
                                            (customer, loan, loanRequest, collateralLine, customerIncome, customerExpense, customerNote) =>
                                            {
                                                if (coustmerDictionary.TryGetValue(customer.CustomerId, out var existingCustomer))
                                                {
                                                    customer = existingCustomer;
                                                }
                                                else
                                                {
                                                    coustmerDictionary.Add(customer.CustomerId, customer);
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
                                                @customerId = customerId,
                                                @include = include
                                            },
                                            commandType: CommandType.StoredProcedure,
                                            splitOn: "Id,LoanRequestId,CustomerRefId,IncomeId,ExpendId,NoteId");


            if (!customerInclude.Any()) return Result.Failure<VCustomerList>(ErrorResponse.NotFound("No note found."));

            var item = customerInclude.First();

            return Result.Success(item);
        }
        catch (SqlException ex)
        {
            return Result.Failure<VCustomerList>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
    public async Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(long customerId,bool includeInUsed=false)
    {
        var queries = _context.Database.SqlQuery<VCollateralLines>($"SELECT * FROM VCollateralLines WHERE CollateralId={customerId}");

        if (!includeInUsed)
        {
            queries = queries.Where(x => x.IsInUsed == false);
        }
        
        if (!queries.Any())
        {
            return Result.Failure<IEnumerable<VCollateralLines>>(ErrorResponse.NotFound("no data."));
        }

        var items=await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VCollateralLines>>(items);
    }
    public async Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(long customerId)
    {
        using var db = _context.Database.GetDbConnection();

        var sql = @"SELECT * FROM VLoanDisbursments WHERE CustomerId=@customerId";

        var loanHistories = db.Query<VLoanDisbursments>(sql, new { @customerId = customerId }).ToList();

        var loanHistoriesResult = await Task.FromResult(loanHistories);

        return Result.Success<IEnumerable<VLoanDisbursments>>(loanHistoriesResult);
    }
    public async Task<Result<string>> DeleteCustomerAsync(long customerId)
    {
        try
        {
            var customer = await _context.Customers
                                        .FirstOrDefaultAsync(x => x.CustomerId == customerId);
            if (customer is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no Customer in the List."));

            }

            var customerInUse = _context.LoanDisbursments.AsNoTracking().Where(x => x.CustomerRefId == customerId).ToList();

            if (customerInUse.Any())
            {
                return Result.Failure<string>(ErrorResponse.Conflict("This Customer information is in use, so cannot delete"));
            }
            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return Result.Success("Customer was removed");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }
    }
    public async Task<Result<string>> AddOrUpdateCustomerAsync(Customer customer)
    {
        try
        {

            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == customer.CustomerId);

            if (existingCustomer is null && customer.CustomerId == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                _context.Entry(existingCustomer!).CurrentValues.SetValues(customer);
            }

            await _context.SaveChangesAsync();

            return Result.Success(new string("New Customer was crated"));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }
    public async Task<Result<string>> UpdateCustomerAsync(Customer customer)
    {
        try
        {
            var existedCustomer = _context.Customers
                                       .AsNoTracking()
                                       .Where(x => x.CustomerId == customer.CustomerId && customer.EditSeq == customer.EditSeq)
                                       .FirstOrDefault();

            if (existedCustomer is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no any customer in the system."));
            }
            _context.Customers.Update(customer);

            await _context.SaveChangesAsync();

            return Result.Success<string>(new string("customer was updated"));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }
    public async Task<Result<string>> MakeCustomerInActiveAsync(long customerId, int editSeq = 0, bool isActive = true)
    {
        try
        {
            var increaseEdit = editSeq + 1;
            var updatedRows = await _context.Customers.Where(x => x.CustomerId == customerId && x.EditSeq == editSeq)
                                  .ExecuteUpdateAsync(prop => prop
                                                              .SetProperty(x => x.IsActive, isActive)
                                                              .SetProperty(x => x.EditSeq, increaseEdit));
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


}

