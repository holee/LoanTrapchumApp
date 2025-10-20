using Applications.Extensions;
using Applications.Interfaces.Employees;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Dapper;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using Domains.Models.Lists;

namespace Infrastructures.Implements;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IDapperConnectionFactory connectionFactory;
    private readonly IDbContextFactory<DataContext> _dbContextFactory;
    private readonly ILogger<EmployeeService> _logger;
    public EmployeeService(IDapperConnectionFactory connectionFactory,
        IDbContextFactory<DataContext> dbContextFactory, ILogger<EmployeeService> logger)
    {
        this.connectionFactory = connectionFactory;
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task<Result<string>> CreateAsync(EntityNames employee, CancellationToken cancellation = default)
    {
        try
        {
            await using var _context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            _context.EntityNames.Add(employee);

            await _context.SaveChangesAsync();

            return Result.Success<string>(new("New Employee was Added."));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.Failure($"{ex.InnerException!.Message}"));
        }
    }

    public async Task<Result<string>> UpdateAsync(EntityNames employee, CancellationToken cancellation = default)
    {
        try
        {

            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);


            context.EntityNames.Update(employee);

            //_context.Entry(employee).State = EntityState.Modified;

            //_context.Entry(employee.Employee!).State = EntityState.Modified;

            await context.SaveChangesAsync(cancellation);

            return Result.Success<string>(new("New Employee was Updated."));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.Failure($"{ex.InnerException!.Message}"));
        }
    }


    public async Task<Result<string>> DeleteAsync(string? id, CancellationToken cancellation = default)
    {
        try
        {
            await using var _context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var employee = await _context.EntityNames
                                            .Where(x => x.Id == id)
                                            .Include(x => x.Employee)
                                            .FirstOrDefaultAsync();
            if (employee is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("There is no Employee in then context."));
            }

            _context.EntityNames.Remove(employee);

            await _context.SaveChangesAsync();

            return Result.Success("One Employee was delete from database.");
        }

        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
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
                return Result.Failure<string>(ErrorResponse.NotFound("No Employee found."));
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
                return Result.Success<string>(new("Employee was updated!"));
            }

            return Result.Failure<string>(ErrorResponse.ServerError("Cannot update employee"));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
        }
    }

    public async Task<Result<IReadOnlyList<VEmployeeList>>> GetEmployeesAsync(string? employeeId,int branchId = 0,bool isActive = true,string? q = null)
    {

        await using var _context = await _dbContextFactory.CreateDbContextAsync();

        IQueryable<VEmployeeList> queries = _context.EmployeeLists;

        if (!isActive)
        {
            queries = queries.Where(x => x.IsActive == true);
        }

        if (!string.IsNullOrEmpty(q))
        {
            queries = queries.Where(x => EF.Functions.Like(x.LastName!, $"%{q}%")
                                         || EF.Functions.Like(x.LastName!, $"%{q}%")
                                         || EF.Functions.Like(x.AltName!, $"%{q}%")
                                         || EF.Functions.Like(EF.Functions.Collate(x.Phone!, "Khmer_100_CI_AI_SC_UTF8"),
                                             $"%{q}%")
                                         || EF.Functions.Like(x.BranchName!, $"%{q}%"));
        }

        if (employeeId.IsNotNullOrEmpty())
        {
            queries = queries.Where(x => x.EmployeeId == employeeId);
        }

        if (branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }

        return Result.Success<IReadOnlyList<VEmployeeList>>(await queries.AsNoTracking().ToListAsync());
    }

    public async Task<Pager<VEmployeeList>> GetEmployeesAsync(int? branchId, string? employeeId, QueryParam q)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var queries = context.EmployeeLists
                              .WhereIf(branchId > 0, x => x.BranchId == branchId)
                              .WhereIf(employeeId.IsNotNullOrEmpty(), x => x.EmployeeId == employeeId)
                              .IgnoreFilterIf(q.IsIncludeInActive);
        if (!string.IsNullOrWhiteSpace(q.Filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.LastName!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(x.FirstName!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(x.BranchName!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(x.DepartmentName!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(x.Phone!, $"%{q}%") ||
                                         EF.Functions.Like(x.AltName!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(x.EmailAddress!, $"%{q.Filter}%") ||
                                         EF.Functions.Like(x.EmployeeId!, $"%{q}%")
            );
        }

        if (q.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, q.SortColumn));
        }

        var counts = queries.Count();

        if (q.PageSize > 0)
        {
            queries = queries.Skip(q.Skip).Take(q.PageSize);
        }

        var items = await queries.AsNoTracking()
                                 .ToListAsync();
        return new Pager<VEmployeeList>(items, counts);
    }


    public async Task<Result<EntityNames>> GetByIdAsync(string? id)
    {
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var employees = await context.EntityNames
                .Include(e => e.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employees is not null)
            {
                return Result.Success(employees);
            }

            return Result.Failure<EntityNames>(ErrorResponse.NotFound("No Employee Found."));
        }
        catch (Exception ex)
        {
            return Result.Failure<EntityNames>(ErrorResponse.NotFound($"{ex.Message}."));
        }
    }


    public async Task<Result<T>> GetByIdAsync<T>(string? id)
        where T : class
    {
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var employees = await context.EmployeeLists
                .Where(x => x.EmployeeId == id && x.IsActive == true)
                .ProjectToType<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (employees is not null)
            {
                return Result.Success(employees);
            }

            return Result.Failure<T>(ErrorResponse.NotFound("No Employee Found."));
        }
        catch (Exception ex)
        {
            return Result.Failure<T>(ErrorResponse.NotFound($"{ex.Message}."));
        }
    }


    //spGetLoansByEmployeeId

    public async Task<Result<VEmployeeList>> GetByIdAsync(string? id, int? branchId, bool include)
    {
        try
        {
            if (id.IsNullOrEmpty())
            {
                return Result.Failure<VEmployeeList>(ErrorResponse.NotFound("No Employee Found."));
            }

            using var db = connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@EmployeeId", id);

            parameters.Add("@BranchId", branchId);

            parameters.Add("@Include", include);

            if (!include)
            {
                var employee = await db.QueryFirstOrDefaultAsync<VEmployeeList>(ProcedureConstant.SpGetEmployeeByIdWithLoans,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (employee is not null)
                {
                    return Result.Success(employee);
                }

                return Result.Failure<VEmployeeList>(ErrorResponse.NotFound("No Employee Found."));
            }

            using (var multiMap = await db.QueryMultipleAsync("spGetEmployeeByIdWithLoans", parameters,
                       commandType: CommandType.StoredProcedure))
            {
                var existingItem = await multiMap.ReadSingleOrDefaultAsync<VEmployeeList>();

                var loanRequests = (await multiMap.ReadAsync<VLoanRequests>()).ToList();

                var loanDisbursments = (await multiMap.ReadAsync<VLoanDisbursments>()).ToList();

                if (existingItem is null)
                {
                    return Result.Failure<VEmployeeList>(ErrorResponse.NotFound("No Employee Found."));
                }

                existingItem.LoanRequests = loanRequests ?? [];

                existingItem.LoanDisbursments = loanDisbursments ?? [];

                return Result.Success(existingItem);
            }
        }
        catch (Exception ex)
        {
            return Result.Failure<VEmployeeList>(ErrorResponse.NotFound($"{ex.Message}."));
        }
    }



    public async Task<Result<VEmployeeList>> GetByIdAsync(string? employeeId, CancellationToken cancellation = default)
    {

        _logger.LogInformation("getting employee with ID #{0}", employeeId);

        if (string.IsNullOrEmpty(employeeId))
        {
            return Result.Failure<VEmployeeList>(ErrorResponse.NotFound(""));
        }

        using var db = await connectionFactory.CreateConnectionAsync(cancellation);

        var employeeDict = new Dictionary<string, VEmployeeList>();

        var result = await db.QueryAsync<VEmployeeList, VLoanDisbursments, VLoanRequests, VEmployeeList>(ProcedureConstant.SpGetEmployeeByIdWithLoans,
            (employee, loan, request) =>
            {
                if (!employeeDict.TryGetValue(employee.EmployeeId!, out var emp))
                {
                    emp = employee;
                    emp.LoanDisbursments = new List<VLoanDisbursments>();
                    emp.LoanRequests = new List<VLoanRequests>();
                    employeeDict.Add(emp.EmployeeId!, emp);
                }

                if (loan?.LoanId != null && !emp.LoanDisbursments.Any(x => x.LoanId == loan.LoanId))
                {
                    emp.LoanDisbursments.Add(loan);
                }

                if (request?.LoanRequestId != null && !emp.LoanRequests.Any(x => x.LoanRequestId == request.LoanRequestId))
                {
                    emp.LoanRequests.Add(request);
                }

                return emp;
            },
            new { EmployeeId = employeeId, BranchId = 1 },
            commandType: CommandType.StoredProcedure,
            splitOn: "LoanId,LoanRequestId");

        var item = employeeDict.Values.FirstOrDefault(x => x.EmployeeId == employeeId);

        if (item is null)
        {
            return Result.Failure<VEmployeeList>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        return Result.Success(item);
    }



    public async Task<Result<VEmployeeList>> GetEmployeeByIdAsync(string? employeeId, CancellationToken cancellation = default)
    {

        _logger.LogInformation("getting employee with ID #{0}", employeeId);

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var item = await context.EmployeeLists
                                .Include(x => x.LoanDisbursments)
                                .Include(x => x.LoanRequests)
                                .IgnoreQueryFilters()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId, cancellation);
        if (item is null)
        {
            return Result.Failure<VEmployeeList>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        return Result.Success(item);
    }



}