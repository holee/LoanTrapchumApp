using Applications.Bases;
using Applications.Commons.Messages.Employees;
using Applications.Enumerations;
using Applications.Exceptions;
using Applications.Extensions;
using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Employees;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Applications.Responses.ViewModels.Loans.Requests;
using Dapper;
using Domains.Models.Lists;
using Domains.Models;
using ErrorOr;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;
using Applications.Responses.ViewModels.Auths;

namespace Infrastructures.Implements;

public sealed class EmployeeRepository(IDapperConnectionFactory connectionFactory,
    IDbContextFactory<DataContext> dbContextFactory,
    ILogger<EmployeeRepository> logger) : IEmployeeService
{
    private readonly IDapperConnectionFactory _connectionFactory = connectionFactory;
    private readonly IDbContextFactory<DataContext> _dbContextFactory = dbContextFactory;
    private readonly ILogger<EmployeeRepository> _logger = logger;

    public async Task<ErrorOr<VEmployeeList>> CreateAsync(EntityNames employee, CancellationToken cancellation = default)
    {
        try
        {
            await using var _context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            _context.EntityNames.Add(employee);

            await _context.SaveChangesAsync();

            var emp = await GetEmployeeDetailsByIdAsync(employee.Id, null, true);

            return emp;
        }
        catch (Exception ex)
        {
            return Error.Failure("ServerError", ex.InnerException!.Message);
        }
    }

    public async Task<ErrorOr<VEmployeeList>> UpdateAsync(EntityNames employee, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var contact = employee.Employee?.Contact;

            if (employee.Employee is not null)

                employee.Employee.Contact = null;

            context.EntityNames.Update(employee);

            if (contact is not null && !string.IsNullOrEmpty(contact.ContactId))
            {
                var exists = await context.Set<EmergencyContact>()
                    .AsNoTracking()
                    .AnyAsync(x => x.ContactId == contact.ContactId, cancellation);

                if (exists)
                    context.Set<EmergencyContact>().Update(contact);
                else
                    context.Set<EmergencyContact>().Add(contact);
            }

            await context.SaveChangesAsync(cancellation);

            var emp = await GetEmployeeDetailsByIdAsync(employee.Id, null, false);

            return emp;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Error.Failure("ServerError", ex.Message);
        }
    }

    public async Task<Result<string>> UpdateIsActiveAsync(string? id, bool state, CancellationToken cancellation = default)
    {
        try
        {
            await using var _context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var employee = await _context.EntityNames.AsNoTracking()
                                            .IgnoreQueryFilters()
                                            .FirstOrDefaultAsync(emp => emp.Id == id);
            if (employee == null)
            {
                return Applications.Responses.Result.Failure<string>(ErrorResponse.NotFound("No Employee found."));
            }

            int editSeq = employee!.EditSeq + 1;
            bool isActive = employee!.IsActive!.Value ? false : true;
            var rowEffect = _context.EntityNames
                .Where(emp => emp.Id == id && emp.EditSeq == employee!.EditSeq)
                .ExecuteUpdate(props =>
                    props.SetProperty(x => x.IsActive, isActive)
                        .SetProperty(x => x.ModifiedDt, DateTime.Now)
                        .SetProperty(x => x.EditSeq, editSeq)
                );
            if (rowEffect > 0)
            {
                return Applications.Responses.Result.Success<string>(new("Employee was updated!"));
            }

            return Applications.Responses.Result.Failure<string>(ErrorResponse.ServerError("Cannot update employee"));
        }
        catch (DbUpdateException ex)
        {
            return Applications.Responses.Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
        }
    }



    public async Task<ErrorOr<VEmployeeList>> UpdateEmployeeStateAsync(string? id, bool state, CancellationToken ct = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(ct);

        var currentSeq = await context.EntityNames
            .IgnoreQueryFilters()
            .Where(x => x.Id == id)
            .Select(x => x.EditSeq)
            .FirstOrDefaultAsync(ct);

        var rows = await context.EntityNames
            .Where(x => x.Id == id && x.EditSeq == currentSeq)
            .ExecuteUpdateAsync(p => p
                .SetProperty(x => x.IsActive, state)
                .SetProperty(x => x.ModifiedDt, DateTime.UtcNow)
                .SetProperty(x => x.EditSeq, x => x.EditSeq + 1),
                ct);

        if (rows == 0)
            return Error.NotFound("Employee not found");

        return await context.EntityNames
            .Where(x => x.Id == id)
            .Select(Projection())
            .FirstAsync(ct);
    }




    public async Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId, string? employeeId, QueryParam q, CancellationToken cancellation = default)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.EmployeeLists.AsQueryable();

        queries = ApplyFiltering(queries, branchId, employeeId, q);

        queries = ApplyOrdering(queries, q);

        var counts = queries.Count();

        if (q.Take > 0)
        {
            queries = queries.Skip(q.Page).Take(q.Take);
        }

        var items = await queries.AsNoTracking()
                                .Select(ProjectionTo())
                                .ToListAsync(cancellation);
        return new Pager<VEmployeeList>(items, counts);
    }


    public async Task<ErrorOr<EntityNames>> GetByIdAsync(string? id, int? branchId, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var employee = await context.EntityNames
                                .Include(e => e.Employee)
                                    .ThenInclude(e => e!.Department)
                                .AsNoTracking()
                                .WhereIf(branchId.HasValue && branchId > 0, e => e.Employee!.BranchRefId == branchId)
                                .FirstOrDefaultAsync(e => e.Id == id, cancellation);

            if (employee is not null)
            {
                return employee;
            }

            return Error.NotFound(EmployeeMessage.NotFound);
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"{ex.Message}.");
        }
    }

    public async Task<ErrorOr<VEmployeeList>> GetEmployeeDetailsByIdAsync(string? id, int? branchId, bool include)
    {
        try
        {

            using var db = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@EmployeeId", id);

            parameters.Add("@BranchId", branchId);

            parameters.Add("@Include", include);

            var sp = ProcedureConstant.SpGetEmployeeByIdWithLoans;

            if (!include)
            {
                var employee = await db.QueryFirstOrDefaultAsync<VEmployeeList>(sp, parameters, commandType: CommandType.StoredProcedure);

                if (employee is null) return Error.NotFound(EmployeeMessage.NotFound);

                return employee;
            }

            using var multiMap = await db.QueryMultipleAsync(sp, parameters, commandType: CommandType.StoredProcedure);

            var existingItem = await multiMap.ReadSingleOrDefaultAsync<VEmployeeList>();

            if (existingItem is null)
            {
                return Error.NotFound(description: "No Employee Found.");
            }

            existingItem.LoanRequests = (await multiMap.ReadAsync<VLoanRequests>()).ToList();

            existingItem.LoanDisbursments = (await multiMap.ReadAsync<VLoanDisbursments>()).ToList();

            return existingItem;

        }
        catch (Exception ex)
        {
            return Error.Unexpected("Server.Error", ex.Message);
        }
    }

    public async Task<ErrorOr<DeleteResult>> DeleteAsync(string? id, int? branchId, CancellationToken cancellation = default)
    {


        _logger.LogInformation("Deleting employee with ID #{0} from branch #{1}", id, branchId);

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        if (await IsEmployeeHasLoanRequest(id, context, cancellation))
        {
            return DeleteResult.InUse(EmployeeMessage.InUsed);
        }

        if (await IsEmployeeHasLoanDisbursement(id, context, cancellation))
        {
            return DeleteResult.InUse(EmployeeMessage.InUsed);
        }

        if(await IsEmployeeAssignAsUser(id,context, cancellation))
        {
            return DeleteResult.InUse(EmployeeMessage.InUsed);
        }


        var employee = await context.EntityNames
                               .Include(x => x.Employee)
                               .FirstOrDefaultAsync(x => x.Id == id, cancellation);
        if (employee is null)
        {
            return Error.NotFound(EmployeeMessage.NotFound);
        }

        try
        {
            context.EntityNames.Remove(employee);

            await context.SaveChangesAsync(cancellation);

            return DeleteResult.Success(EmployeeMessage.Deleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Error.Unexpected(ex.Message);
        }

    }

    private static async Task<bool> IsEmployeeHasLoanDisbursement(string? id, DataContext context, CancellationToken cancellation)
    {
        return await context.LoanDisbursments
                                               .AsNoTracking()
                                               .AnyAsync(ld => ld.CreditAgentRefId == id, cancellation);
    }

    private static async Task<bool> IsEmployeeHasLoanRequest(string? id, DataContext context, CancellationToken cancellation)
    {
        return await context.LoanRequests
                                       .AsNoTracking()
                                       .AnyAsync(lr => lr.CreditAgentRefId == id, cancellation);
    }


    private static async Task<bool> IsEmployeeAssignAsUser(string? id, DataContext context, CancellationToken cancellation)
    {
        return await context.Set<VUser>()
                                       .AsNoTracking()
                                       .AnyAsync(u => u.EmployeeRefId == id, cancellation);
    }


    private IQueryable<VEmployeeList> ApplyFiltering(IQueryable<VEmployeeList> queries, int? branchId, string? employeeId, QueryParam param)
    {
        var filter = param.Filter?.Trim();

        queries = queries.WhereIf(branchId > 0, x => x.BranchId == branchId)
                         .WhereIf(employeeId.IsNotNullOrEmpty(), x => x.EmployeeId == employeeId);

        if (param.IncludeInActive)
        {
            queries = queries.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(filter) && string.IsNullOrEmpty(param.FilterColumn))
        {

            var props = new[] { "FirstName", "LastName", "BranchName", "DepartmentName", "Phone", "AltName" };

            queries = queries.StartWiths(filter, props);
        }

        if (!string.IsNullOrEmpty(param.Filter) && !string.IsNullOrEmpty(param.FilterColumn))
        {
            var property = typeof(VEmployeeList).MapField(param.FilterColumn); //  .GetProperty(param.FilterColumn);

            if (property != null)
            {
                queries = queries.StartWiths(param.Filter, param.FilterColumn);
            }
        }

        return queries;
    }

    private IQueryable<VEmployeeList> ApplyOrdering(IQueryable<VEmployeeList> query, QueryParam param)
    {

        var isDescending = param.SortDescending;

        if (!string.IsNullOrWhiteSpace(param.SortColumn))
        {

            var property = typeof(VEmployeeList).GetProperty(param.SortColumn);

            if (property != null)
            {
                query = isDescending
                    ? query
                        .OrderByDescending(x => x.ModifiedDT)
                        .ThenByDescending(e => EF.Property<object>(e, param.SortColumn))
                    : query
                            .OrderBy(x => x.ModifiedDT)
                            .ThenBy(e => EF.Property<object>(e, param.SortColumn));
            }

            return query;
        }

        query = isDescending ? query.OrderByDescending(x => x.ModifiedDT)
                    .ThenByDescending(x => x.FirstName)
                    .ThenByDescending(x => x.LastName) : query.OrderBy(x => x.ModifiedDT)
                    .ThenBy(e => e.FirstName).ThenBy(e => e.LastName);

        return query;
    }

    private static Expression<Func<VEmployeeList, VEmployeeList>> ProjectionTo()
    {
        return x => new VEmployeeList
        {
            EmployeeId = x.EmployeeId,
            FirstName = x.FirstName,
            LastName = x.LastName,
            AltName = x.AltName,
            Phone = x.Phone,
            DepartmentName = x.DepartmentName,
            EmailAddress = x.EmailAddress,
            BranchName = x.BranchName,
            Occupation = x.Occupation,
            ModifiedDT = x.ModifiedDT,
            IsActive = x.IsActive,
        };
    }

    private static Expression<Func<EntityNames, VEmployeeList>> Projection()
    {
        return x => new VEmployeeList
        {
            EmployeeId = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            AltName = x.AltName,
            Phone = x.Phone,
            DepartmentName = x.Employee!.Department!.DepartmentName,
            EmailAddress = x.EmailAddress,
            BranchName = x.Employee.Branch!.BranchName,
            Occupation = x.Occupation,
            ModifiedDT = x.ModifiedDt,
            IsActive = x.IsActive ?? false,
        };
    }

}

#region commented code

//public async Task<Result<string>> DeleteAsync(string? id, CancellationToken cancellation = default)
//{
//    try
//    {
//        await using var _context = await _dbContextFactory.CreateDbContextAsync(cancellation);

//        var employee = await _context.EntityNames
//                                        .Where(x => x.Id == id)
//                                        .Include(x => x.Employee)
//                                        .FirstOrDefaultAsync();
//        if (employee is null)
//        {
//            return Applications.Responses.Result.Failure<string>(ErrorResponse.NotFound("There is no Employee in then context."));
//        }

//        _context.EntityNames.Remove(employee);

//        await _context.SaveChangesAsync();

//        return Applications.Responses.Result.Success("One Employee was delete from database.");
//    }

//    catch (Exception ex)
//    {
//        return Applications.Responses.Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
//    }
//}


//public async Task<DeleteResult> DeleteAsync(string? id, CancellationToken cancellation = default)
//{
//    try
//    {
//        await using var _context = await _dbContextFactory.CreateDbContextAsync(cancellation);


//        var usedEmployee = await _context.LoanRequests
//                                       .AsNoTracking()
//                                       .AnyAsync(lr => lr.CreditAgentRefId == id, cancellation);


//        if (usedEmployee)
//        {
//            return DeleteResult.InUse("Cannot delete employee who has associated loan requests.");
//        }

//        var employee = await _context.EntityNames
//                                        .Where(x => x.Id == id)
//                                        .Include(x => x.Employee)
//                                        .FirstOrDefaultAsync();
//        if (employee is null)
//        {
//            throw new NotFoundExceptions("No found employee to delete.");
//        }

//        _context.EntityNames.Remove(employee);

//        await _context.SaveChangesAsync();

//        return DeleteResult.Success("Employee was deleted.");
//    }

//    catch (Exception ex)
//    {
//        throw new Exception(ex.Message, ex);
//    }
//}


//public async Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId, string? employeeId, QueryParam q, CancellationToken cancellation = default, params string[] columns)
//{
//    using var connection = await _connectionFactory.CreateConnectionAsync(cancellation);

//    var selectColumns = SqlColumnHelper.BuildSafeColumnList<VEmployeeList>(columns);

//    var parameters = new
//    {
//        BranchId = branchId,
//        EmployeeId = employeeId,
//        Filter = q.Filter,
//        FilterColumn = q.FilterColumn,
//        SortColumn = q.SortColumn,
//        SortDescending = q.SortDescending,
//        SelectColumns = selectColumns,
//        Skip = q.Page,
//        PageSize = q.Take
//    };

//    using var multi = await connection.QueryMultipleAsync("dbo.spGetEmployees_Dynamic",
//                                        parameters,
//                                        commandType: CommandType.StoredProcedure);

//    var totalCount = await multi.ReadSingleAsync<int>();

//    var items = (await multi.ReadAsync<VEmployeeList>()).ToList();

//    return new Pager<VEmployeeList>(items, totalCount);



//}

//public async Task<Result<VEmployeeList>> GetEmployeeByIdAsync(string? employeeId, CancellationToken cancellation = default)
//{

//    _logger.LogInformation("getting employee with ID #{0}", employeeId);

//    await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

//    var item = await context.EmployeeLists
//                            .Include(x => x.LoanDisbursments)
//                            .Include(x => x.LoanRequests)
//                            .IgnoreQueryFilters()
//                            .AsNoTracking()
//                            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId, cancellation);
//    if (item is null)
//    {
//        return Applications.Responses.Result.Failure<VEmployeeList>(ErrorResponse.NotFound(Messages.DataNotFound));
//    }

//    return Applications.Responses.Result.Success(item);
//}

//public async Task<Result<VEmployeeList>> GetByIdAsync(string? employeeId, CancellationToken cancellation = default)
//{

//    _logger.LogInformation("getting employee with ID #{0}", employeeId);

//    if (string.IsNullOrEmpty(employeeId))
//    {
//        return Applications.Responses.Result.Failure<VEmployeeList>(ErrorResponse.NotFound(""));
//    }

//    using var db = await connectionFactory.CreateConnectionAsync(cancellation);

//    var employeeDict = new Dictionary<string, VEmployeeList>();

//    var result = await db.QueryAsync<VEmployeeList, VLoanDisbursments, VLoanRequests, VEmployeeList>(ProcedureConstant.SpGetEmployeeByIdWithLoans,
//        (employee, loan, request) =>
//        {
//            if (!employeeDict.TryGetValue(employee.EmployeeId!, out var emp))
//            {
//                emp = employee;
//                emp.LoanDisbursments = new List<VLoanDisbursments>();
//                emp.LoanRequests = new List<VLoanRequests>();
//                employeeDict.Add(emp.EmployeeId!, emp);
//            }

//            if (loan?.LoanId != null && !emp.LoanDisbursments.Any(x => x.LoanId == loan.LoanId))
//            {
//                emp.LoanDisbursments.Add(loan);
//            }

//            if (request?.LoanRequestId != null && !emp.LoanRequests.Any(x => x.LoanRequestId == request.LoanRequestId))
//            {
//                emp.LoanRequests.Add(request);
//            }

//            return emp;
//        },
//        new { EmployeeId = employeeId, BranchId = 1 },
//        commandType: CommandType.StoredProcedure,
//        splitOn: "LoanId,LoanRequestId");

//    var item = employeeDict.Values.FirstOrDefault(x => x.EmployeeId == employeeId);

//    if (item is null)
//    {
//        return Applications.Responses.Result.Failure<VEmployeeList>(ErrorResponse.NotFound(Messages.DataNotFound));
//    }

//    return Applications.Responses.Result.Success(item);
//}


//public async Task<Result<T>> GetByIdAsync<T>(string? id)
//    where T : class
//{
//    try
//    {
//        await using var context = await _dbContextFactory.CreateDbContextAsync();

//        var employees = await context.EmployeeLists
//            .Where(x => x.EmployeeId == id && x.IsActive == true)
//            .ProjectToType<T>()
//            .AsNoTracking()
//            .FirstOrDefaultAsync();
//        if (employees is not null)
//        {
//            return Result.Success(employees);
//        }

//        return Result.Failure<T>(ErrorResponse.NotFound("No Employee Found."));
//    }
//    catch (Exception ex)
//    {
//        return Result.Failure<T>(ErrorResponse.NotFound($"{ex.Message}."));
//    }
//}
//public async Task<Result<IReadOnlyList<VEmployeeList>>> GetEmployeesAsync(string? employeeId, int branchId = 0, bool isActive = true, string? q = null)
//{

//    await using var _context = await _dbContextFactory.CreateDbContextAsync();

//    IQueryable<VEmployeeList> queries = _context.EmployeeLists;

//    if (!isActive)
//    {
//        queries = queries.Where(x => x.IsActive == true);
//    }

//    if (!string.IsNullOrEmpty(q))
//    {
//        queries = queries.Where(x => EF.Functions.Like(x.LastName!, $"%{q}%")
//                                     || EF.Functions.Like(x.LastName!, $"%{q}%")
//                                     || EF.Functions.Like(x.AltName!, $"%{q}%")
//                                     || EF.Functions.Like(EF.Functions.Collate(x.Phone!, "Khmer_100_CI_AI_SC_UTF8"),
//                                         $"%{q}%")
//                                     || EF.Functions.Like(x.BranchName!, $"%{q}%"));
//    }

//    if (employeeId.IsNotNullOrEmpty())
//    {
//        queries = queries.Where(x => x.EmployeeId == employeeId);
//    }

//    if (branchId > 0)
//    {
//        queries = queries.Where(x => x.BranchId == branchId);
//    }

//    return Applications.Responses.Result.Success<IReadOnlyList<VEmployeeList>>(await queries.AsNoTracking().ToListAsync());
//}

#endregion