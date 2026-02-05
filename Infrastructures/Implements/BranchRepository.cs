using Applications.Bases;
using Applications.Commons.Extensions.QueryableExtensions;
using Applications.Commons.Messages;
using Applications.Extensions;
using Applications.Features.Branches.Dtos;
using Applications.Interfaces.Branches;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Lists.Entities;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Auths;
using Dapper;
using Domains.Enumerations;
using Domains.Models.Lists;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.DataAccess.Extensions;
using Infrastructures.Implements.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;

namespace Infrastructures.Implements;

public class BranchRepository(IDbContextFactory<DataContext> contextFactory,
                    ILogger<BranchRepository> logger,
                    IDapperConnectionFactory connectionFactory) : IBranchService
{

    public async Task<ErrorOr<VBranch>> CreateAsync(Branch branch, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("Try to create new brnach with Name:{0}", branch.BranchName);

            if (await IsBranchExist(null, branch.BranchName))
            {
                return ErrorOr.Error.Conflict("Branch Name is already exist.");
            }

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            context.Branches.Add(branch);

            await context.SaveChangesAsync();

            var created = await context.VBranchs
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.BranchId == branch.BranchId);

            if (created is null)
            {
                logger.LogError("No branch was found with name:{0}", created!.BranchName);

                return ErrorOr.Error.NotFound("Could not created branch...!");
            }

            logger.LogInformation($"New branch was Created with name {0} successfully", created.BranchName);

            return created;
        }
        catch (DbUpdateException ex)
        {
            return Error.Unexpected("ServerError", ex.Message);
        }
    }

    public async Task<ErrorOr<DeleteResult>> DeleteAsync(int? id, CancellationToken cancellation = default)
    {
        try
        {

            if (await IsBranchUsed(id))
            {
                return DeleteResult.InUse("This branch is used in other module, you can't delete it.");
            }

            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var branch = await context.Branches.FindAsync(id);

            if (branch is null)
            {
                return Error.NotFound("There is no branch found.");
            }

            context.Branches.Remove(branch);

            await context.SaveChangesAsync(cancellation);

            return DeleteResult.Success("Branch deleted successfully.");
        }
        catch (Exception ex)
        {
            return Error.Unexpected("ServerError", ex.Message);
        }
    }
    public async Task<ErrorOr<VBranch>> UpdateAsync(Branch branch, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var existingBranch = await context.Branches
                                            .FirstOrDefaultAsync(x => x.BranchId == branch.BranchId &&
                                          x.EditSeq == branch.EditSeq);

            if (existingBranch is null)
            {
                return Error.Failure("One branch was updated Or Branch was chnaged by someone");
            }

            branch.EditSeq = existingBranch.EditSeq + 1;

            context.Entry(existingBranch).CurrentValues.SetValues(branch);

            await context.SaveChangesAsync(cancellation);


            var updatedBranch = await context.VBranchs
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.BranchId == branch.BranchId, cancellation);

            if (updatedBranch is null) return Error.NotFound(BranchMessage.BranchNotFound);

            return updatedBranch;
        }
        catch (Exception ex)
        {
            return Error.Unexpected("Server.Error", ex.InnerException!.Message);
        }
    }
    public async Task<ErrorOr<VBranch>> UpdateStatusAsync(int? branchId, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var existingBranch = await context.Branches.FindAsync(branchId);

            if (existingBranch is null)
            {
                return Error.NotFound(BranchMessage.BranchNotFound);
            }

            existingBranch.IsActive = !existingBranch.IsActive;

            existingBranch.EditSeq = existingBranch.EditSeq + 1;

            context.Branches.Update(existingBranch);

            await context.SaveChangesAsync();

            VBranch? updatedBranch = await GetBranchDetailsById(branchId, cancellation);

            if (updatedBranch is null) return Error.NotFound(BranchMessage.BranchNotFound);

            return updatedBranch;
        }
        catch (Exception ex)
        {
            return Error.Unexpected("ServerError", ex.Message);
        }
    }
    public async Task<ErrorOr<VBranch>> UpdateStatusAsync(int? branchId, bool state, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var existingBranch = await context.Branches.FindAsync(branchId);

            if (existingBranch is null)
            {
                return Error.NotFound(BranchMessage.BranchNotFound);
            }

            existingBranch.IsActive = state;

            existingBranch.ModifiedDt = DateTime.Now;

            await context.SaveChangesAsync();

            var updatedBranch = await GetBranchDetailsById(branchId, cancellation);

            if (updatedBranch is null) return Error.NotFound(BranchMessage.BranchNotFound);

            return updatedBranch;
        }
        catch (Exception ex)
        {
            return Error.Unexpected("ServerError", ex.InnerException!.Message);
        }
    }

    public async Task<VBranch?> GetBranchDetailsById(int? branchId, CancellationToken cancellation = default, params string[] columns)
    {
        using var db = await connectionFactory.CreateConnectionAsync(cancellation);

        var columnList = SqlColumnHelper.BuildSafeColumnList<VBranch>(columns);

        var sql = $"SELECT {columnList} FROM VBranchs WHERE BranchId = @BranchId";

        return await db.QueryFirstOrDefaultAsync<VBranch>(sql, new { BranchId = branchId });
    }

    public async Task<VBranch?> GetBranchDetailsById(int? branchId, Expression<Func<VBranch, object>> selector, CancellationToken cancellation = default)
    {
        using var db = await connectionFactory.CreateConnectionAsync(cancellation);

        var columnList = SqlColumnHelper.BuildColumnList(selector);

        var sql = $"SELECT {columnList} FROM VBranchs WHERE BranchId = @BranchId";

        return await db.QueryFirstOrDefaultAsync<VBranch>(sql, new { BranchId = branchId });
    }

    public async Task<ErrorOr<Branch>> GetBranchById(int? id, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var branch = await context.Branches
                            .IgnoreQueryFilters()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.BranchId == id, cancellation);

        if (branch is null)
        {
            return Error.NotFound(BranchMessage.BranchNotFound);
        }

        return branch;
    }

    public async Task<Pager<VBranch>> GetBranchesAsync(int? branchId, QueryParam param, CancellationToken cancellation = default)
    {

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        IQueryable<VBranch> queries = ApplyFilters(param, context);

        queries = ApplySorts(param, queries);

        int counts = queries.Count();

        if (param.PageSize > 0)
        {
            queries = queries.Skip(param.Skip).Take(param.PageSize);
        }

        var items = await queries.AsNoTracking().ToListAsync(cancellation);

        return new Pager<VBranch>(items, counts);
    }

    public async Task<Pager<VBranch>> GetBranchesAsync(int? branchId, QueryParam param, CancellationToken cancellation = default, params string[] columns)
    {

        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        IQueryable<VBranch> queries = ApplyFilters(param, context);

        queries = ApplySorts(param, queries);

        int counts = queries.Count();

        if (param.PageSize > 0)
        {
            queries = queries.Skip(param.Skip).Take(param.PageSize);
        }

        var items = await queries
                            .AsNoTracking()
                            .ProjectTo<VBranch>(columns)
                            .ToListAsync(cancellation);

        return new Pager<VBranch>(items, counts);
    }



    public async Task<ErrorOr<IEnumerable<VBranch>>> GetBranchesByUser(string? userId, int? branchId, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var user = await context.Database
                            .SqlQuery<VUser>($"SELECT * FROM VUsers WHERE id={userId}")
                            .WhereIf(branchId.HasValue && branchId > 0, x => x.BranchId == branchId)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

        if (user is null)
        {
            return Error.NotFound("No User found in the branch.");
        }

        if (string.IsNullOrEmpty(user.EmployeeRefId))
        {
            return Error.NotFound("No User found in the branch.");
        }

        var queries = context.VBranchs.Where(x => x.BranchId == user.BranchId);

        if (branchId > 0)
        {
            queries = queries.Where(x => x.BranchId == branchId);
        }

        var items = await queries
                            .AsNoTracking()
                            .ToListAsync(cancellation);


        return items;
    }

    public async Task<ErrorOr<BranchStatus>> GetBranchStatus(int? branchId, CancellationToken cancellation = default)
    {
        using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var branch = await context.VBranchs
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.BranchId == branchId &&
                                                    x.IsActive == true, cancellation);

        if (branch is null)
        {
            return Error.NotFound(BranchMessage.BranchNotFound);
        }

        return branch.OpenStatus == (int)BranchStateEnum.Open ? new BranchStatus(true, "Open") : new BranchStatus(false, "Closed");
    }


    public async Task<ErrorOr<BranchStatus>> GetBranchStatus(string? userId, CancellationToken cancellation = default)
    {
        using var context = await connectionFactory.CreateConnectionAsync(cancellation);

        var branch = await context.QueryFirstOrDefaultAsync<BranchStatus>("spGetBranchStatus",
                                    new { UserId = userId },
                                    commandType: CommandType.StoredProcedure);

        if (branch is null)
        {
            return Error.NotFound(BranchMessage.BranchNotFound);
        }

        return branch;

    }



    public async Task<ErrorOr<VBranch>> GetBranchForVault(int? branchId, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellation);

            var branch = await context.VBranchs
                                        .IgnoreAutoIncludes()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.BranchId == branchId, cancellation);
            if (branch is null)
            {
                return Error.NotFound(BranchMessage.BranchNotFound);
            }

            return branch;
        }
        catch (Exception e)
        {
            return Error.Unexpected("ServerError", e.Message);
        }
    }

    public async Task<ErrorOr<BranchDtoUpdate>> GetUpdateBranchById(int? branchId, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var existingBranch = await context
                                        .Branches
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.BranchId == branchId, cancellation);

        if (existingBranch is null)
        {
            return Error.NotFound(BranchMessage.BranchNotFound);
        }

        var item = existingBranch.Adapt<BranchDtoUpdate>();

        return item;
    }

    public async Task<ErrorOr<UserBranchesResponse?>> GetBranchesByUser(string? userId, CancellationToken cancellation = default)
    {

        using var context = await connectionFactory.CreateConnectionAsync(cancellation);

        var sql = @"SELECT u.Id,
                           u.UserName,
                           u.BranchId,
                           b.BranchName    
                    FROM VUsers u
                    WHERE u.Id = @UserId";

        var sqlParam = new DynamicParameters();

        sqlParam.Add("@UserId", userId);

        var user = await context.QueryFirstOrDefaultAsync<UserBranchesResponse>(sql, sqlParam);

        return user;
    }

    public async Task<ErrorOr<IEnumerable<ApprovedUser>>> GetApprovedUser(int? branchId, string? filter, CancellationToken cancellation = default)
    {

        using var context = await connectionFactory.CreateConnectionAsync(cancellation);

        var filterTrim = filter?.Trim();

        var sql = @"SELECT u.Id,
                           u.UserName,
                           CONCAT(u.FirstName, ' ', u.LastName) AS FullName
                    FROM VUsers u
                    WHERE u.BranchId = @BranchId
                    AND u.CanApproved = CAST(1 AS BIT)
                    AND (
                        @Filter IS NULL OR
                        u.UserName LIKE @Filter OR
                        u.FirstName LIKE @Filter OR
                        u.LastName LIKE @Filter
                    )";

        var sqlParam = new DynamicParameters();

        sqlParam.Add("@BranchId", branchId);

        sqlParam.Add("@Filter", $"{filterTrim}%");

        var users = await context.QueryAsync<ApprovedUser>(sql, sqlParam);

        return users.ToList();
    }

    /// <summary>
    /// private fields and methods
    /// </summary>
    /// <param name="param"></param>
    /// <param name="queries"></param>
    /// <returns></returns>
    private static IQueryable<VBranch> ApplySorts(QueryParam param, IQueryable<VBranch> queries)
    {
        bool sortDesc = param.SortDescending;

        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            queries = sortDesc ?
                            queries.OrderByDescending(f => EF.Property<object>(f, param.SortColumn)) :
                            queries.OrderBy(f => EF.Property<object>(f, param.SortColumn));

            return queries;
        }

        queries = sortDesc ?
                queries.OrderByDescending(x => x.BranchName).ThenByDescending(x => x.BranchId) :
                queries.OrderBy(x => x.BranchName).ThenBy(x => x.BranchId);

        return queries;
    }

    private static IQueryable<VBranch> ApplyFilters(QueryParam param, DataContext context)
    {
        var queries = context.VBranchs.AsQueryable();

        if (param.IsIncludeInActive)
        {
            queries = queries.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(param.Filter) && string.IsNullOrEmpty(param.FilterColumn))
        {
            string[] props = { "BranchName", "BranchId" };

            queries = queries.WhereMatchesFilter(param.Filter, props);

        }

        if (!string.IsNullOrEmpty(param.Filter) && !string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.WhereMatchesFilter(param.Filter, param.FilterColumn);
        }




        return queries;
    }

    private async Task<bool> IsBranchExist(int? branchId, string branchName, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var existings = await context.VBranchs
                                    .Where(x => x.BranchName == branchName &&
                                            (!branchId.HasValue || x.BranchId != branchId))
                                    .AsNoTracking()
                                    .ToListAsync(cancellation);

        return existings.Any();
    }

    private async Task<bool> IsBranchUsed(int? branchId, CancellationToken cancellation = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellation);

        var existings = await context.VBranchs
                                    .Where(x => x.BranchId == branchId && x.Used == true)
                                    .AsNoTracking()
                                    .ToListAsync(cancellation);

        return existings.Any();
    }

}