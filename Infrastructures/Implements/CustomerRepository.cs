using Applications.Bases;
using Applications.Extensions;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Customers.Response;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Customers;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans.Requests;
using Dapper;
using Domains.Models.Lists;
using Domains.Shared.Enums;
using ErrorOr;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace Infrastructures.Implements;

public sealed class CustomerRepository(IDbContextFactory<DataContext> contextFactory, IDapperConnectionFactory dapperFactory, ILogger<CustomerRepository> logger) : ICustomerService
{


    public async Task<Pager<VCustomerList>> GetCustomersAsync(int? branchId, string? userId, string? customerId, QueryParam param, bool isLoanActiveOnly = false, CancellationToken cancellation = default)
    {
        await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

        var queries = BuildCustomerQueriesAsync(cxt, branchId, userId, isLoanActiveOnly, cancellation);

        if (!string.IsNullOrEmpty(customerId))
        {
            queries = queries.Where(x => x.CustomerId == customerId);
        }

        queries = ApplyFiltering(queries, param);

        queries = ApplyOrdering(queries, param);

        var countTask = await queries.CountAsync(cancellation);

        var dataTask = await queries
            .Skip(param.Page)
            .Take(param.Take)
            .ToListAsync(cancellation);

        return new Pager<VCustomerList>(dataTask, countTask);

    }

    public async Task<ErrorOr<VCustomerDetails>> GetCustomerDetailByIdAsync(string? customerId, bool include = false, CancellationToken cancellationToken = default)
    {
        return await GetCustomerDetailAsync(customerId, include, cancellationToken);
    }

    public async Task<Pager<VLoanRequests>> GetCustomerLoanRequestAsync(string? customerId, CancellationToken cancellation = default)
    {
        using var db = await dapperFactory.CreateConnectionAsync(cancellation);

        var sql = @"SELECT * FROM VLoanRequests r 
                    WHERE r.CustomerId=@customerId AND r.StatusId NOT IN (7)";
        var parameters = new DynamicParameters();

        parameters.Add("@customerId", customerId);

        var rsp = await db.QueryAsync<VLoanRequests>(sql, parameters);

        return PagerExtensions.Create(rsp.ToList(), rsp.Count());

    }

    public async Task<Pager<VLoanDisbursments>> GetCustomerLoanAsync(string? customerId, CancellationToken cancellation = default)
    {
        using var db = await dapperFactory.CreateConnectionAsync(cancellation);

        var sql = @"SELECT * FROM  VLoansDisbursments r WHERE r.CustomerId=@customerId";

        var parameters = new DynamicParameters();

        parameters.Add("@customerId", customerId);

        var rsp = (await db.QueryAsync<VLoanDisbursments>(sql, parameters)).ToList();

        return PagerExtensions.Create(rsp, rsp.Count());
    }

    public async Task<EntityNames?> GetCustomerByIdAsync(string? customerId, CancellationToken cancellation = default)
    {

        await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

        var customer = await cxt.EntityNames
                        .Where(x => x.Id == customerId)
                        .Include(e => e.Customer)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == customerId, cancellation);
        return customer;
    }

    public async Task<VCustomerDetails?> GetCustomerForRequestLoanAsync(string? customerId, CancellationToken cancellation = default)
    {

        await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

        var customer = await cxt.VCustomerLists
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellation);

        return customer;
    }

    public async Task<Pager<VCollateralLines>> GetCustomerCollateralLines(string? customerId, bool includeInUsed = false, CancellationToken cancellation = default)
    {
        using var db = await dapperFactory.CreateConnectionAsync(cancellation);

        var sql = """
        SELECT * FROM VCustomerCollateralLines
        WHERE CollateralId = @customerId
          AND (@includeInUsed = 1 OR IsUsed = 0)
        """;

        var command = new CommandDefinition(
            sql,
            new { customerId, includeInUsed },
            cancellationToken: cancellation
        );

        var items = (await db.QueryAsync<VCollateralLines>(command)).ToList();

        return PagerExtensions.Create(items, items.Count);
    }


    public async Task<Pager<VLoanDisbursments>> GetCustomerLoanHistoryAsync(string? customerId, CancellationToken cancellation = default)
    {
        await using var db = await contextFactory.CreateDbContextAsync(cancellation);

        var loanHistories = await db.Set<VLoanDisbursments>()
                                    .Where(x => x.CustomerId == customerId)
                                    .ToListAsync(cancellation);

        return PagerExtensions.Create(loanHistories, loanHistories.Count);
    }


    public async Task<DeleteResult> DeleteAsync(string? customerId, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var customer = await context.Customers.FindAsync(customerId, cancellation);

            if (customer is null)
            {
                return DeleteResult.NotFound("There is no Customer in the List.");
            }

            if (await IsCustomerInUsed(customerId, cancellation))
            {
                return DeleteResult.InUse("This customer is being used by someone, you can't delete it.");
            }

            context.Customers.Remove(customer);

            await context.SaveChangesAsync();

            return DeleteResult.Success("Customer was removed");
        }
        catch (Exception ex)
        {
            return DeleteResult.Error($"{ex.InnerException!.Message}");
        }
    }


    public async Task<ErrorOr<bool>> MakeCustomerInActiveAsync(string? customerId, CancellationToken cancellation = default)
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
                return ErrorOr.Error.NotFound(Messages.DataNotFound);
            }

            existingCustomer.IsActive = false;

            await _context.SaveChangesAsync(cancellation);

            return true;
        }
        catch (SqlException ex)
        {
            return ErrorOr.Error.Unexpected($"{ex.InnerException!.Message}");
        }
    }

    public async Task<ErrorOr<VCustomerList>> CreateAsync(EntityNames model, CancellationToken cancellation = default)
    {
        try
        {
            if (await IsCustomerRegisterByPhone(null, model.Phone, cancellation))
            {
                return Error.Conflict(description: $"Phone Number:{model.Phone} is already registered.");
            }

            if (await IsCustomerRegisterIdCard(null, model.IdCard, cancellation))
            {
                return Error.Conflict(description: $"ID Card:{model.IdCard} is already registered.");
            }

            if (await IsCustomerRegisterByPassport(null, model.PassportId, cancellation))
            {
                return Error.Conflict(description: $"Passport ID:{model.PassportId} is already registered.");
            }

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            await AddNewIfNotExistJob(model, context, cancellation);

            await AddJobSpouseIfNotExistJob(model, context, cancellation);

            var customerSequence = await GetMaxSequenceNumber(context);

            model.Customer!.CustomerInfId = customerSequence;

            model.Customer.CustomerKey = CreateCustomerKey(customerSequence, model.FirstName, model.LastName);

            context.EntityNames.Add(model);

            await context.SaveChangesAsync(cancellation);

            var createdCustomer = await GetCustomerListByIdAsync(model.Id, cancellation);

            if (createdCustomer.IsError == false && createdCustomer.Value is { CustomerId: not null })
            {
                return createdCustomer.Value;
            }

            return ErrorOr.Error.NotFound("Not Found", "Created customer not found.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return ErrorOr.Error.Conflict("Conflict", $"{ex.InnerException!.Message}");
        }
        catch (Exception ex)
        {
            return ErrorOr.Error.Unexpected("Unexpected Error", $"{ex.Message}");
        }
    }

    public async Task<ErrorOr<VCustomerList>> UpdateAsync(EntityNames model, CancellationToken cancellation = default)
    {
        try
        {

            if (await IsCustomerRegisterByPhone(model.Id, model.Phone, cancellation))
            {
                return ErrorOr.Error.Conflict(description: $"Customer With Name:{model.AltName} is already register.");
            }

            if (await IsCustomerRegisterIdCard(model.Id, model.IdCard, cancellation))
            {
                return ErrorOr.Error.Conflict(description: $"Customer With Name:{model.AltName} is already register.");
            }

            if (await IsCustomerRegisterByPassport(model.Id, model.PassportId, cancellation))
            {
                return ErrorOr.Error.Conflict(description: $"Customer With Name:{model.AltName} is already register.");
            }

            await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);


            await AddNewIfNotExistJob(model, cxt, cancellation);

            await AddJobSpouseIfNotExistJob(model, cxt, cancellation);

            cxt.EntityNames.Update(model);


            await cxt.SaveChangesAsync(cancellation);

            var updatedCustomer = await GetCustomerListByIdAsync(model.Id, cancellation);

            if (!updatedCustomer.IsError && updatedCustomer.Value is { CustomerId: not null })
            {
                return updatedCustomer.Value;
            }

            return ErrorOr.Error.NotFound("Not Found", "Updated customer not found.");

        }
        catch (DbUpdateConcurrencyException ex)
        {
            return ErrorOr.Error.Failure($"{ex.InnerException!.Message}");
        }
    }





    /// <summary>
    /// Private Field to check customer is in used
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    private async Task<bool> IsCustomerInUsed(string? customerId, CancellationToken cancellation = default)
    {
        await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

        var customerInUse = await _context.LoanDisbursments
                                   .AsNoTracking()
                                   .AnyAsync(x => x.CustomerRefId == customerId, cancellation);
        return customerInUse;
    }

    private async Task<bool> IsCustomerRegisterByPhone(string? customerId, string? phoneNumber, CancellationToken cancellation = default)
    {

        if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("Phone Number is required", nameof(phoneNumber));

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        return await context.VCustomerLists
                            .AsNoTracking()
                            .AnyAsync(x => (x.Phone == phoneNumber && (string.IsNullOrEmpty(x.CustomerId) || x.CustomerId != customerId)), cancellation);

    }


    private async Task<bool> IsCustomerRegisterByPassport(string? customerId, string? passport, CancellationToken cancellation = default)
    {

        if (string.IsNullOrWhiteSpace(passport)) return false;

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        return await context.VCustomerLists
                            .AsNoTracking()
                            .AnyAsync(x => (x.PassportId == passport && (string.IsNullOrEmpty(x.CustomerId) || x.CustomerId != customerId)), cancellation);

    }

    private async Task<bool> IsCustomerRegisterIdCard(string? customerId, string? idCard, CancellationToken cancellation = default)
    {
        if (string.IsNullOrWhiteSpace(idCard)) throw new ArgumentException("ID Card is required", nameof(idCard));

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        return await context.VCustomerLists
                            .AsNoTracking()
                            .AnyAsync(x => (x.IdCard == idCard && (string.IsNullOrEmpty(x.CustomerId) || x.CustomerId != customerId)), cancellation);

    }
    private async Task<long> GetMaxSequenceNumber(DataContext context)
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
            query = query.OrderByDescending(x => x.ModifiedDT)
                        .ThenByDescending(x => x.FirstName)
                        .ThenByDescending(x => x.LastName);
        }

        return query;
    }


    private async Task<ErrorOr<VCustomerDetails>> GetCustomerDetailAsync(string? customerId, bool include = false, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching customer details for ID {CustomerId} (Include Related: {Include})", customerId, include);

        using var db = await dapperFactory.CreateConnectionAsync(cancellationToken);

        var parameters = new
        {
            customerId = customerId,
            include = include
        };

        using var multi = await db.QueryMultipleAsync(ProcedureConstant.SpGetCustomerDetailsById, parameters, commandType: CommandType.StoredProcedure);

        var customer = await multi.ReadSingleOrDefaultAsync<VCustomerDetails>();

        if (customer == null)
        {
            return ErrorOr.Error.NotFound("NotFound", "Customer not found.");
        }

        if (!include)
        {
            return customer;
        }

        customer.Loans = (await multi.ReadAsync<VLoanDisbursments>()).ToList();
        customer.LoanRequests = (await multi.ReadAsync<VLoanRequests>()).ToList();
        customer.CollateralLines = (await multi.ReadAsync<VCollateralLines>()).ToList();
        customer.CustomerIncomes = (await multi.ReadAsync<VCustomerIncomes>()).ToList();
        customer.CustomerExpenses = (await multi.ReadAsync<VCustomerExpenses>()).ToList();
        customer.CustomerNotes = (await multi.ReadAsync<VCustomerNotes>()).ToList();
        customer.CustomerActivities = (await multi.ReadAsync<VCustomerActivities>()).ToList();
        return customer;

    }


    private IQueryable<VCustomerList> ApplyFiltering(IQueryable<VCustomerList> queries, QueryParam param)
    {
        var filter = param.Filter?.Trim();

        if (param.IncludeInActive)
        {
            queries = queries.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(filter) && string.IsNullOrEmpty(param.FilterColumn))
        {
            var props = new[] { "FullName", "AltName", "Phone", "FirstName", "LastName" };

            queries = queries.WhereMatchesFilter(filter, props);
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

        return queries;
    }


    private static Expression<Func<VCustomerDetails, VCustomerList>> ProjectToCustomerList()
    {
        return x => new VCustomerList
        {
            CustomerId = x.CustomerId,
            CustomerKey = x.CustomerKey,
            FirstName = x.FirstName,
            LastName = x.LastName,
            FullName = x.FullName,
            AltName = x.AltName,
            Phone = x.Phone,
            EmailAddress = x.EmailAddress,
            IsLoanActive = x.IsLoanActive,
            IsActive = x.IsActive,
            City = x.City,
            VilInKhmer = x.VilInKhmer,
            ComInKhmer = x.ComInKhmer,
            DisInKhmer = x.DisInKhmer,
            ProInKhmer = x.ProInKhmer,
            ModifiedDT = x.ModifiedDT,
        };
    }

    private static Expression<Func<VCustomerList, VCustomerList>> ProjectTo()
    {
        return x => new VCustomerList
        {
            CustomerId = x.CustomerId,
            CustomerKey = x.CustomerKey,
            FirstName = x.FirstName,
            LastName = x.LastName,
            FullName = x.FullName,
            AltName = x.AltName,
            Phone = x.Phone,
            EmailAddress = x.EmailAddress,
            IsLoanActive = x.IsLoanActive,
            IsActive = x.IsActive,
            City = x.City,
            VilInKhmer = x.VilInKhmer,
            ComInKhmer = x.ComInKhmer,
            DisInKhmer = x.DisInKhmer,
            ProInKhmer = x.ProInKhmer,
            ModifiedDT = x.ModifiedDT,
        };
    }

    private IQueryable<VCustomerList> BuildCustomerQueriesAsync(DataContext cxt, int? branchId, string? userId, bool activeLoanOnly = false, CancellationToken cancellation = default)
    {
        //if (branchId > 0 || !string.IsNullOrEmpty(userId))
        //{
            return GetCustomersByBranchOrUserAsync(cxt, branchId, userId, activeLoanOnly, cancellation);
        //}

        //return GetCustomersWithoutActiveLoan(cxt, activeLoanOnly);
    }

    private IQueryable<VCustomerList> GetCustomersByBranchOrUserAsync(DataContext cxt, int? branchId, string? userId, bool activeLoanOnly = false, CancellationToken cancellation = default)
    {
        return cxt.Set<VCustomerDetails>()
                                .WhereIf(branchId > 0, x => x.BranchId == branchId)
                                .WhereIf(!string.IsNullOrEmpty(userId), x => x.CreatedBy == userId)
                                .WhereIf(activeLoanOnly, x => x.IsLoanActive == true)
                                .Select(ProjectToCustomerList())
                                .AsQueryable();
    }

    private IQueryable<VCustomerList> GetCustomersWithoutActiveLoan(DataContext cxt, bool isInlcudeActiveLoan = false)
    {
        return cxt.Set<VCustomerDetails>()
                    .WhereIf(isInlcudeActiveLoan, x => x.IsLoanActive == true)
                    .Select(ProjectToCustomerList())
                    .AsQueryable();
    }


    private async Task<ErrorOr<VCustomerList>> GetCustomerListByIdAsync(string? customerId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching customer details for ID {CustomerId}", customerId);

        using var db = await dapperFactory.CreateConnectionAsync(cancellationToken);

        var parameters = new
        {
            customerId = customerId
        };

        var sql = @"SELECT c.* FROM VCustomerList c WHERE c.CustomerId=@customerId";

        var customer = await db.QueryFirstOrDefaultAsync<VCustomerList>(sql, parameters);

        if (customer == null)
        {
            return ErrorOr.Error.NotFound("Customer not found.");
        }

        return customer;

    }

    private static async Task AddNewIfNotExistJob(EntityNames model, DataContext context, CancellationToken cancellation)
    {
        if (!string.IsNullOrEmpty(model.Occupation))
        {

            var duplicate = await context.Occupations
                                    .AnyAsync(x => x.OccupationTitle.Equals(model.Occupation), cancellation);

            if (duplicate) return;

            context.Occupations.Add(new Occupation
            {
                OccupationTitle = model.Occupation,
                Khmer = model.Occupation,
                TypeOf = TypeOfEnum.Job.ToString()
            });

            await context.SaveChangesAsync(cancellation);

        }
    }


    private static async Task AddJobSpouseIfNotExistJob(EntityNames model, DataContext context, CancellationToken cancellation)
    {
        if (model.Customer is not null && !string.IsNullOrEmpty(model.Customer.SpouseOccupation))
        {

            var duplicate = await context.Occupations
                                    .AnyAsync(x => x.OccupationTitle.Equals(model.Customer.SpouseOccupation), cancellation);

            if (duplicate) return;

            context.Occupations.Add(new Occupation
            {
                OccupationTitle = model.Customer.SpouseOccupation,
                Khmer = model.Customer.SpouseOccupation,
                TypeOf = TypeOfEnum.Job.ToString()
            });

            await context.SaveChangesAsync(cancellation);

        }
    }



}



//public async Task<Pager<VCustomerList>> GetCustomersAsync(int? branchId, string? userId, string? customerId, QueryParam param, CancellationToken cancellation = default)
//{
//    await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

//    var queries = BuildCustomerQueriesAsync(cxt, branchId, userId, false, cancellation);

//    if (!string.IsNullOrEmpty(customerId))
//    {
//        queries = queries.Where(x => x.CustomerId == customerId);
//    }

//    queries = ApplyFiltering(queries, param);

//    queries = ApplyOrdering(queries, param);

//    var countTask = await queries.CountAsync(cancellation);

//    var dataTask = await queries
//        .Skip(param.Page)
//        .Take(param.PageSize)
//        .ToListAsync(cancellation);

//    return new Pager<VCustomerList>(dataTask, countTask);

//}

//public async Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId)
//{
//    using var db = dapperFactory.CreateConnection();

//    var sql = @"SELECT * FROM VLoansDisbursments WHERE CustomerId=@customerId";

//    var loanHistories = await db.QueryAsync<VLoanDisbursments>(sql, new { @customerId = customerId });

//    return Result.Success(loanHistories);
//}




//public async Task<Result<string>> MakeCustomerInActiveAsync(string? customerId, bool state, CancellationToken cancellation = default)
//{
//    try
//    {

//        await using var _context = await contextFactory.CreateDbContextAsync(cancellation);

//        var existingCustomer = await _context.EntityNames
//                                            .Where(x => x.Id == customerId)
//                                            .Include(x => x.Customer)
//                                            .FirstOrDefaultAsync(cancellation);
//        if (existingCustomer is null)
//        {
//            return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
//        }

//        existingCustomer.IsActive = state;

//        await _context.SaveChangesAsync(cancellation);

//        return Result.Success<string>("customer state was changed.");
//    }
//    catch (SqlException ex)
//    {
//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}

//public async Task<Result<string>> DeleteCustomerAsync(string? customerId, CancellationToken cancellation = default)
//{
//    try
//    {
//        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

//        var customer = await context.Customers.FindAsync(customerId, cancellation);

//        if (customer is null)
//        {
//            return Result.Failure<string>(ErrorResponse.NotFound("There is no Customer in the List."));
//        }

//        if (await IsCustomerInUsed(customerId, cancellation))
//        {
//            var rsp = await MakeCustomerInActiveAsync(customerId, false, cancellation);

//            if (rsp.IsSuccess)
//            {
//                return Result.Success("This customer was disable,because of is being used by someone.");
//            }
//        }

//        context.Customers.Remove(customer);

//        await context.SaveChangesAsync();

//        return Result.Success("Customer was removed");
//    }
//    catch (Exception ex)
//    {
//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}


//public async Task<Result<string>> UpdateAsync(EntityNames model, CancellationToken cancellation = default)
//{
//    try
//    {

//        if (await IsCustomerRegister(model.Id, model.Phone, model.IdCard, cancellation))
//        {
//            return Result.Failure<string>(ErrorResponse.ServerError($"Customer With Name:{model.AltName} is already register."));
//        }

//        await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

//        cxt.EntityNames.Update(model);

//        await cxt.SaveChangesAsync(cancellation);

//        return Result.Success<string>("Customer update customer.");
//    }
//    catch (DbUpdateConcurrencyException ex)
//    {
//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}


//public async Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(string? customerId, bool onlyComplete = false, CancellationToken cancellation = default)
//{
//    try
//    {
//        using var db = await dapperFactory.CreateConnectionAsync(cancellation);

//        var sql = @"SELECT *
//                        FROM VLoanRequests r 
//                    WHERE r.CustomerId=@customerId AND r.StatusId NOT IN (7)";
//        var parameters = new DynamicParameters();
//        parameters.Add("@customerId", customerId);
//        var rsp = await db.QueryAsync<VLoanRequests>(sql, parameters);
//        if (!rsp.Any())
//        {
//            return Result.Failure<IEnumerable<VLoanRequests>>(ErrorResponse.NotFound("No note found."));
//        }
//        return Result.Success(rsp);
//    }
//    catch (SqlException ex)
//    {
//        return Result.Failure<IEnumerable<VLoanRequests>>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}


//public async Task<Result<string>> CreateAsync(EntityNames model, CancellationToken cancellation = default)
//{
//    try
//    {
//        if (await IsCustomerRegister(null, model.Phone, model.IdCard, cancellation))
//        {
//            return Result.Failure<string>(ErrorResponse.Conflict($"Customer With Name:{model.AltName} is already register."));
//        }

//        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

//        var customerSequence = await GetMaxSequenceNumber(context);

//        model.Customer!.CustomerInfId = customerSequence;

//        model.Customer.CustomerKey = CreateCustomerKey(customerSequence, model.FirstName, model.LastName);

//        context.EntityNames.Add(model);

//        await context.SaveChangesAsync(cancellation);

//        return Result.Success<string>($"New Customer was added.");
//    }
//    catch (DbUpdateConcurrencyException ex)
//    {
//        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
//    }
//}


//private IQueryable<VCustomerDetails> ApplyFiltering(IQueryable<VCustomerDetails> queries, QueryParam param)
// {
//     if (!string.IsNullOrEmpty(param.Filter) && string.IsNullOrEmpty(param.FilterColumn))
//     {
//         queries = queries.Where(x => EF.Functions.Like(x.FullName ?? "", $"{param.Filter}%") ||
//                                      EF.Functions.Like(x.AltName ?? "", $"{param.Filter}%") ||
//                                      EF.Functions.Like(x.AltPhone ?? "", $"{param.Filter}%") ||
//                                      EF.Functions.Like(x.FirstName ?? "", $"{param.Filter}%") ||
//                                      EF.Functions.Like(x.LastName ?? "", $"{param.Filter}%") ||
//                                      x.CustomerKey == param.Filter);
//     }

//     if (!string.IsNullOrEmpty(param.Filter) && !string.IsNullOrEmpty(param.FilterColumn))
//     {
//         var property = typeof(VCustomerDetails).GetProperty(param.FilterColumn);

//         if (property != null)
//         {
//             queries = queries.Where(e =>
//                 EF.Functions.Like(EF.Property<string>(e, param.FilterColumn), $"{param.Filter}%"));
//         }
//     }

//     return queries;
// }


//private IQueryable<VCustomerDetails> ApplyOrdering(IQueryable<VCustomerDetails> query, QueryParam param)
//{
//    if (!string.IsNullOrWhiteSpace(param.SortColumn))
//    {
//        var isDescending = param.SortDescending;

//        var property = typeof(VCustomerDetails).GetProperty(param.SortColumn);

//        if (property != null)
//        {
//            query = isDescending
//                ? query.OrderByDescending(e => EF.Property<object>(e, param.SortColumn))
//                : query.OrderBy(e => EF.Property<object>(e, param.SortColumn));
//        }
//    }
//    else
//    {
//        query = query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
//    }

//    return query;
//}

//public async Task<Pager<VCustomerList>> GetCustomersAsync(string? customerId, QueryParam param, CancellationToken cancellation = default)
//{
//    await using var cxt = await contextFactory.CreateDbContextAsync(cancellation);

//    var baseQueries = cxt.VCustomerLists.AsNoTracking().Where(x => x.IsLoanActive == false);


//    baseQueries = ApplyFiltering(baseQueries, param);

//    if (customerId.IsNotNullOrEmpty())
//    {
//        baseQueries = baseQueries.Where(x => x.CustomerId == customerId);
//    }

//    baseQueries = ApplyOrdering(baseQueries, param);

//    var finalQueries = baseQueries.Select(ProjectToCustomerList());

//    var countTask = finalQueries.CountAsync(cancellation);

//    var dataTask = finalQueries
//                    .Skip(param.Skip)
//                    .Take(param.PageSize)
//                    .ToListAsync(cancellation);


//    await Task.WhenAll(dataTask, countTask);

//    return new Pager<VCustomerList>(dataTask.Result, countTask.Result);

//}

//public async Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes)
//{

//    await using var cxt = await contextFactory.CreateDbContextAsync();

//    var customersQuery = cxt.Customers
//                                 .AsNoTracking()
//                                  .Where(x => x.CustomerId == customerId);
//    if (includes.Any())
//    {
//        foreach (var includeProp in includes)
//        {
//            customersQuery = customersQuery.Include(includeProp);
//        }
//    }

//    if (!customersQuery.Any())
//    {
//        return Result.Failure<Customer>(ErrorResponse.ServerError($"no customer found"));
//    }

//    var customer = await customersQuery
//                        .AsNoTracking()
//                        .FirstOrDefaultAsync();

//    if (customer is null)
//    {
//        return Result.Failure<Customer>(ErrorResponse.ServerError($"no customer found"));
//    }

//    return Result.Success(customer);
//}



//private async Task<List<VCustomerActivities>> GetCustomerActivities(string? customerId, DataContext context, CancellationToken cancellation)
//{
//    return await context.VCustomerActivities
//        .Where(x => x.CustomerRefId == customerId)
//        .AsNoTracking()
//        .ToListAsync(cancellation);
//}

//private async Task<List<VCollateralLines>> GetCustomerCollateral(string? customerId, DataContext context, CancellationToken cancellation)
//{
//    return await context.VCollateralLines
//        .Where(x => x.CollateralId == customerId)
//        .AsNoTracking()
//        .ToListAsync(cancellation);
//}

//private async Task<List<VCustomerIncomes>> GetCustomerIncomes(string? customerId, DataContext context, CancellationToken cancellation)
//{
//    return await context.VCustomerIncomes
//                .Where(x => x.CustomerRefId == customerId)
//                .AsNoTracking()
//                .ToListAsync(cancellation);
//}

//private async Task<List<VCustomerExpenses>> GetCustomerExpenses(string? customerId, DataContext context, CancellationToken cancellation)
//{
//    return await context.VCustomerExpenses
//        .Where(x => x.CustomerRefId == customerId)
//        .AsNoTracking()
//        .ToListAsync(cancellation);
//}

//private async Task<List<VCustomerNotes>> GetCustomerNotes(string? customerId, DataContext context, CancellationToken cancellation)
//{
//    return await context.VCustomerNotes
//                .Where(x => x.CustomerRefId == customerId)
//                .AsNoTracking()
//                .ToListAsync(cancellation);
//}

//private async Task<List<VLoanDisbursments>> GetCustomerLoans(string? customerId, DataContext context, CancellationToken cancellation)
//{
//    return await context.VLoanDisbursements
//        .Where(x => x.CustomerId == customerId)
//        .AsNoTracking()
//        .ToListAsync(cancellation);
//}

//private async Task<List<VLoanRequests>> GetCustomerLoanRequests(string? customerId, DataContext context, CancellationToken cancellation)
//{
//    return await context.VLoanRequests
//                .Where(x => x.CustomerId == customerId)
//                .AsNoTracking()
//                .ToListAsync(cancellation);
//}



//public async Task<Result<VCustomerDetails>> GetCustomerDetailIdAsync(string? customerId, bool includes, CancellationToken cancellation = default)
//{
//    logger.LogInformation("Fetching customer details for ID {CustomerId}", customerId);

//    await using var context = await contextFactory.CreateDbContextAsync(cancellation);

//    var customer = await context.VCustomerLists
//                               .AsNoTracking()
//                               .FirstOrDefaultAsync(x => x.CustomerId == customerId);

//    if (customer is null)
//    {
//        return Result.Failure<VCustomerDetails>(ErrorResponse.NotFound("Customer Not Found."));
//    }

//    if (!includes)
//    {
//        return Result.Success(customer);
//    }

//    List<VLoanRequests> loanRequestsTask = await GetCustomerLoanRequests(customerId, context, cancellation);

//    List<VLoanDisbursments> loanDisbursementsTask = await GetCustomerLoans(customerId, context, cancellation);

//    List<VCustomerNotes> customerNotesTask = await GetCustomerNotes(customerId, context, cancellation);

//    List<VCustomerExpenses> customerExpensesTask = await GetCustomerExpenses(customerId, context, cancellation);

//    List<VCustomerIncomes> customerIncomesTask = await GetCustomerIncomes(customerId, context, cancellation);

//    List<VCollateralLines> collateralLinesTask = await GetCustomerCollateral(customerId, context, cancellation);

//    List<VCustomerActivities> customerActivitiesTask = await GetCustomerActivities(customerId, context, cancellation);

//    customer.LoanRequests = loanRequestsTask;
//    customer.Loans = loanDisbursementsTask;
//    customer.CustomerNotes = customerNotesTask;
//    customer.CustomerExpenses = customerExpensesTask;
//    customer.CustomerIncomes = customerIncomesTask;
//    customer.CollateralLines = collateralLinesTask;
//    customer.CustomerActivities = customerActivitiesTask;

//    return Result.Success(customer);
//}
