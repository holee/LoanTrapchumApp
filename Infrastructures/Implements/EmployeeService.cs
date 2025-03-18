using Applications.Interfaces.Employees;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Lists.Employees;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;
    public EmployeeService(ApplicationDbContext app)
    {
        this._context = app;
    }
    public async Task<Result<string>> CreateOrUpdateAsync(Employee employee)
    {
        try
        {

            if (employee.EmployeeId == 0)
            {
                _context.Employees.Add(employee);
            }
            else
            {
                var emp = _context.Employees.AsNoTracking()
                    .Where(x => x.EmployeeId == employee.EmployeeId && x.EditSeq == employee.EditSeq)
                    .FirstOrDefault();

                if (emp is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("There is no employee in the context or Someone Has Chnaged it."));
                }
                _context.Employees.Update(employee);
            }

            await _context.SaveChangesAsync();

            return Result.Success<string>(new("New Employee was Added Or Updated."));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.Failure($"{ex.InnerException!.Message}"));
        }

    }
    public async Task<Result<string>> DeleteAsync(long id)
    {
        try
        {
            var employee = _context.Employees.AsNoTracking().Where(x => x.EmployeeId == id).FirstOrDefault();
            if (employee is null)
                return Result.Failure<string>(ErrorResponse.NotFound("There is no Employee in then context."));
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Result.Success("One Employee was delete from database.");
        }

        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.NotFound($"{ex.InnerException!.Message}"));
        }
    }
    public async Task<Result<string>> UpdateIsActiveAsync(long id)
    {
        try
        {

            var employee = await _context.Employees.AsNoTracking()
                                                .IgnoreQueryFilters()
                                                .FirstOrDefaultAsync(emp => emp.EmployeeId == id);
            if (employee == null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound("No Employee found."));
            }
            employee.EditSeq = employee.EditSeq + 1;
            employee.IsActive = employee.IsActive!.Value ? false : true;
            employee.ModifiedDt = DateTime.Now;
            _context.Employees.Update(employee);
            var rowEffect = await _context.SaveChangesAsync();
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
    public async Task<Result<Employee>> GetByIdAsync(long employeeId)
    {
        try
        {
            IQueryable<Employee> entitiesQuery = _context.Employees
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .Where(x => x.EmployeeId == employeeId);

            if (entitiesQuery.Any())
            {
                var employees = await entitiesQuery.AsNoTracking().FirstOrDefaultAsync();

                return Result.Success(employees!);
            }
            return Result.Failure<Employee>(ErrorResponse.NotFound("No Employee Found."));
        }
        catch (Exception ex)
        {
            return Result.Failure<Employee>(ErrorResponse.NotFound($"{ex.Message}."));
        }

    }
    public async Task<Result<IReadOnlyList<VEmployees>>> GetEmployeesAsync(long employeeId = 0,
        int branchId = 0,
        bool isActive = true,
        string? q = null)
    {
        IQueryable<VEmployees> queries = _context.EmployeeLists;

        if (!isActive)
        {
            queries = queries.Where(x => x.IsActive == true);
        }

        if (!string.IsNullOrEmpty(q))
        {
            queries = queries.Where(x => EF.Functions.Like(x.LastName!, $"%{q}%")
                                        || EF.Functions.Like(x.LastName!, $"%{q}%")
                                        || EF.Functions.Like(x.AltName!, $"%{q}%")
                                        || EF.Functions.Like(EF.Functions.Collate(x.Phone!, "Khmer_100_CI_AI_SC_UTF8"), $"%{q}%")
                                        || EF.Functions.Like(x.BranchName!, $"%{q}%"));
        }
        if (employeeId > 0)
        {
            queries = queries.Where(x => x.EmployeeId == employeeId);
        }

        if (branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }
        return Result.Success<IReadOnlyList<VEmployees>>(await queries.AsNoTracking().ToListAsync());
    }
    public async Task<Pager<VEmployees>> GetEmployeesAsync(int pageIndex, int pageSize,
        string sortColumn,
        bool isDecending = false,
        bool includeInActive = false,
        string? q = null)
    {
        IQueryable<VEmployees> queries = _context.EmployeeLists;

        if (includeInActive)
        {
            queries = queries.IgnoreQueryFilters();
        }
        if (!string.IsNullOrWhiteSpace(q))
        {
            queries = queries.Where(x => EF.Functions.Like(x.LastName!, $"%{q}%") ||
                                         EF.Functions.Like(x.FirstName!, $"%{q}%") ||
                                         EF.Functions.Like(x.BranchName!, $"%{q}%") ||
                                         EF.Functions.Like(x.DepartmentName!, $"%{q}%") ||
                                         EF.Functions.Like(x.Phone!, $"%{q}%"));
        }
        if (isDecending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, sortColumn));
        }
        var counts = await queries.CountAsync();
        var items = await queries.Skip(pageIndex)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
        return new Pager<VEmployees>(items, counts);
    }


}
