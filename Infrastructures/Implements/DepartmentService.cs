using Applications.Extensions;
using Applications.Features.Departments.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Dapper;
using Domains.Models.Lists;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
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

    public async Task<ErrorOr<VDepartment>> CreateAsync(Department req, CancellationToken cancellation = default)
    {

        _logger.LogInformation("Creating new department with name: {DepartmentName}", req.DepartmentName);

        try
        {

            if (await IsDepartmentExist(null, req.DepartmentName))
            {
                return Error.Conflict("Department already exists.");
            }

            await using var context = await _contextFactory.CreateDbContextAsync();

            await context.Departments.AddAsync(req);

            await context.SaveChangesAsync(cancellation);

            return await GetDepartmentDetailsById(req.DepartmentId, 0, cancellation);
        }
        catch (DbUpdateException ex)
        {
            return Error.Unexpected("Error", ex.InnerException!.Message);
        }

    }

    public async Task<bool> ExistsByNameAsync(string name, int branchId, CancellationToken ct)
    {
        return await _context.Departments
            .AnyAsync(x =>
                EF.Functions.Collate(x.DepartmentName, "SQL_Latin1_General_CP1_CI_AS") == name
                                        && x.BranchId == branchId, ct);
    }


    public async Task<ErrorOr<VDepartment>> UpdateAsync(Department req, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Updating department with ID: {DepartmentId}", req.DepartmentId);

        try
        {

            if (await IsDepartmentExist(req.DepartmentId, req.DepartmentName, cancellation))
            {
                return Error.Conflict("Department Name already exist or invalid sequence.");
            }

            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            context.Departments.Update(req);

            await context.SaveChangesAsync(cancellation);

            return await GetDepartmentDetailsById(req.DepartmentId, 0, cancellation);
        }
        catch (DbUpdateException ex)
        {
            return Error.Unexpected("Error", ex.InnerException!.Message);
        }

    }

    public async Task<ErrorOr<VDepartment>> ActivateOrDeactivate(int? id, bool state, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Activating or deactivating department with ID: {DepartmentId}", id);

        try
        {

            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var dep = await context.Departments
                                   .IgnoreQueryFilters()
                                   .FirstOrDefaultAsync(x => x.DepartmentId == id, cancellation);

            if (dep is null)
            {
                return Error.NotFound(Messages.DataNotFound);
            }

            dep.IsActive = state;

            dep.ModifiedDt = DateTime.Now;

            await context.SaveChangesAsync(cancellation);

            return await GetDepartmentDetailsById(dep.DepartmentId, 0, cancellation);

        }
        catch (Exception ex)
        {
            return Error.Unexpected("Error", ex.InnerException!.Message);
        }

    }



    public async Task<ErrorOr<string>> DeleteAsync(int? id, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Deleting department with ID: {DepartmentId}", id);
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var dept = await context.Departments
                                    .IgnoreQueryFilters()
                                    .FirstOrDefaultAsync(x => x.DepartmentId == id, cancellation);

            if (dept is null) return Error.NotFound("No Department Found.");

            if (await IsDepartmentInUsed(id))
            {
                return Error.Validation("Can not delete department because is already used.");
            }

            context.Departments.Remove(dept);

            await context.SaveChangesAsync(cancellation);

            return "department was deleted.";

        }
        catch (Exception ex)
        {
            return Error.Unexpected("Error", ex.InnerException!.Message);
        }

    }

    public async Task<ErrorOr<Department>> GetDepartmentById(int? departmentId, int? branchId, CancellationToken cancellation = default)
    {
        _logger.LogInformation("Fetching department with ID: {DepartmentId}", departmentId);

        await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

        var department = await context.Departments
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(dept => dept.DepartmentId == departmentId);

        if (department is null)
            return ErrorOr.Error.NotFound("No Deapartment found.");

        return department;
    }


    public async Task<ErrorOr<VDepartment>> GetDepartmentDetailsById(int? departmentId, int? branchId, CancellationToken cancellation = default, params string[] columns)
    {
        _logger.LogInformation("Fetching department details with ID: {DepartmentId}", departmentId);

        var context = await _dapperFactory.CreateConnectionAsync(cancellation);

        var columnSql = SqlColumnHelper.BuildSafeColumnList<VDepartment>(columns);

        var sql = $@"
            SELECT {columnSql}
            FROM VDepartments
            WHERE DepartmentId = @DepartmentId OR (@BranchId IS NULL OR BranchId = @BranchId)
        ";

        var department = await context.QueryFirstOrDefaultAsync<VDepartment>(sql, new { DepartmentId = departmentId, BranchId = branchId });

        if (department is null) return ErrorOr.Error.NotFound("No Department found.");

        return department;
    }

    public async Task<Pager<VDepartment>> GetDepartments(int? departmentId, QueryParam param, CancellationToken cancellation = default)
    {

        _logger.LogInformation("Fetching paginated departments with parameters - Skip: {Skip}, PageSize: {PageSize}, Filter: {Filter}, SortColumn: {SortColumn}, SortDescending: {SortDescending}, IsIncludeInActive: {IsIncludeInActive}", param.Page, param.Take, param.Filter, param.SortColumn, param.SortDescending, param.IncludeInActive);

        await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

        var query = ApplyFilter(departmentId, param, context);

        query = ApplyOrder(param, query);

        var counts = await query.CountAsync();

        if (param.Page > 0)
        {
            query = query.Skip(param.Page).Take(param.Take);
        }

        var data = await query.AsNoTracking().ToListAsync(cancellation);

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

    private static IQueryable<VDepartment> ApplyOrder(QueryParam param, IQueryable<VDepartment> query)
    {

        var sort = param.SortDescending;

        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            query = sort ?
                    query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn)) :
                    query.OrderBy(x => EF.Property<object>(x, param.SortColumn));
        }
        else
        {
            query = sort ?
                    query.OrderByDescending(x => x.DepartmentName) :
                    query.OrderBy(x => x.DepartmentName);
        }

        return query;
    }

    private static IQueryable<VDepartment> ApplyFilter(int? departmentId, QueryParam param, DataContext context)
    {
        var query = context.VDepartments.WhereIf(departmentId.HasValue && departmentId > 0, x => x.DepartmentId == departmentId);

        if (param.IncludeInActive == true)
        {
            query = query.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(param.Filter))
        {
            string[] filterColumns = ["DepartmentName", "Description"];

            query = query.StartWiths(param.Filter, filterColumns);
        }

        return query;
    }

}

//public async Task<Pager<VDepartment>> GetDepartments(QueryParam param)
//{

//    _logger.LogInformation("Fetching paginated departments with parameters - Skip: {Skip}, PageSize: {PageSize}, Filter: {Filter}, SortColumn: {SortColumn}, SortDescending: {SortDescending}, IsIncludeInActive: {IsIncludeInActive}", param.Skip, param.PageSize, param.Filter, param.SortColumn, param.SortDescending, param.IsIncludeInActive);

//    await using var context = await _contextFactory.CreateDbContextAsync();


//    IQueryable<VDepartment> query = context.VDepartments;

//    if (param.IsIncludeInActive == true)
//    {
//        query = query.IgnoreQueryFilters();
//    }

//    if (!string.IsNullOrEmpty(param.Filter))
//    {

//        query = query.Where(x =>
//            EF.Functions.Like(x.DepartmentId.ToString()!, $"%{param.Filter}%") ||
//            EF.Functions.Like(x.DepartmentName!, $"%{param.Filter}%") ||
//            EF.Functions.Like(x.Description!, $"%{param.Filter}%")
//        );
//    }

//    if (param.SortDescending)
//    {
//        query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
//    }
//    else
//    {
//        query = query.OrderBy(x => EF.Property<object>(x, param.SortColumn));
//    }

//    var counts = await query.CountAsync();

//    var data = await query.Skip(param.Skip)
//                   .Take(param.PageSize)
//                   .AsNoTracking()
//                   .ToListAsync();
//    return new Pager<VDepartment>(data, counts);

//}
