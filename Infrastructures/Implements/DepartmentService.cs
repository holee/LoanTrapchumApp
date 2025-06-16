using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Departments;
using Domains.Models.Lists;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public class DepartmentService : IDepartmentService
{
    private readonly DataContext _context;
    private readonly IDapperConnectionFactory _dapperFactory;
    private readonly IDbContextFactory<DataContext> _contextFactory;
    private readonly ILogger<DepartmentService> _logger;
    public DepartmentService(DataContext app, IDapperConnectionFactory dapperConnectionFactory, IDbContextFactory<DataContext> contextFactory, ILogger<DepartmentService> logger)
    {
        _context = app;
        _dapperFactory = dapperConnectionFactory;
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<Result<string>> CreateAsync(Department req, CancellationToken cancellation = default)
    {

        _logger.LogInformation("Creating new department with name: {DepartmentName}", req.DepartmentName);

        try
        {

            if (await IsDepartmentExist(null, req.DepartmentName))
            {
                return Result.Failure<string>(ErrorResponse.Conflict("Department already exists."));
            }

            await using var context = await _contextFactory.CreateDbContextAsync();

            await context.Departments.AddAsync(req);

            await context.SaveChangesAsync();

            return Result.Success<string>(new("new department added."));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }

    public async Task<Result<string>> UpdateAsync(Department req, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Updating department with ID: {DepartmentId}", req.DepartmentId);

        try
        {

            if (await IsDepartmentExist(req.DepartmentId, req.DepartmentName, cancellation))
            {
                return Result.Failure<string>(ErrorResponse.Conflict("Department Name already exist or invalid sequence.!"));
            }

            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            context.Departments.Update(req);

            await context.SaveChangesAsync(cancellation);

            return Result.Success(new string("department was updated"));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }

    public async Task<Result<string>> ActivateOrDeactivate(int? id, bool state)
    {
        _logger.LogInformation("Activating or deactivating department with ID: {DepartmentId}", id);

        try
        {

            await using var context = await _contextFactory.CreateDbContextAsync();

            var dep = await context.Departments
                                   .IgnoreQueryFilters()
                                   .FirstOrDefaultAsync(x => x.DepartmentId == id);

            if (dep is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            dep.IsActive = state;

            dep.ModifiedDt = DateTime.Now;

            await context.SaveChangesAsync();

            return Result.Success<string>(new("department was updated."));

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }



    public async Task<Result<string>> DeleteAsync(int? id, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Deleting department with ID: {DepartmentId}", id);
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var dept = await context.Departments
                                    .IgnoreQueryFilters()
                                    .FirstOrDefaultAsync(x => x.DepartmentId == id, cancellation);

            if (dept is null) return Result.Failure<string>(ErrorResponse.NotFound("No Department Found."));

            if (await IsDepartmentInUsed(id))
            {
                return Result.Failure<string>(ErrorResponse.Validation("Can not delete department because is already used."));
            }

            context.Departments.Remove(dept);

            await context.SaveChangesAsync();

            return Result.Success<string>(new("department was deleted."));

        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.InnerException!.Message}"));
        }

    }
    public async Task<Result<Department>> GetDepartmentById(int? departmentId)
    {
        _logger.LogInformation("Fetching department with ID: {DepartmentId}", departmentId);

        await using var context = await _contextFactory.CreateDbContextAsync();

        var department = await context.Departments.Where(dept => dept.DepartmentId == departmentId).FirstOrDefaultAsync();

        if (department is null) return Result.Failure<Department>(ErrorResponse.NotFound("No Deapartment found."));

        return Result.Success<Department>(department);
    }
    public async Task<Result<VDepartment>> GetDepartmentDetails(int? departmentId)
    {
        _logger.LogInformation("Fetching department details with ID: {DepartmentId}", departmentId);

        await using var context = await _contextFactory.CreateDbContextAsync();

        var department = context.VDepartments
                                .IgnoreQueryFilters()
                                .Where(dept => dept.DepartmentId == departmentId)
                                .AsNoTracking()
                                .FirstOrDefault();

        if (department is null) return Result.Failure<VDepartment>(ErrorResponse.NotFound("Deapartment found."));

        return Result.Success<VDepartment>(department);
    }
    public async Task<Result<IEnumerable<Department>>> GetDepartments(int? departmentId = 0, int take = 0, bool includeInactive = false, string? condition = null)
    {

        _logger.LogInformation("Fetching departments with parameters - DepartmentId: {DepartmentId}, Take: {Take}, IncludeInactive: {IncludeInactive}, Condition: {Condition}", departmentId, take, includeInactive, condition);

        await using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<Department> depsQuery = context.Departments;

        if (!includeInactive)
        {
            depsQuery = context.Departments.Where(x => x.IsActive == true);
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
    public async Task<Result<IEnumerable<T>>> GetDepartments<T>(int? departmentId, int take = 10, string? filter = null)
        where T : class
    {

        _logger.LogInformation("Fetching departments of type {Type} with parameters - DepartmentId: {DepartmentId}, Take: {Take}, Filter: {Filter}", typeof(T).Name, departmentId, take, filter);

        await using var context = await _contextFactory.CreateDbContextAsync();


        IQueryable<Department> depsQuery = context.Departments;


        if (!string.IsNullOrEmpty(filter))
        {
            depsQuery = depsQuery.Where(x => x.DepartmentName!.Contains(filter));
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

        _logger.LogInformation("Fetching paginated departments with parameters - Skip: {Skip}, PageSize: {PageSize}, Filter: {Filter}, SortColumn: {SortColumn}, SortDescending: {SortDescending}, IsIncludeInActive: {IsIncludeInActive}", param.Skip, param.PageSize, param.Filter, param.SortColumn, param.SortDescending, param.IsIncludeInActive);

        await using var context = await _contextFactory.CreateDbContextAsync();


        IQueryable<VDepartment> query = context.VDepartments;

        if (param.IsIncludeInActive == true)
        {
            query = query.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(param.Filter))
        {

            query = query.Where(x =>
                EF.Functions.Like(x.DepartmentId.ToString()!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.DepartmentName!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.Description!, $"%{param.Filter}%")
            );
        }

        if (param.SortDescending)
        {
            query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }
        else
        {
            query = query.OrderBy(x => EF.Property<object>(x, param.SortColumn));
        }

        var counts = await query.CountAsync();

        var data = await query.Skip(param.Skip)
                       .Take(param.PageSize)
                       .AsNoTracking()
                       .ToListAsync();
        return new Pager<VDepartment>(data, counts);

    }

    private async Task<bool> IsDepartmentInUsed(int? departmentId, CancellationToken cancellation = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

        var existing = await context.VDepartments
                            .IgnoreQueryFilters()
                            .Where(x => x.DepartmentId == departmentId &&
                                        x.Used == true)
                            .AsNoTracking()
                            .ToListAsync();

        return existing.Any();


    }

    private async Task<bool> IsDepartmentExist(int? id, string departmentName, CancellationToken cancellation = default)
    {

        await using var context = await _contextFactory.CreateDbContextAsync();

        var dep = await context.Departments
                               .Where(x => (x.DepartmentName == departmentName) &&
                                           (!id.HasValue || x.DepartmentId != id))
                               .AsNoTracking()
                               .ToListAsync();
        return dep.Any();

    }
}
