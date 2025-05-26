using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class DepartmentService : IDepartmentService
{
    private readonly DataContext _context;
    private readonly IDapperConnectionFactory dapperFactory;
    public DepartmentService(DataContext app, IDapperConnectionFactory dapperConnectionFactory)
    {
        this._context = app;
        this.dapperFactory = dapperConnectionFactory;
    }

    public async Task<Result<string>> CreateAsync(Department req)
    {
        try
        {
            var dep = await _context.Departments.Where(x => x.DepartmentName != null & x.DepartmentName == req.DepartmentName)
                                   .FirstOrDefaultAsync();
            if (dep is not null) return Result.Failure<string>(ErrorResponse.Conflict("Department already exist."));

            await _context.Departments.AddAsync(req);

            await _context.SaveChangesAsync();

            return Result.Success<string>(new("new department added."));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }
    public async Task<Result<string>> DeleteAsync(int? id)
    {
        try
        {
            var dept = await _context.Departments.FirstOrDefaultAsync(dp => dp.DepartmentId == id);

            if (dept is null) return Result.Failure<string>(ErrorResponse.NotFound("No Department Found."));

            _context.Departments.Remove(dept);

            await _context.SaveChangesAsync();

            return Result.Success<string>(new("department was deleted."));

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }
    public async Task<Result<Department>> GetDepartmentById(int? departmentId)
    {
        var department = await _context.Departments.Where(dept => dept.DepartmentId == departmentId).FirstOrDefaultAsync();

        if (department is null) return Result.Failure<Department>(ErrorResponse.NotFound("No Deapartment found."));

        return Result.Success<Department>(department);
    }
    public async Task<Result<VDepartment>> GetDepartmentDetails(int? departmentId)
    {
        var department = _context.VDepartments.Where(dept => dept.DepartmentId == departmentId);

        if (department is null) return Result.Failure<VDepartment>(ErrorResponse.NotFound("Deapartment found."));

        var dept = await department.AsNoTracking().FirstOrDefaultAsync();

        return Result.Success<VDepartment>(dept!);
    }


    public async Task<Result<IEnumerable<Department>>> GetDepartments(int? departmentId = 0, int take = 0, bool includeInactive = false, string? condition = null)
    {
        IQueryable<Department> depsQuery = _context.Departments;

        if (!includeInactive)
        {
            depsQuery = _context.Departments.Where(x => x.IsActive == true);
        }

        if (condition is not null)
        {
            depsQuery = depsQuery.Where(x => x.DepartmentName != null
                                        && x.DepartmentName.Contains(condition));
        }

        if (departmentId > 0)
        {

            depsQuery = depsQuery.Where(x => x.DepartmentId == departmentId);
        }

        if (take > 0)
        {
            depsQuery = depsQuery.Take(take).OrderByDescending(x => x.DepartmentName);
        }

        var depts = await depsQuery.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<Department>>(depts);

    }


    public async Task<Result<IEnumerable<T>>> GetDepartments<T>(int? departmentId, int take = 10,string? filter=null)
        where T:class
    {
        IQueryable<Department> depsQuery = _context.Departments;


        if (!string.IsNullOrEmpty(filter))
        {
            depsQuery=depsQuery.Where(x=>x.DepartmentName!.Contains(filter));
        }

        if (departmentId > 0)
        {
            depsQuery = depsQuery.Where(x => x.DepartmentId == departmentId);
        }

        if (take > 0)
        {
            depsQuery = depsQuery.Take(take).OrderByDescending(x => x.DepartmentName);
        }

        var depts = await depsQuery
                                .ProjectToType<T>()
                                .AsNoTracking()
                                .ToListAsync();

        return Result.Success<IEnumerable<T>>(depts);

    }


    public async Task<Pager<VDepartment>> GetDepartments(QueryParam param)
    {
        IQueryable<VDepartment> query = _context.VDepartments;

        if (param.IsIncludeInActive == true)
        {
            query = query.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(param.Filter))
        {
            query = query.Where(x => EF.Functions.Like(x.DepartmentName!, $"%{param.Filter}%"));
        }

        if (param.SortDescending)
        {
            query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        var counts = await query.CountAsync();

        var data = await query.Skip(param.PageNumber)
                       .Take(param.PageSize)
                       .AsNoTracking()
                       .ToListAsync();
        return new Pager<VDepartment>(data, counts);

    }
    public async Task<Result<string>> UpdateAsync(Department req)
    {
        try
        {
            var dep = await _context.Departments.Where(x => ((x.DepartmentName != null & x.DepartmentName == req.DepartmentName) &&
                                               x.DepartmentId != req.DepartmentId) &&
                                               x.EditSeq == req.EditSeq).FirstOrDefaultAsync();

            if (dep is not null) return Result.Failure<string>(ErrorResponse.Conflict("Department Name already exist or invalid sequence.!"));

            _context.Departments.Update(req);

            await _context.SaveChangesAsync();

            return Result.Success(new string("department was updated"));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }
    public async Task<Result<string>> ActivateOrDeactivate(int? id, Department req)
    {
        try
        {
            var dep = await _context.Departments
                                   .Where(x => x.DepartmentId != id && x.EditSeq == req.EditSeq)
                                       .FirstOrDefaultAsync();

            if (dep is not null)
            {
                return Result.Failure<string>(ErrorResponse.Conflict("Department Name already exist."));
            }

            _context.Departments.Update(req);

            await _context.SaveChangesAsync();

            return Result.Success<string>(new("department was updated."));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }
}
